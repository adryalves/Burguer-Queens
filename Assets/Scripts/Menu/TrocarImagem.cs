using UnityEngine;
using UnityEngine.UI;

public class TrocarImagem : MonoBehaviour
{
    [Header("Referências")]
    public Image background;         // arrastar o objeto UI Image (Background) aqui
    public Sprite novaImagem;        // arrastar o sprite novo
    public Sprite imagemOriginal;    // arrastar o sprite original (opcional)
    public GameObject popup;         // arrastar o objeto do pop-up aqui

    // Abre o popup e troca a imagem (com checagens para evitar NullReference)
    public void AbrirPopUp()
    {
        if (background == null) Debug.LogWarning("TrocarImagem: background não está atribuído no Inspector.");
        else if (novaImagem == null) Debug.LogWarning("TrocarImagem: novaImagem não está atribuída no Inspector.");
        else background.sprite = novaImagem;

        if (popup != null) popup.SetActive(true);
        else Debug.LogWarning("TrocarImagem: popup não está atribuído no Inspector.");
    }

    // Fecha o popup e restaura a imagem original (se atribuída)
    public void FecharPopUp()
    {
        if (background == null) Debug.LogWarning("TrocarImagem: background não está atribuído no Inspector.");
        else if (imagemOriginal != null) background.sprite = imagemOriginal;

        if (popup != null) popup.SetActive(false);
        else Debug.LogWarning("TrocarImagem: popup não está atribuído no Inspector.");
    }
}
