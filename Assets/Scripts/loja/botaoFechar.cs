using UnityEngine;
using UnityEngine.SceneManagement;

public class botaoFechar : MonoBehaviour
{
    public string cenaMenu;

    public void Fechar()
    {
        SceneManager.LoadScene(cenaMenu);
    }
}
