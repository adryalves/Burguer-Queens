using UnityEngine;
using UnityEngine.SceneManagement;

public class botaoFechar : MonoBehaviour
{
    public string CenaMenu;

    public void Fechar()
    {
        SceneManager.LoadScene(CenaMenu);
    }
}
