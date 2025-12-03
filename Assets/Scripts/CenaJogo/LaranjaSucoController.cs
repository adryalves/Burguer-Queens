﻿using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class LaranjaSucoController : MonoBehaviour
    {
        private bool estaSobreMaquina = false;
        private MaquinaSucoController maquinaAlvo;

        public void OnDragReleased()
        {
            if (estaSobreMaquina && maquinaAlvo != null && maquinaAlvo.PodeReceberLaranja())
            {
                maquinaAlvo.ProcessarLaranja();
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area != null && area.areaName == "MaquinaSuco")
            {
                estaSobreMaquina = true;
                maquinaAlvo = col.GetComponentInParent<MaquinaSucoController>();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area != null && area.areaName == "MaquinaSuco")
            {
                estaSobreMaquina = false;
                maquinaAlvo = null;
            }
        }
    }
}
