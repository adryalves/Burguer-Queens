using UnityEngine;

public class FecharJogoEsc : MonoBehaviour
{
    private static FecharJogoEsc instance;

    public void FecharJogo()
    {
        Application.Quit();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
