using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{

    public class PratoController : MonoBehaviour
    {
        public bool estaNaBandeja = false;
        private Transform bandejaTransform;

        
        public void OnDragReleased()
        {
            if (estaNaBandeja)
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
                Debug.Log("esta na bandeja hein");
                estaNaBandeja = true;
                bandejaTransform = col.transform;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();

            if (area != null && area.areaName == "Bandeja")
            {
                estaNaBandeja = false;
                bandejaTransform = null;
            }
        }

        
        public void ColocarIngredienteNoPrato(GameObject ingrediente)
        {
            
            ingrediente.transform.SetParent(
                transform.Find("IngredientesEmpilhados")
            );

            
            ingrediente.transform.localPosition = new Vector3(0, 0, -1);
        }

    }    }
