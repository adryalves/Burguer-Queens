using Assets.Scripts.CenaJogo;
using UnityEngine;
using TMPro;

public class ControleTempoFase : MonoBehaviour
{
    public float tempoTotal = 60f;
    private float tempoRestante;
    private bool faseEncerrada = false;

    public TextMeshProUGUI Relogio;

    [Header("PopUpGanharPerder de Resultado")]
  
    public GameObject PopUpGanharPerder;  

    void Start()
    {
        tempoRestante = tempoTotal;



        if (PopUpGanharPerder != null)
            PopUpGanharPerder.SetActive(false);
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
        Relogio.text = $"{minutos:00}:{segundos:00}";
    }

    void EncerrarFase()
    {
        faseEncerrada = true;

        Time.timeScale = 0f;

        ArrastarItensController[] itens = FindObjectsOfType<ArrastarItensController>();
        foreach (var item in itens)
        {
            item.AtivarInteracao(false);
        }


        if (PopUpGanharPerder != null)
            PopUpGanharPerder.SetActive(true);

    }
}
