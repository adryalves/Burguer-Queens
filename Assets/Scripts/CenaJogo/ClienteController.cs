using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class ClienteController : MonoBehaviour
    {
        [Header("Referências do pedido")]
        public SpriteRenderer spritePedidoLanche;    
        public SpriteRenderer spritePedidoSuco;    
        [Header("Catálogo de pedidos possíveis")]
        public Sprite[] spritesPedidosPossiveis;   
        [Range(0f, 1f)]
        public float chanceQuerSuco = 0.5f;       

        [Header("Configuração de pontuação")]
        public int pontosSucoCorreto = 5;

        [Header("Debug")]
        public string codigoPedidoLanche; 
        public bool querSuco;

        [HideInInspector] public ClienteSpawner spawnerOrigem;

        void Awake()
        {
            GerarPedidoAleatorio();
            if (spritePedidoLanche != null && spritePedidoLanche.sprite != null)
            {
                string nomeOriginal = spritePedidoLanche.sprite.name;
                codigoPedidoLanche = nomeOriginal == null ? nomeOriginal : nomeOriginal.Replace("_0", "");
            }
            else
            {
                codigoPedidoLanche = string.Empty;
            }

            if (spritePedidoSuco != null)
            {
                querSuco = spritePedidoSuco.enabled;
            }
            else
            {
                querSuco = false;
            }
        }

        private void GerarPedidoAleatorio()
        {
            if (spritesPedidosPossiveis != null &&
                spritesPedidosPossiveis.Length > 0 &&
                spritePedidoLanche != null)
            {
                int idx = UnityEngine.Random.Range(0, spritesPedidosPossiveis.Length);
                Sprite escolhido = spritesPedidosPossiveis[idx];

                spritePedidoLanche.sprite = escolhido;
                spritePedidoLanche.enabled = true; 
            }

            bool quer = UnityEngine.Random.value < chanceQuerSuco;

            if (spritePedidoSuco != null)
            {
                spritePedidoSuco.enabled = quer;
            }
        }

        void OnDestroy()
        {
            if (spawnerOrigem != null)
                spawnerOrigem.ClienteFoiDestruido();
        }



        public int RegistrarEntrega(bool sucoEntregue, string codigoLancheEntregue, out bool pedidoPerfeito)
        {
            int acertosExatos;
            int acertosForaDePosicao;

            int pontosLanche = PedidoPontuacaoUtils.CalcularPontuacao(
                codigoPedidoLanche,
                codigoLancheEntregue,
                out acertosExatos,
                out acertosForaDePosicao
            );

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
                    sucoCorreto = false; 
                }
            }
            else
            {
                if (sucoEntregue)
                {
                    sucoCorreto = false; 
                }
                else
                {
                    sucoCorreto = true;
                }
            }

            int pontosTotais = pontosLanche + pontosSuco;

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