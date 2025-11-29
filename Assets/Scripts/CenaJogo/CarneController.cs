using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{

        public class CarneController : MonoBehaviour
        {
            private bool foiColocadaNaFrigideira = false;
            private static bool frigideiraOcupada = false;

        public void OnDragReleased()
            {
                // Se a carne não foi aceita na frigideira, ela desaparece
                if (!foiColocadaNaFrigideira)
                {
                    Destroy(this.gameObject);
                }
            }
        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();

            if (area != null && area.areaName == "Frigideira")
            {
                if (!frigideiraOcupada)
                {
                    // Ocupa a frigideira
                    frigideiraOcupada = true;

                    // Fixa a carne na frigideira
                    transform.SetParent(col.transform);
                    transform.position = col.transform.position + new Vector3(0f, 0.08f, 0f);
                    foiColocadaNaFrigideira = true;

                    // Impede continuar arrastando
                    Destroy(GetComponent<ArrastarItensController>());
                }
                else
                {
                    // Já tem carne → destruir ou voltar pro lugar
                    Destroy(this.gameObject);
                }
            }
        }

    }
    }


