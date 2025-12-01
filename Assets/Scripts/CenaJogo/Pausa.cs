using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject pausePopup;
    public Button BotaoPausa;
    public Button BotaoContinuar;
    public Button BotaoVoltar;
    public Button BotaoReiniciar;

    bool isPaused = false;

    void Start()
    {
        if (pausePopup != null) pausePopup.SetActive(false);

        if (BotaoPausa != null) BotaoPausa.onClick.AddListener(TogglePause);
        if (BotaoContinuar != null) BotaoContinuar.onClick.AddListener(ResumeGame);
        if (BotaoVoltar != null) BotaoVoltar.onClick.AddListener(GoToMenu);
        if (BotaoReiniciar != null) BotaoReiniciar.onClick.AddListener(RestartLevel);
    }

    void OnDestroy()
    {
        if (BotaoPausa != null) BotaoPausa.onClick.RemoveListener(TogglePause);
        if (BotaoContinuar != null) BotaoContinuar.onClick.RemoveListener(ResumeGame);
        if (BotaoVoltar != null) BotaoVoltar.onClick.RemoveListener(GoToMenu);
        if (BotaoReiniciar != null) BotaoReiniciar.onClick.RemoveListener(RestartLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;

        Time.timeScale = 0f;

        if (pausePopup != null) pausePopup.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;

        Time.timeScale = 1f;

        if (pausePopup != null) pausePopup.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        Input.ResetInputAxes();
        Assets.Scripts.CenaJogo.CarneController.ResetFrigideiraOcupada();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GoToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("CenaMenu");
    }
}
