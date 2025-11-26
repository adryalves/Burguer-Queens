using UnityEngine;
using TMPro;

public class MostrarRequisitoDeFase : MonoBehaviour
{
    public RequisitosDeFase requisitos;
    public TMP_Text textoNivel;
    public TMP_Text textoMoedas;

    public void MostrarPopup(int fase)
    {
        textoNivel.text = fase.ToString();
        textoMoedas.text = requisitos.GetRequisitoDaFase(fase).ToString();

        gameObject.SetActive(true);
    }
}
