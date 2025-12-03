using UnityEngine;
using UnityEngine.SceneManagement;

public class BotoesPopupFinal : MonoBehaviour
{
    
    public void TentarNovamente()
    {
        Time.timeScale = 1f; 
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

   
   
    public void ProximaFase()
    {
        Time.timeScale = 1f;

        
        
        int proxima = SceneManager.GetActiveScene().buildIndex + 1;

        
        
        if (proxima < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene("CenaMenu");
        else
            Debug.Log("Não existe próxima fase configurada!");
    }


    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CenaMenu"); 
    }
}
