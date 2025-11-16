using UnityEngine;

public class FecharPopUp : MonoBehaviour
{
    public GameObject popUp; // painel que será fechado

    public void Fechar()
    {
        popUp.SetActive(false);
    }
}
