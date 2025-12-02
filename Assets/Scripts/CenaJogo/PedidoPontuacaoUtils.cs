using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public static class PedidoPontuacaoUtils
    {

        public static int CalcularPontuacao(
            string pedido,
            string entregue,
            out int acertosExatos,
            out int acertosForaDePosicao)
        {
            acertosExatos = 0;
            acertosForaDePosicao = 0;
            int pontuacao = 0;

            if (string.IsNullOrEmpty(pedido) || string.IsNullOrEmpty(entregue))
            {
                return 0;
            }

            int nPedido = pedido.Length;
            int nEntregue = entregue.Length;

            bool[] usadoPedido = new bool[nPedido];
            bool[] usadoEntregue = new bool[nEntregue];

            int nMin = Mathf.Min(nPedido, nEntregue);


            for (int i = 0; i < nMin; i++)
            {
                if (pedido[i] == entregue[i])
                {
                    usadoPedido[i] = true;
                    usadoEntregue[i] = true;
                    acertosExatos++;
                    pontuacao += 20;
                }
            }

            // 2ª passada: ingredientes corretos mas em posição diferente
            for (int i = 0; i < nEntregue; i++)
            {
                if (usadoEntregue[i])
                    continue;

                char c = entregue[i];

                for (int j = 0; j < nPedido; j++)
                {
                    if (usadoPedido[j])
                        continue;

                    if (pedido[j] == c)
                    {
                        usadoPedido[j] = true;
                        usadoEntregue[i] = true;
                        acertosForaDePosicao++;
                        pontuacao += 10;
                        break;
                    }
                }
            }

            return pontuacao;
        }
    }
}
