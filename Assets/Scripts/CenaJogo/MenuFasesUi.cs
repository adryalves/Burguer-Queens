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

    
    private int moedasDasFases;
    private int[] requisitoPorFase = new int[4] { 150, 0, 0, 0 };

    public void LoadData(DadosJogador data)
    {
        if (data == null)
        {
            moedasDasFases = 0;
            AtualizarUI();
            return;
        }
        moedasDasFases = data.pontuacaoPorFase[0];


        Debug.Log("✅ Moedas carregadas do JSON: " + string.Join(", ", moedasDasFases));

        AtualizarUI();
    }

    public void SaveData(DadosJogador data)
    {
        
    }

    void Start()
    {
        AtualizarUI();
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
