using UnityEngine;

public class botaoComprar : MonoBehaviour
{
    public string itemId;
    public lojaInterface lojaInterface;

    public void Comprar()
    {
        Debug.Log("Botão clicado: " + itemId);
        Debug.Log("Moedas antes: " + playerData.Instance.moedas);

        bool compra = playerData.Instance.Comprar(itemId);

        if (compra)
        {
            lojaInterface.AtualizarUI();
        }
        else
        {
            Debug.Log("Não foi possível comprar!");
        }
    }
}
