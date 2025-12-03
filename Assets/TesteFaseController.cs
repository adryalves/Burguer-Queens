using UnityEngine;
using UnityEngine.UI; 

public class TesteFaseController : MonoBehaviour
{
    [Header("Regras da Fase")]
    public int moedasNecessarias = 10;
    public float tempoMaximo = 60f;

    [Header("Simulação (edite no Inspector)")]
    public int moedasSimuladas = 0;
    public float tempoSimulado = 0f;

    [Header("UI (opcional)")]
    public Text textoStatus;

    public void SimularResultado()
    {
        Debug.Log($"Simulação — moedas: {moedasSimuladas}, tempo: {tempoSimulado}s (necessário: {moedasNecessarias}, max: {tempoMaximo}s)");

        bool passou = moedasSimuladas >= moedasNecessarias && tempoSimulado <= tempoMaximo;

        if (passou)
        {
            Debug.Log("🟢 PASSOU DE FASE!");
            if (textoStatus != null) textoStatus.text = "PASSOU DE FASE! 🎉";
        }
        else
        {
            Debug.Log("🔴 NÃO PASSOU DE FASE.");
            if (textoStatus != null) textoStatus.text = "NÃO PASSOU DE FASE 😢";
        }
    }

    public void AdicionarMoeda(int qtd = 1)
    {
        moedasSimuladas += qtd;
        if (textoStatus != null) textoStatus.text = $"Moedas: {moedasSimuladas}";
    }
}
