using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class ClienteController : MonoBehaviour
    {
        [Header("Referências do pedido")]
        public SpriteRenderer spritePedidoLanche;   // balão com imagem do lanche
        public SpriteRenderer spritePedidoSuco;     // ícone do suco (ligado/desligado)

        [Header("Catálogo de pedidos possíveis")]
        public Sprite[] spritesPedidosPossiveis;    // arrasta aqui suas imagens de pedidos
        [Range(0f, 1f)]
        public float chanceQuerSuco = 0.5f;         // 50% das vezes ele quer suco

        [Header("Configuração de pontuação")]
        public int pontosSucoCorreto = 5;

        [Header("Debug")]
        public string codigoPedidoLanche;   // preenchido em runtime
        public bool querSuco;

        [HideInInspector] public ClienteSpawner spawnerOrigem;

        void Awake()
        {
            // 1) Primeiro gera um pedido aleatório para este cliente
            GerarPedidoAleatorio();

            // 2) Depois lê as infos para preencher as variáveis usadas na pontuação

            // Código do lanche = nome do sprite do pedido
            if (spritePedidoLanche != null && spritePedidoLanche.sprite != null)
            {
                string nomeOriginal = spritePedidoLanche.sprite.name;
                codigoPedidoLanche = nomeOriginal == null ? nomeOriginal : nomeOriginal.Replace("_0", "");
            }
            else
            {
                codigoPedidoLanche = string.Empty;
            }

            // Quer suco = se o sprite de suco está habilitado
            if (spritePedidoSuco != null)
            {
                querSuco = spritePedidoSuco.enabled;
            }
            else
            {
                querSuco = false;
            }
        }

        // Gera o pedido aleatório (lanche + se quer suco)
        private void GerarPedidoAleatorio()
        {
            // Escolhe aleatoriamente um sprite de lanche
            if (spritesPedidosPossiveis != null &&
                spritesPedidosPossiveis.Length > 0 &&
                spritePedidoLanche != null)
            {
                int idx = UnityEngine.Random.Range(0, spritesPedidosPossiveis.Length);
                Sprite escolhido = spritesPedidosPossiveis[idx];

                spritePedidoLanche.sprite = escolhido;
                spritePedidoLanche.enabled = true; // garante que o balão aparece
            }

            // Decide aleatoriamente se ele quer suco
            bool quer = UnityEngine.Random.value < chanceQuerSuco;

            if (spritePedidoSuco != null)
            {
                spritePedidoSuco.enabled = quer;
            }
        }

        // Registra a entrega da bandeja para este cliente e retorna a pontuação

        void OnDestroy()
        {
            if (spawnerOrigem != null)
                spawnerOrigem.ClienteFoiDestruido();
        }



        public int RegistrarEntrega(bool sucoEntregue, string codigoLancheEntregue, out bool pedidoPerfeito)
        {
            // Pontuação do lanche (ingredientes)
            int acertosExatos;
            int acertosForaDePosicao;

            int pontosLanche = PedidoPontuacaoUtils.CalcularPontuacao(
                codigoPedidoLanche,
                codigoLancheEntregue,
                out acertosExatos,
                out acertosForaDePosicao
            );

            // Pontuação do suco
            int pontosSuco = 0;
            bool sucoCorreto = true;

            if (querSuco)
            {
                if (sucoEntregue)
                {
                    pontosSuco += pontosSucoCorreto;
                    sucoCorreto = true;
                }
                else
                {
                    sucoCorreto = false; // queria suco mas não recebeu
                }
            }
            else
            {
                // Cliente não queria suco
                if (sucoEntregue)
                {
                    sucoCorreto = false; // entregou suco sem precisar
                }
                else
                {
                    sucoCorreto = true;
                }
            }

            int pontosTotais = pontosLanche + pontosSuco;

            // Pedido perfeito = lanche exatamente igual + suco correto
            bool lanchePerfeito = (codigoPedidoLanche == codigoLancheEntregue);
            pedidoPerfeito = lanchePerfeito && sucoCorreto;

            Debug.Log(
                $"Cliente {name} recebeu pedido. " +
                $"Lanche entregue = {codigoLancheEntregue}, " +
                $"pedido = {codigoPedidoLanche}, " +
                $"pontos = {pontosTotais}, perfeito = {pedidoPerfeito}"
            );

            return pontosTotais;
        }
    }
}