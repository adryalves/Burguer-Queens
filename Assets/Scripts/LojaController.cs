using Assets.Scripts.Menu;
using TMPro;
using UnityEngine;

public class LojaController : MonoBehaviour, IJogadorPersistencia
{
    public TextMeshProUGUI txtMoedasLoja;

    private int moedas;

    public void LoadData(DadosJogador data)
    {
        moedas = data.moedas;
    }

    public void SaveData(DadosJogador data)
    {
        data.moedas = moedas;
    }

    private void Start()
    {
        AtualizarMoedasUI();
    }

    private void AtualizarMoedasUI()
    {
        if (txtMoedasLoja != null)
            txtMoedasLoja.text = moedas.ToString();
        else
            Debug.LogError("txtMoedasLoja não foi atribuído no Inspector!");
    }
}
