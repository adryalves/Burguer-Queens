using UnityEngine;
using UnityEngine.UI;

public class TrocarImagem : MonoBehaviour
{
    [Header("Referências")]
    public Image background;        
    public Sprite novaImagem;       
    public Sprite imagemOriginal;    
    public GameObject popup;        

    public void AbrirPopUp()
    {
        if (background == null) Debug.LogWarning("TrocarImagem: background não está atribuído no Inspector.");
        else if (novaImagem == null) Debug.LogWarning("TrocarImagem: novaImagem não está atribuída no Inspector.");
        else background.sprite = novaImagem;

        if (popup != null) popup.SetActive(true);
        else Debug.LogWarning("TrocarImagem: popup não está atribuído no Inspector.");
    }

    public void FecharPopUp()
    {
        if (background == null) Debug.LogWarning("TrocarImagem: background não está atribuído no Inspector.");
        else if (imagemOriginal != null) background.sprite = imagemOriginal;

        if (popup != null) popup.SetActive(false);
        else Debug.LogWarning("TrocarImagem: popup não está atribuído no Inspector.");
    }
}
