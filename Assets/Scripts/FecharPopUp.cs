using UnityEngine;

public class FecharPopUp : MonoBehaviour
{
    public GameObject popUp; 

    public void Fechar()
    {
        popUp.SetActive(false);
    }

    public void fecharParaMenu()
    {
        Time.timeScale = 1f; 
         popUp.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("CenaMenu");
    }
}
