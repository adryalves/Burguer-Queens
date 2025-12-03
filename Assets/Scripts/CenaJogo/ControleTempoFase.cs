using UnityEngine;
using TMPro;
using Assets.Scripts.CenaJogo; // NECESSÁRIO para ArrastarItensController
using Assets.Scripts.Menu;

public class ControleTempoFase : MonoBehaviour, IJogadorPersistencia
{
    public float tempoTotal = 60f;
    
    private float tempoRestante;
    private bool faseEncerrada = false;

    public TextMeshProUGUI textoRelogio;

    [Header("Pop-up Final")]
    public GameObject popUpFinal;         // Janela do pop-up final
    public GameObject ganhouObj;          // Imagem de check
    public GameObject perdeuObj;          // Imagem de X

    [Header("Pop-up Buttons")]
    public GameObject botaoStart;                // Botão para iniciar (aparece ao ganhar)
    public GameObject botaoProximo;              // Botão para próxima fase (aparece ao ganhar)
    public GameObject botaoTentarNovamente;      // Botão para tentar novamente (aparece ao perder)

    [Header("Requisitos da Fase")]
    public MostrarRequisitoDeFase popupRequisitos;
    public int numeroDaFase = 1;          // Fase atual (configure no Inspector)
    int moedasDoJogador;
    public BandejaController controller;

    void Start()
    {
        tempoRestante = tempoTotal;

        if (popUpFinal != null) popUpFinal.SetActive(false);
        if (ganhouObj != null) ganhouObj.SetActive(false);
        if (perdeuObj != null) perdeuObj.SetActive(false);
        // Inicializa botões do pop-up como invisíveis
        if (botaoStart != null) botaoStart.SetActive(false);
        if (botaoProximo != null) botaoProximo.SetActive(false);
        if (botaoTentarNovamente != null) botaoTentarNovamente.SetActive(false);
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

    popUpFinal.SetActive(true);

    ganhouObj.SetActive(false);
    perdeuObj.SetActive(false);

    botaoStart.SetActive(false); // Sempre mostrar o botão Start         
    botaoProximo.SetActive(false); // Sempre mostrar o botão Próximo
    botaoTentarNovamente.SetActive(false); // Sempre mostrar o botão Tentar Nov

    if (controller != null){
        controller.SalvarDados();

    }

    int requisitoDaFase = popupRequisitos.requisitos.GetRequisitoDaFase(numeroDaFase);
    popupRequisitos.MostrarPopup(numeroDaFase);

    bool ganhou = controller.pontuacaoFase >= requisitoDaFase;

    // Agora mostra o correto
    if (ganhou)
    {
        ganhouObj.SetActive(true);
        botaoStart.SetActive(true); // Mostrar o botão Start ao ganhar
        botaoProximo.SetActive(true); // Mostrar o botão Próximo ao ganhar
    }
    else
    {
        perdeuObj.SetActive(true);
        botaoTentarNovamente.SetActive(true); // Mostrar o botão Tentar Novamente ao perder
    }


    // Debug.Log(ganhou ? "🎉 GANHOU a fase!" : "😢 PERDEU a fase!");
}

    public void LoadData(DadosJogador data)
    {
        moedasDoJogador = data.pontuacaoPorFase[0];  
    }

    public void SaveData(DadosJogador data)
    {
     
    }
}
