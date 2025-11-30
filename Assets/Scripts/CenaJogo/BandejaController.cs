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
            // Tem lanche? (prato na bandeja + pelo menos 1 ingrediente)
            bool temPratoComLanche =
                pratoAtual != null && pratoAtual.TemAlgumIngredienteNoPrato();

            // Tem suco na bandeja?
            bool temCopo = (copoNaBandeja != null);

            // Bandeja completamente vazia → sempre só volta pro lugar
            if (!temPratoComLanche && !temCopo)
            {
                VoltarParaPosicaoInicial();
                return;
            }

            // ============================
            // 1) ENTREGA PARA O CLIENTE
            // ============================
            // Só pode entregar se tiver lanche (prato com ingrediente).
            if (sobreCliente && temPratoComLanche)
            {
                // some com o prato
                if (pratoAtual != null)
                {
                    Destroy(pratoAtual.gameObject);
                    pratoAtual = null;
                }

                // se tiver suco junto, também reseta
                if (copoNaBandeja != null)
                {
                    copoNaBandeja.ResetarCopoParaOrigem();
                    copoNaBandeja = null;
                }

                VoltarParaPosicaoInicial();
                return;
            }

            // ============================
            // 2) JOGAR FORA NA LIXEIRA
            // ============================
            // Aqui pode descartar se tiver lanche OU só suco
            if (sobreLixeira && (temPratoComLanche || temCopo))
            {
                if (pratoAtual != null)
                {
                    Destroy(pratoAtual.gameObject);
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

            // ============================
            // 3) Qualquer outro lugar
            // ============================
            // Não entregou nem jogou fora → só volta pro lugar com o que tiver em cima
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
