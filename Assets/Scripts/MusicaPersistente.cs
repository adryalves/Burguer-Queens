using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instance;
    private AudioSource audioSource;

    [Header("Botão de Música (nome esperado: BotaoMusica)")]
    public Button botaoMusica;
    public Sprite spriteOn;
    public Sprite spriteOff;
    private Image imagemBotao;

    [Header("Controle de Volume (nome esperado: Slider)")]
    public GameObject sliderVolumeGO;
    public Slider sliderVolume;

    private bool musicaAtiva = true;
    private bool sliderVisivel = false;
    private float volumeAnterior = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) Debug.LogWarning("MusicaPersistente: não encontrou AudioSource no GameObject.");

            if (audioSource != null) audioSource.Play();
            volumeAnterior = audioSource != null ? audioSource.volume : 1f;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        RecarregarUI();
    }

    private void RecarregarUI()
    {
        GameObject botaoGO = GameObject.Find("BotaoMusica");
        if (botaoGO != null)
        {
            botaoMusica = botaoGO.GetComponent<Button>();
        }

        imagemBotao = null;
        if (botaoMusica != null)
        {
            imagemBotao = botaoMusica.GetComponent<Image>();
            if (imagemBotao == null)
                imagemBotao = botaoMusica.GetComponentInChildren<Image>();
        }

        sliderVolumeGO = GameObject.Find("Slider");
        sliderVolume = sliderVolumeGO != null ? sliderVolumeGO.GetComponent<Slider>() : null;

        if (sliderVolume == null)
        {
            Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>(true);
            foreach (var c in canvases)
            {
                Slider s = c.GetComponentInChildren<Slider>(true);
                if (s != null)
                {
                    sliderVolume = s;
                    sliderVolumeGO = s.gameObject;
                    break;
                }
            }
        }

        if (botaoMusica != null)
        {
            botaoMusica.onClick.RemoveAllListeners();
            botaoMusica.onClick.AddListener(ToggleMusica);
        }
        else
        {
            Debug.LogWarning("MusicaPersistente: não encontrou BotaoMusica nesta cena.");
        }

        if (sliderVolume != null)
        {
            sliderVolume.minValue = 0f;
            sliderVolume.maxValue = 1f;
            sliderVolume.value = audioSource != null ? audioSource.volume : 1f;

            sliderVolume.onValueChanged.RemoveAllListeners();
            sliderVolume.onValueChanged.AddListener(AtualizaVolume);

            sliderVolumeGO.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MusicaPersistente: não encontrou Slider nesta cena (ou está desativado).");
        }

        AtualizaImagemBotao();
    }

    void Update()
    {
        if (!sliderVisivel) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverAllowedUI())
            {
                EsconderSlider();
            }
        }
    }


    private void ToggleMusica()
    {
        if (audioSource == null) return;

        if (musicaAtiva)
        {
            volumeAnterior = audioSource.volume;
            audioSource.Pause();
            musicaAtiva = false;

            MostrarSlider();
            if (sliderVolume != null) sliderVolume.value = 0f;
        }
        else
        {
            audioSource.UnPause();
            musicaAtiva = true;

            MostrarSlider();
            if (sliderVolume != null) sliderVolume.value = Mathf.Max(0.0001f, volumeAnterior);
        }

        AtualizaVolume(sliderVolume != null ? sliderVolume.value : volumeAnterior);
        AtualizaImagemBotao();
    }

    private void MostrarSlider()
    {
        if (sliderVolumeGO != null)
        {
            sliderVisivel = true;
            sliderVolumeGO.SetActive(true);
        }
    }

    private void EsconderSlider()
    {
        if (sliderVolumeGO != null)
        {
            sliderVisivel = false;
            sliderVolumeGO.SetActive(false);
        }
    }

    private void AtualizaVolume(float valor)
    {
        audioSource.volume = valor;

        if (valor > 0f)
        {
            if (!musicaAtiva)
            {
                audioSource.UnPause();
                musicaAtiva = true;
            }

            volumeAnterior = valor;

            if (imagemBotao != null)
                imagemBotao.sprite = spriteOn;
        }
        else
        {
            if (imagemBotao != null)
                imagemBotao.sprite = spriteOff;

            audioSource.Pause();
            musicaAtiva = false;
        }
    }


    private void AtualizaImagemBotao()
    {
        if (imagemBotao != null && audioSource != null)
            imagemBotao.sprite = (musicaAtiva && audioSource.volume > 0f) ? spriteOn : spriteOff;
    }

    private bool IsPointerOverAllowedUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (botaoMusica != null &&
                (hit.gameObject == botaoMusica.gameObject ||
                 hit.gameObject.transform.IsChildOf(botaoMusica.transform)))
                return true;

            if (sliderVolumeGO != null &&
                (hit.gameObject == sliderVolumeGO ||
                 hit.gameObject.transform.IsChildOf(sliderVolumeGO.transform)))
                return true;
        }

        return false;
    }

}
