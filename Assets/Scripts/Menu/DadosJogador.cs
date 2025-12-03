using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosJogador
{
    public int faseAtual = 1;
    public int moedas = 0;

    public List<int> pontuacaoPorFase = new List<int>()
    {
        0, 0, 0, 0
    };


}
