using System.Collections;
using UnityEngine;
using Assets.Scripts.CenaJogo;

public class PratoController : MonoBehaviour
{
    public bool estaNaBandeja = false;

    private Transform bandejaTransform;
    private BandejaController bandejaController;

    
    public void OnDragReleased()
    {
        if (estaNaBandeja && bandejaTransform != null)
        {
            
            transform.position = bandejaTransform.position;
        }
        else
        {
            
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        AreaDetector area = col.GetComponent<AreaDetector>();

        if (area != null && area.areaName == "Bandeja")
        {
            
            BandejaController bc = col.GetComponentInParent<BandejaController>();

            if (bc != null)
            {
                
                if (bc.pratoAtual != null && bc.pratoAtual != this)
                {
                    Destroy(this.gameObject);
                    return;
                }

                bandejaController = bc;
                bandejaController.pratoAtual = this;
                bandejaTransform = bc.transform;
            }
            else
            {
                
                bandejaTransform = col.transform;
            }

            estaNaBandeja = true;

            
            transform.position = bandejaTransform.position;

            
            var drag = GetComponent<Assets.Scripts.CenaJogo.ArrastarItensController>();
            if (drag != null) Destroy(drag);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        AreaDetector area = col.GetComponent<AreaDetector>();

        if (area != null && area.areaName == "Bandeja")
        {
            estaNaBandeja = false;

            if (bandejaController != null && bandejaController.pratoAtual == this)
            {
                bandejaController.pratoAtual = null;
            }

            bandejaTransform = null;
        }
    }

    
    public void ColocarIngredienteNoPrato(GameObject ingrediente)
    {
        
        Transform alvo = transform.Find("IngredientesEmpilhados");
        if (alvo == null)
        {
            Debug.LogWarning("Prato: não encontrou o filho 'IngredientesEmpilhados'. Usando o próprio prato.");
            alvo = transform;
        }

        
        ingrediente.transform.SetParent(alvo, worldPositionStays: false);

        
        int index = alvo.childCount - 1; 

        
        float offsetY = 2.0f; 

       
        ingrediente.transform.localPosition = new Vector3(0f, index * offsetY, -1f);

        
        var drag = ingrediente.GetComponent<Assets.Scripts.CenaJogo.ArrastarItensController>();
        if (drag != null) Destroy(drag);

       
        var col2D = ingrediente.GetComponent<Collider2D>();
        if (col2D != null) col2D.enabled = false;
    }
}
