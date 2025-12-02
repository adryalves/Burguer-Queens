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
        
        private ControleTempoFase controleTempoFase;


        public int pontuacaoFase = 0;

        public int moedas;

        void Awake()
        {
            posicaoInicial = transform.position;
            
            // Encontra o ControleTempoFase na cena
            controleTempoFase = FindObjectOfType<ControleTempoFase>();
            if (controleTempoFase == null)
                Debug.LogWarning("⚠️ ControleTempoFase não encontrado na cena!");
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

                if (clienteAtual == null)
                {
                    Debug.LogWarning("[BANDEJA] Tentou entregar para cliente, mas clienteAtual == null");
                    VoltarParaPosicaoInicial();
                    return;
                }

                string codigoEntregue = pratoAtual != null
                    ? pratoAtual.GerarCodigoPorNome()
                    : string.Empty;

                bool sucoEntregue = temCopo;

                Debug.Log("[BANDEJA] Entregando para " + clienteAtual.name);

                bool pedidoPerfeito;
                moedas = clienteAtual.RegistrarEntrega(sucoEntregue, codigoEntregue, out pedidoPerfeito);
                pontuacaoFase += moedas;


                if (JogadorPersistenciaManager.Instance != null)
                {
                    JogadorPersistenciaManager.Instance.SavePlayerData();
                }


                Destroy(clienteAtual.gameObject);
                clienteAtual = null;

                if (pratoAtual != null)
                {
                    // Adiciona 1 moeda por prato entregue
                    if (controleTempoFase != null)
                    {
                        controleTempoFase.moedasDoJogador += 1;
                        Debug.Log($" +1 moeda! Total: {controleTempoFase.moedasDoJogador}");
                    }
                    
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
            {
                sobreCliente = true;


                clienteAtual = col.GetComponentInParent<ClienteController>();

                Debug.Log("[BANDEJA] Entrei na área do cliente, clienteAtual = " + clienteAtual);
            }
            else if (area.areaName == "Lixeira")
            {
                sobreLixeira = true;
            }
        }


        void OnTriggerExit2D(Collider2D col)
        {
            var area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Cliente")
            {
                sobreCliente = false;


                var cli = col.GetComponentInParent<ClienteController>();
                if (cli != null && cli == clienteAtual)
                {
                    clienteAtual = null;
                }

                Debug.Log("[BANDEJA] Saí da área do cliente");
            }
            else if (area.areaName == "Lixeira")
            {
                sobreLixeira = false;
            }
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
            data.pontuacaoPorFase[0] += this.pontuacaoFase;
        }
    }
}