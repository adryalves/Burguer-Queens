
using Assets.Scripts.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuFasesScript : MonoBehaviour, IJogadorPersistencia
{
    
    public Button[] faseButtons; 
    public Image[] faseImages;   


    public Sprite[] spritesLiberados; 
    public Sprite[] spritesBloqueados; 

    private int faseAtual;
    private int moedas;

    public TextMeshProUGUI txtMoedas;


    public void LoadData(DadosJogador data)
    {
        faseAtual = data.faseAtual;
        moedas = data.moedas;
    }

    void Start()
    {
        if (faseButtons.Length != 5 || faseImages.Length != 5 || spritesLiberados.Length != 5 || spritesBloqueados.Length != 4)
        {
            Debug.LogError("� necess�rio configurar 5 Bot�es, 5 Imagens, 5 Sprites Liberados e 4 Sprites Bloqueados no Inspector!");
            return;
        }

        AtualizarFases();
        AtualizarMoedas();
    
    }
    public void SaveData(DadosJogador data)
    {
        data.moedas = this.moedas;

    }

    private void AtualizarFases()
    {
        for (int i = 0; i < 5; i++)
        {
            int numeroFase = i + 1;
            
            bool liberada = faseAtual >= numeroFase;
           
            faseButtons[i].interactable = liberada;
        
            if (liberada)
            {               
                faseImages[i].sprite = spritesLiberados[i];
            }
            else
            {
                
                if (i > 0)
                {
                    faseImages[i].sprite = spritesBloqueados[i - 1];
                }
            }
        }
    }

    private void AtualizarMoedas()
    {
        if (txtMoedas != null)
             txtMoedas.text = moedas.ToString();
           
        else
            Debug.LogError("txtMoedas n�o foi conectado no Inspector!");
    }

    public void IrParaProximaFase()
    {
        SceneManager.LoadScene("CenaFase");
    }

  

}
