using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instance;
    private AudioSource audioSource;

    [Header("Botão de Música")]
    public Button botaoMusica;
    public Sprite spriteOn;   
    public Sprite spriteOff; 
    public Image imagemBotao;

    [Header("Controle de Volume")]
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
            audioSource.Play();
            volumeAnterior = audioSource.volume;

            if (botaoMusica != null)
                botaoMusica.onClick.AddListener(ToggleMusica);

            if (sliderVolumeGO != null)
                sliderVolumeGO.SetActive(false);

            if (sliderVolume != null)
            {
                sliderVolume.minValue = 0f;
                sliderVolume.maxValue = 1f;
                sliderVolume.value = audioSource.volume;
                sliderVolume.onValueChanged.AddListener(AtualizaVolume);
            }

            AtualizaImagemBotao();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (sliderVisivel && Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(botaoMusica.gameObject) &&
                !IsPointerOverUIElement(sliderVolumeGO))
            {
                EsconderSlider();
            }
        }
    }

    private void ToggleMusica()
    {
        if (musicaAtiva)
        {
            // Música ativa → pausa
            volumeAnterior = audioSource.volume; 
            audioSource.Pause();
            musicaAtiva = false;

            MostrarSlider();
            sliderVolume.value = 0f; 
        }
        else
        {
            // Música pausada → resume
            audioSource.UnPause();
            musicaAtiva = true;

            MostrarSlider();
            sliderVolume.value = volumeAnterior; 
        }

        AtualizaVolume(sliderVolume.value);
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
        if (!musicaAtiva && valor > 0f)
        {
            audioSource.UnPause();
            musicaAtiva = true;
        }

        audioSource.volume = valor;

        if (valor <= 0f)
        {
            if (imagemBotao != null)
                imagemBotao.sprite = spriteOff;

            if (musicaAtiva)
            {
                audioSource.Pause();
                musicaAtiva = false;
            }
        }
        else
        {
            if (imagemBotao != null)
                imagemBotao.sprite = spriteOn;

            if (musicaAtiva)
                volumeAnterior = valor;
        }
    }

    private void AtualizaImagemBotao()
    {
        if (imagemBotao != null)
            imagemBotao.sprite = (musicaAtiva && audioSource.volume > 0f) ? spriteOn : spriteOff;
    }

    private bool IsPointerOverUIElement(GameObject uiObject)
    {
        if (uiObject == null) return false;
        return EventSystem.current.IsPointerOverGameObject() &&
               EventSystem.current.currentSelectedGameObject == uiObject;
    }
}
