using UnityEngine;
using UnityEngine.UI;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instance;
    private AudioSource audioSource;

    [Header("Botão de Música")]
    public Button botaoMusica;          
    public Sprite spriteOn;         
    public Sprite spriteOff;           
    public Image imagemBotao;           

    private bool musicaAtiva = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            if(botaoMusica != null)
                botaoMusica.onClick.AddListener(ToggleMusica);

            AtualizaImagemBotao();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusica()
    {
        if (musicaAtiva)
        {
            audioSource.Pause();
            musicaAtiva = false;
        }
        else
        {
            audioSource.UnPause();
            musicaAtiva = true;
        }

        AtualizaImagemBotao();
    }

    private void AtualizaImagemBotao()
    {
        if(imagemBotao != null)
            imagemBotao.sprite = musicaAtiva ? spriteOn : spriteOff;
    }
}
