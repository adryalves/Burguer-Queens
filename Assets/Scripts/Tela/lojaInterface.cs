using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lojaInterface : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI moedasText;

    public TextMeshProUGUI precoFrigideira;
    public TextMeshProUGUI quantidadeFrigideira;

    public TextMeshProUGUI precoLiquidificador;
    public TextMeshProUGUI quantidadeLiquidificador;

    public TextMeshProUGUI precoBandeja;
    public TextMeshProUGUI quantidadeBandeja;

    [Header("Bloqueios")]
    public Image bloqueioFrigideira;
    public Image bloqueioLiquidificador;
    public Image bloqueioBandeja;

    void Start()
    {
        AtualizarUI();
    }

    public void AtualizarUI()
    {
        if (playerData.Instance == null) return;

        moedasText.text = playerData.Instance.moedas.ToString();

        // FRIGIDEIRA
        var fr = lojaDataLoader.Instance.GetItem("frigideira");
        precoFrigideira.text = playerData.Instance.precoFrigideira.ToString();
        quantidadeFrigideira.text = $"{playerData.Instance.frigideiras} de {fr.quantidadeMax}";

        bloqueioFrigideira.enabled =
            playerData.Instance.moedas < playerData.Instance.precoFrigideira ||
            playerData.Instance.frigideiras >= fr.quantidadeMax;


        // LIQUIDIFICADOR
        var li = lojaDataLoader.Instance.GetItem("liquidificador");
        precoLiquidificador.text = playerData.Instance.precoLiquidificador.ToString();
        quantidadeLiquidificador.text = $"{playerData.Instance.liquidificadores} de {li.quantidadeMax}";

        bloqueioLiquidificador.enabled =
            playerData.Instance.moedas < playerData.Instance.precoLiquidificador ||
            playerData.Instance.liquidificadores >= li.quantidadeMax;


        // BANDEJA
        var ba = lojaDataLoader.Instance.GetItem("bandeja");
        precoBandeja.text = playerData.Instance.precoBandeja.ToString();
        quantidadeBandeja.text = $"{playerData.Instance.bandejas} de {ba.quantidadeMax}";

        bloqueioBandeja.enabled =
            playerData.Instance.moedas < playerData.Instance.precoBandeja ||
            playerData.Instance.bandejas >= ba.quantidadeMax;
    }
}
