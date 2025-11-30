using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class IngredienteController : MonoBehaviour
    {
        private bool foiColocadoNoPrato = false;

       
        public void OnDragReleased()
        {
           
            if (!foiColocadoNoPrato)
            {
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();

           
            if (area != null && area.areaName == "Prato")
            {
                
                PratoController pratoController = col.GetComponentInParent<PratoController>();

                if (pratoController == null)
                {
                    Debug.LogWarning("[INGREDIENTE] Area 'Prato' sem PratoController no parent: " + col.name);
                    return;
                }

                Debug.Log("[INGREDIENTE] Entrei na área do prato: " + pratoController.name +
                          " | estaNaBandeja = " + pratoController.estaNaBandeja);

                
                if (pratoController.estaNaBandeja)
                {
                    pratoController.ColocarIngredienteNoPrato(this.gameObject);
                    foiColocadoNoPrato = true;
                }

               
            }
        }
    }
}
