using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class CopoSucoController : MonoBehaviour
    {
        [Header("Sprites do copo")]
        public Sprite spriteCopoVazio;
        public Sprite spriteCopoCheio;

        [Header("Offsets")]
        public Vector3 offsetNaBandeja = new Vector3(0.8f, 0f, 0f);

        [Header("Referências")]
        public MaquinaSucoController maquinaSuco;

        private SpriteRenderer sr;
        private ArrastarItensController drag;

        private Vector3 posicaoInicial;
        private Transform bandejaTransform;

        private bool copoCheio = false;
        private bool sobreBandeja = false;
        private bool sobreLixeira = false;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            drag = GetComponent<ArrastarItensController>();

            posicaoInicial = transform.position;

            if (sr != null && spriteCopoVazio != null)
                sr.sprite = spriteCopoVazio;

           
            if (drag != null)
                drag.enabled = false;
        }

       
        public void EncherCopo()
        {
            copoCheio = true;

            if (sr != null && spriteCopoCheio != null)
                sr.sprite = spriteCopoCheio;

            if (drag != null)
                drag.enabled = true;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Bandeja")
            {
                sobreBandeja = true;
                bandejaTransform = col.transform;
            }
            else if (area.areaName == "Lixeira")
            {
                sobreLixeira = true;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Bandeja")
            {
                sobreBandeja = false;
                bandejaTransform = null;
            }
            else if (area.areaName == "Lixeira")
            {
                sobreLixeira = false;
            }
        }

        
        public void OnDragReleased()
        {
            if (!copoCheio)
            {
               
                VoltarParaOrigemVazio();
                return;
            }

            if (sobreBandeja && bandejaTransform != null)
            {
                transform.SetParent(bandejaTransform);
                transform.localPosition = offsetNaBandeja;

               
                if (drag != null)
                    drag.enabled = false;

               
                var col = GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = false;

                return;
            }


            if (sobreLixeira)
            {
              
                VoltarParaOrigemVazio();

                if (maquinaSuco != null)
                    maquinaSuco.NotificarCopoLiberado();

                return;
            }

           
            transform.SetParent(null);
            transform.position = posicaoInicial;
        }

        private void VoltarParaOrigemVazio()
        {
            copoCheio = false;

            transform.SetParent(null);
            transform.position = posicaoInicial;

            if (sr != null && spriteCopoVazio != null)
                sr.sprite = spriteCopoVazio;

            if (drag != null)
                drag.enabled = false;
        }
    }
}
