using UnityEngine;
using UnityEngine.SceneManagement;

public class Historia1 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("CenaHistoria2");
        }

        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("CenaHistoria2");
        }
    }
}