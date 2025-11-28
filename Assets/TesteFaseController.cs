using UnityEngine;
using UnityEngine.UI; // usar TMPro se preferir TextMeshPro

public class TesteFaseController : MonoBehaviour
{
    [Header("Regras da Fase")]
    public int moedasNecessarias = 10;
    public float tempoMaximo = 60f;

    [Header("Simulação (edite no Inspector)")]
    public int moedasSimuladas = 0;
    public float tempoSimulado = 0f;

    [Header("UI (opcional)")]
    public Text textoStatus; // arraste seu Text aqui ou use TMP_Text

    // Método público para ligar ao botão
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

    // Método para permitir simular incrementalmente no Play (opcional)
    public void AdicionarMoeda(int qtd = 1)
    {
        moedasSimuladas += qtd;
        if (textoStatus != null) textoStatus.text = $"Moedas: {moedasSimuladas}";
    }
}
