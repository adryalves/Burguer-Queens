using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class BandejaController : MonoBehaviour
    {
        
        public PratoController pratoAtual;

       
        [HideInInspector] public CopoSucoController copoNaBandeja;

        private Vector3 posicaoInicial;
        private bool sobreCliente = false;
        private bool sobreLixeira = false;

        void Awake()
        {
            posicaoInicial = transform.position;
        }

        void Update()
        {
         
            if (pratoAtual != null)
            {
                pratoAtual.transform.position =
                    transform.position + new Vector3(0f, 0.1f, 0f);
            }
        }

        
        public void OnDragReleased()
        {
            bool temPratoComLanche =
                pratoAtual != null && pratoAtual.TemAlgumIngredienteNoPrato();

            bool temCopo = (copoNaBandeja != null);

            
            if (!temPratoComLanche && !temCopo)
            {
                VoltarParaPosicaoInicial();
                return;
            }

            
            if ((sobreCliente || sobreLixeira) && temPratoComLanche)
            {
                
                if (pratoAtual != null)
                {
                    Object.Destroy(pratoAtual.gameObject);
                    pratoAtual = null;
                }

               
                if (copoNaBandeja != null)
                {
                    copoNaBandeja.ResetarCopoParaOrigem();
                    copoNaBandeja = null;
                }

               
                VoltarParaPosicaoInicial();
                return;
            }

           
            VoltarParaPosicaoInicial();
        }

        private void VoltarParaPosicaoInicial()
        {
            var drag = GetComponent<ArrastarItensController>();
            if (drag != null)
                drag.ResetToStart();
            else
                transform.position = posicaoInicial;
        }

       
        void OnTriggerEnter2D(Collider2D col)
        {
            var area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Cliente")
                sobreCliente = true;
            else if (area.areaName == "Lixeira")
                sobreLixeira = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            var area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Cliente")
                sobreCliente = false;
            else if (area.areaName == "Lixeira")
                sobreLixeira = false;
        }

       
        public void RegistrarCopoNaBandeja(CopoSucoController copo)
        {
            copoNaBandeja = copo;
        }
    }
}
