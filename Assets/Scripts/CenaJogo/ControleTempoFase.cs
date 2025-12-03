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
    }
    else
    {
        perdeuObj.SetActive(true);
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
