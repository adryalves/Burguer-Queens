using UnityEngine;
using TMPro;
using System.Collections.Generic;

// Ajuste o using abaixo se a interface IJogadorPersistencia estiver em outro namespace.
// Ex: using Assets.Scripts.Menu;
using Assets.Scripts.Menu;

public class MenuFaseUI : MonoBehaviour, IJogadorPersistencia
{
    [Header("Textos que mostram o REQUISITO da fase (Fase 1 -> índice 0)")]
    public TextMeshProUGUI[] textosPontuacao;

    private int[] pontuacoes = new int[0];
    private int[] moedas = new int[0];
    private int[] requisitoPorFase = new int[4] { 150, 0, 0, 0 };

    public void LoadData(DadosJogador data)
    {
        if (data == null)
        {
            pontuacoes = new int[0];
            moedas = new int[0];
            AtualizarUI();
            return;
        }

        if (data.pontuacaoPorFase == null)
            pontuacoes = new int[0];
        else
            pontuacoes = data.pontuacaoPorFase.ToArray(); 

        if (data.moedaoPorFase == null)
            moedas = new int[0];
        else
            moedas = data.moedaoPorFase.ToArray(); 

        Debug.Log("✅ Moedas carregadas do JSON: " + string.Join(", ", moedas));

        AtualizarUI();
    }

    public void SaveData(DadosJogador data)
    {
        if (data != null && moedas != null)
        {
            data.moedaoPorFase = new List<int>(moedas);
            Debug.Log("💾 Moedas salvas no JSON: " + string.Join(", ", moedas));
        }
    }

    void Start()
    {
        AtualizarUI();
    }

    public void AdicionarMoedas(int faseIndex, int quantidade)
    {
        if (faseIndex >= 0 && faseIndex < moedas.Length)
        {
            moedas[faseIndex] += quantidade;
            Debug.Log($"🪙 Moeda adicionada na fase {faseIndex + 1}. Total: {moedas[faseIndex]}");
            AtualizarUI();
        }
    }

    public void AtualizarUI()
    {
        if (textosPontuacao == null || textosPontuacao.Length == 0)
            return;

        for (int i = 0; i < textosPontuacao.Length; i++)
        {
            string valor = "0";

            if (requisitoPorFase != null && i < requisitoPorFase.Length)
                valor = requisitoPorFase[i].ToString();

            if (textosPontuacao[i] != null)
                textosPontuacao[i].text = valor;
        }
    }
}
