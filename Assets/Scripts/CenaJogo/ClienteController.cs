using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    using UnityEngine;

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

            void Awake()
            {
               
                if (spritePedidoLanche != null && spritePedidoLanche.sprite != null)
                {
                    codigoPedidoLanche = spritePedidoLanche.sprite.name; 
                }
                else
                {
                    codigoPedidoLanche = string.Empty;
                }

                // Decide se esse cliente quer suco
                if (spritePedidoSuco != null)
                {
                    querSuco = spritePedidoSuco.enabled;
                }
                else
                {
                    querSuco = false;
                }
            }

            // Registra a entrega da bandeja para este cliente e retorna a pontuação
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
                        sucoCorreto = false;
                    }
                }
                else
                {
                    // Se não quer suco:
                    // aqui considerei que entregar suco dá uma penalidade leve
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

                // "Pedido perfeito" = string igual + suco correto
                bool lanchePerfeito = (codigoPedidoLanche == codigoLancheEntregue);
                pedidoPerfeito = lanchePerfeito && sucoCorreto;

                Debug.Log($"Cliente {name} recebeu pedido. Lanche entregue = {codigoLancheEntregue}, pedido = {codigoPedidoLanche}, pontos = {pontosTotais}, perfeito = {pedidoPerfeito}");

                return pontosTotais;
            }
        }
    }

}
