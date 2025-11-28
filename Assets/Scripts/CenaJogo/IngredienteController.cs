using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{


    public class IngredienteController : MonoBehaviour
    {

        private PratoController pratoController;

        void Start()
        {
         
            pratoController = FindObjectOfType<PratoController>();
        }

        
        public void OnDragReleased()
        {
            
            if (!foiColocadoNoPrato)
            {
                Destroy(this.gameObject);
            }
        }

        private bool foiColocadoNoPrato = false;

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();

            if (area != null && area.areaName == "Prato")
            {
                
                if (pratoController != null && pratoController.estaNaBandeja)
                {
                    pratoController.ColocarIngredienteNoPrato(this.gameObject);
                    foiColocadoNoPrato = true;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
        
    
