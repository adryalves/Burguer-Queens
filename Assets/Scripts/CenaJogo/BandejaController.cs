using Assets.Scripts.CenaJogo.Assets.Scripts.CenaJogo;
using Assets.Scripts.Menu;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class BandejaController : MonoBehaviour, IJogadorPersistencia
    {
        
        public PratoController pratoAtual;

       
        [HideInInspector] public CopoSucoController copoNaBandeja;

        [HideInInspector] public ClienteController clienteAtual;

        private Vector3 posicaoInicial;
        private bool sobreCliente = false;
        private bool sobreLixeira = false;

        public int moedas;

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


            if (sobreCliente && temPratoComLanche)
            {
                
                string codigoEntregue = pratoAtual != null
                    ? pratoAtual.GerarCodigoPorNome()
                    : string.Empty;

                bool sucoEntregue = temCopo;

               
                if (clienteAtual != null)
                {
                    bool pedidoPerfeito;
                    moedas = clienteAtual.RegistrarEntrega(sucoEntregue, codigoEntregue, out pedidoPerfeito);

                    JogadorPersistenciaManager.Instance.SavePlayerData();
                    
                }

                // Some com o prato e o suco (como já fazia)
                if (pratoAtual != null)
                {
                    Destroy(pratoAtual.gameObject);
                    pratoAtual = null;
                }

                if (copoNaBandeja != null)
                {
                    copoNaBandeja.VoltarParaOrigemVazio();
                    copoNaBandeja = null;
                }

                VoltarParaPosicaoInicial();
                return;
            }


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

        public void LoadData(DadosJogador data)
        {
            
        }

        public void SaveData(DadosJogador data)
        {
            data.moedas += this.moedas;
        }
    }
}
