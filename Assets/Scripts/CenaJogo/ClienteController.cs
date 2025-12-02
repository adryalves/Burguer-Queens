using UnityEngine;
using Assets.Scripts.CenaJogo;

namespace Assets.Scripts.CenaJogo
{
    public class ClienteController : MonoBehaviour
    {
        [Header("Referências do pedido")]
        public SpriteRenderer spritePedidoLanche;   // sprite cujo nome é o código do pedido, ex: "BCQP"
        public SpriteRenderer spritePedidoSuco;     // sprite do suco (enabled = quer suco)

        [Header("Configuração de pontuação")]
        public int pontosSucoCorreto = 5;

        [Header("Debug")]
        public string codigoPedidoLanche;   // preenchido em runtime
        public bool querSuco;

        [HideInInspector] public ClienteSpawner spawnerOrigem;


        void Awake()
        {
            // Pega o código do lanche baseado no nome da sprite
            if (spritePedidoLanche != null && spritePedidoLanche.sprite != null)
            {
                codigoPedidoLanche = spritePedidoLanche.sprite.name;
            }
            else
            {
                codigoPedidoLanche = string.Empty;
            }

            // Decide se o cliente quer suco
            querSuco = spritePedidoSuco != null && spritePedidoSuco.enabled;
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
