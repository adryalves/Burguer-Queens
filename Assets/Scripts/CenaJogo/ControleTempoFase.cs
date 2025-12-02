using UnityEngine;
using TMPro;
using Assets.Scripts.CenaJogo; // NECESSÁRIO para ArrastarItensController
using Assets.Scripts.Menu;

public class ControleTempoFase : MonoBehaviour
{
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool faseEncerrada = false;

    public TextMeshProUGUI textoRelogio;

    [Header("Pop-up Final")]
    public GameObject popUpFinal;         // Janela do pop-up final
    public GameObject ganhouObj;          // Imagem de check
    public GameObject perdeuObj;          // Imagem de X

    [Header("Requisitos da Fase")]
    public MostrarRequisitoDeFase popupRequisitos;
    public int numeroDaFase = 1;          // Fase atual (configure no Inspector)
    public int moedasDoJogador = 0;       // Número de moedas coletadas nesta fase

    void Start()
    {
        tempoRestante = tempoTotal;

        if (popUpFinal != null) popUpFinal.SetActive(false);
        if (ganhouObj != null) ganhouObj.SetActive(false);
        if (perdeuObj != null) perdeuObj.SetActive(false);
    }

    void Update()
    {
        if (faseEncerrada) return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            EncerrarFase();
        }

        AtualizarRelogio();
    }

    void AtualizarRelogio()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textoRelogio.text = $"{minutos:00}:{segundos:00}";
    }

   void EncerrarFase()
{
    faseEncerrada = true;

    Time.timeScale = 0f;

    var itens = FindObjectsByType<ArrastarItensController>(FindObjectsSortMode.None);
    foreach (var item in itens)
        item.AtivarInteracao(false);

    // Ativa o pop-up final primeiro
    popUpFinal.SetActive(true);

    // Garante que as imagens começam escondidas
    ganhouObj.SetActive(false);
    perdeuObj.SetActive(false);

    // Calcula requisito
    // Se não houver moedas coletadas em runtime, tenta ler do save
    if (moedasDoJogador == 0 && JogadorPersistenciaManager.Instance != null)
    {
        int salvas = JogadorPersistenciaManager.Instance.GetMoedasDaFase(numeroDaFase);
        if (salvas > 0)
        {
            moedasDoJogador = salvas;
            Debug.Log($"ℹ️ Usando moedas salvas da fase {numeroDaFase}: {moedasDoJogador}");
        }
    }

    int requisitoDaFase = popupRequisitos.requisitos.GetRequisitoDaFase(numeroDaFase);
    popupRequisitos.MostrarPopup(numeroDaFase);

    bool ganhou = moedasDoJogador >= requisitoDaFase;

    // Agora mostra o correto
    if (ganhou)
    {
        ganhouObj.SetActive(true);
    }
    else
    {
        perdeuObj.SetActive(true);
    }

    // Salva as moedas no MenuFaseUI
    MenuFaseUI menuUI = FindObjectOfType<MenuFaseUI>();
    if (menuUI != null)
    {
        // índice da fase (numeroDaFase - 1 porque começa de 0)
        menuUI.AdicionarMoedas(numeroDaFase - 1, moedasDoJogador);
        Debug.Log($"✅ Moedas adicionadas à fase {numeroDaFase}: {moedasDoJogador}");
    }
    else
    {
        Debug.LogWarning("⚠️ MenuFaseUI não encontrado na cena!");
    }

    // Salva os dados
    if (JogadorPersistenciaManager.Instance != null)
    {
        JogadorPersistenciaManager.Instance.SavePlayerData();
        Debug.Log("💾 Dados salvos!");
    }
    else
    {
        Debug.LogWarning("⚠️ JogadorPersistenciaManager não encontrado!");
    }

    Debug.Log(ganhou ? "GANHOU a fase!" : "PERDEU a fase!");
}
}
