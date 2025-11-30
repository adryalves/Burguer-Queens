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
        private Collider2D col2D;

        private Vector3 posicaoInicial;
        private Transform bandejaTransform;
        private BandejaController bandejaController;

        private bool copoCheio = false;
        private bool sobreBandeja = false;
        private bool sobreLixeira = false;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            drag = GetComponent<ArrastarItensController>();
            col2D = GetComponent<Collider2D>();

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

            if (col2D != null)
                col2D.enabled = true;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Bandeja")
            {
                sobreBandeja = true;
                bandejaTransform = col.transform;
                bandejaController = col.GetComponentInParent<BandejaController>();
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

                if (bandejaTransform == col.transform)
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

                if (col2D != null)
                    col2D.enabled = false;

               
                if (bandejaController != null)
                    bandejaController.RegistrarCopoNaBandeja(this);

                return;
            }

           
            if (sobreLixeira)
            {
                VoltarParaOrigemVazio();
                return;
            }

           
            transform.SetParent(null);
            transform.position = posicaoInicial;

           
            if (bandejaController != null && bandejaController.copoNaBandeja == this)
                bandejaController.copoNaBandeja = null;

            bandejaController = null;
            bandejaTransform = null;

            if (drag != null)
                drag.enabled = true;

            if (col2D != null)
                col2D.enabled = true;
        }

        
        public void VoltarParaOrigemVazio()
        {
            copoCheio = false;

            transform.SetParent(null);
            transform.position = posicaoInicial;

            if (sr != null && spriteCopoVazio != null)
                sr.sprite = spriteCopoVazio;

            if (drag != null)
                drag.enabled = false;

            if (col2D != null)
                col2D.enabled = true;
        }

       
        public void ResetarCopoParaOrigem()
        {
            VoltarParaOrigemVazio();

            if (maquinaSuco != null)
                maquinaSuco.NotificarCopoLiberado();

           
            bandejaTransform = null;
            bandejaController = null;
        }
    }
}
