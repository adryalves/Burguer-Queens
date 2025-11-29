using System.Collections;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class CarneController : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite spriteCrua;
        public Sprite spriteAssada;
        public Sprite spriteQueimada;   

        [Header("Tempo de cozimento (segundos)")]
        public float tempoCozimento = 3f;
        [Tooltip("Tempo depois de assada até queimar (segundos)")]
        public float tempoAteQueimar = 4f;  

        [Header("Offset na frigideira (ajuste no Inspector)")]
        public Vector3 offsetFrigideira = new Vector3(0f, 0.08f, 0f);

        private static bool frigideiraOcupada = false;

        private enum EstadoCarne
        {
            Crua,
            NaFrigideira,
            Assada,
            Queimada,   
            NoPrato,
            NoLixo
        }

        private EstadoCarne estadoAtual = EstadoCarne.Crua;

        private bool foiColocadaNaFrigideira = false;
        private bool foiColocadaNoPrato = false;
        private bool foiJogadaNoLixo = false;

        
        private bool estaSobreFrigideira = false;

        private ArrastarItensController drag;
        private Collider2D col2D;
        private SpriteRenderer sr;

        private Transform frigideiraTransform;

        void Awake()
        {
            drag = GetComponent<ArrastarItensController>();
            col2D = GetComponent<Collider2D>();
            sr = GetComponent<SpriteRenderer>();

            if (spriteCrua != null && sr != null)
                sr.sprite = spriteCrua;
        }

       
        public void OnDragReleased()
        {
            switch (estadoAtual)
            {
                case EstadoCarne.Crua:

                    if (estaSobreFrigideira && !frigideiraOcupada && frigideiraTransform != null)
                    {
                        frigideiraOcupada = true;
                        foiColocadaNaFrigideira = true;
                        estadoAtual = EstadoCarne.NaFrigideira;

                        transform.SetParent(frigideiraTransform);
                        transform.position = frigideiraTransform.position + offsetFrigideira;

                        if (drag != null)
                            drag.enabled = false;

                        StartCoroutine(CozinharCarne());
                    }
                    else
                    {
                        
                        Destroy(gameObject);
                    }
                    break;

                case EstadoCarne.Assada:
                case EstadoCarne.Queimada:
                   
                    if (foiColocadaNoPrato || foiJogadaNoLixo)
                        return;

                    
                    if (frigideiraTransform != null)
                    {
                        transform.SetParent(frigideiraTransform);
                        transform.position = frigideiraTransform.position + offsetFrigideira;
                    }
                    break;

                case EstadoCarne.NaFrigideira:
                case EstadoCarne.NoPrato:
                case EstadoCarne.NoLixo:
                   
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            
            if (area.areaName == "Frigideira")
            {
                if (estadoAtual == EstadoCarne.Crua)
                {
                    estaSobreFrigideira = true;
                    frigideiraTransform = col.transform;
                }
                return;
            }

           
            if (area.areaName == "Prato" && estadoAtual == EstadoCarne.Assada)
            {
                PratoController prato = col.GetComponentInParent<PratoController>();
                if (prato != null && prato.estaNaBandeja)
                {
                    frigideiraOcupada = false;

                    prato.ColocarIngredienteNoPrato(gameObject);
                    foiColocadaNoPrato = true;
                    estadoAtual = EstadoCarne.NoPrato;
                }
                return;
            }

            
            if (area.areaName == "Lixeira" &&
                (estadoAtual == EstadoCarne.Assada || estadoAtual == EstadoCarne.Queimada))
            {
                frigideiraOcupada = false;
                foiJogadaNoLixo = true;
                estadoAtual = EstadoCarne.NoLixo;
                Destroy(gameObject);
                return;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            AreaDetector area = col.GetComponent<AreaDetector>();
            if (area == null) return;

            if (area.areaName == "Frigideira" && estadoAtual == EstadoCarne.Crua)
            {
                estaSobreFrigideira = false;
            }
        }

        
        private IEnumerator CozinharCarne()
        {
            yield return new WaitForSeconds(tempoCozimento);

            estadoAtual = EstadoCarne.Assada;

            if (spriteAssada != null && sr != null)
                sr.sprite = spriteAssada;

            
            if (drag != null)
                drag.enabled = true;

            
            StartCoroutine(QueimarCarne());
        }

       
        private IEnumerator QueimarCarne()
        {
            yield return new WaitForSeconds(tempoAteQueimar);

           
            if (estadoAtual == EstadoCarne.Assada)
            {
                estadoAtual = EstadoCarne.Queimada;

                if (spriteQueimada != null && sr != null)
                    sr.sprite = spriteQueimada;

               
                if (drag != null)
                    drag.enabled = true;
            }
        }

        public bool PodeSerArrastada()
        {
            
            return estadoAtual == EstadoCarne.Crua ||
                   estadoAtual == EstadoCarne.Assada ||
                   estadoAtual == EstadoCarne.Queimada;
        }
    }
}
