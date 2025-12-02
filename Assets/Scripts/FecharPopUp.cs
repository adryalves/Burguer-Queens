using UnityEngine;

public class FecharPopUp : MonoBehaviour
{
    public GameObject popUp; 

    public void Fechar()
    {
        popUp.SetActive(false);
    }
}
