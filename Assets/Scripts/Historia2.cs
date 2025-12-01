using UnityEngine;
using UnityEngine.SceneManagement;

public class Historia2 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("CenaMenu");
        }

        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("CenaMenu");
        }
    }
}