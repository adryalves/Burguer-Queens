using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        public float tempoAteQueimar = 4f;

        [Header("Timer Visual")]
        public Image imagemTimer;
        public Color corCozinhando = Color.green;
        public Color corMeio = Color.yellow;
        public Color corQueimando = Color.red;

        [Header("Offset na frigideira")]
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

        private bool foiColocadaNoPrato = false;
        private bool foiJogadaNoLixo = false;
        private bool estaSobreFrigideira = false;

        private ArrastarItensController drag;
        private SpriteRenderer sr;
        private Transform frigideiraTransform;

        private Coroutine rotinaCozinhar;
        private Coroutine rotinaQueimar;

        void Awake()
        {
            drag = GetComponent<ArrastarItensController>();
            sr = GetComponent<SpriteRenderer>();

            sr.sprite = spriteCrua;

            if (imagemTimer != null)
            {
                imagemTimer.fillAmount = 0f;
                imagemTimer.enabled = false;
            }
        }

        public void OnDragReleased()
        {
            switch (estadoAtual)
            {
                case EstadoCarne.Crua:
                    if (estaSobreFrigideira && !frigideiraOcupada && frigideiraTransform != null)
                    {
                        frigideiraOcupada = true;
                        estadoAtual = EstadoCarne.NaFrigideira;

                        transform.SetParent(frigideiraTransform);
                        transform.position = frigideiraTransform.position + offsetFrigideira;

                        drag.enabled = false;

                        ReiniciarTimer();

                        if (rotinaCozinhar != null) StopCoroutine(rotinaCozinhar);
                        if (rotinaQueimar != null) StopCoroutine(rotinaQueimar);

                        rotinaCozinhar = StartCoroutine(Cozinhar());
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

                    if (rotinaQueimar != null) StopCoroutine(rotinaQueimar);

                    DesativarTimer();
                }
                return;
            }

            if (area.areaName == "Lixeira" &&
                (estadoAtual == EstadoCarne.Assada || estadoAtual == EstadoCarne.Queimada))
            {
                frigideiraOcupada = false;
                foiJogadaNoLixo = true;
                estadoAtual = EstadoCarne.NoLixo;

                if (rotinaQueimar != null) StopCoroutine(rotinaQueimar);

                DesativarTimer();
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

        private void ReiniciarTimer()
        {
            if (imagemTimer == null) return;

            imagemTimer.enabled = true;
            imagemTimer.fillAmount = 0f;
            imagemTimer.color = corCozinhando;
        }

        private void DesativarTimer()
        {
            if (imagemTimer != null)
            {
                imagemTimer.enabled = false;
                imagemTimer.fillAmount = 0f;
            }
        }

        private IEnumerator Cozinhar()
        {
            float t = 0f;
            imagemTimer.enabled = true;
            imagemTimer.color = corCozinhando;

            while (t < tempoCozimento)
            {
                t += Time.deltaTime;
                float progresso = t / tempoCozimento;

                imagemTimer.fillAmount = progresso;

                yield return null;
            }

            estadoAtual = EstadoCarne.Assada;
            sr.sprite = spriteAssada;
            drag.enabled = true;

            rotinaQueimar = StartCoroutine(Queimar());
        }

        private IEnumerator Queimar()
        {
            float t = 0f;

            imagemTimer.fillAmount = 0f;

            while (t < tempoAteQueimar)
            {
                if (estadoAtual != EstadoCarne.Assada)
                    yield break;

                t += Time.deltaTime;
                float progresso = t / tempoAteQueimar;
                imagemTimer.fillAmount = progresso;

                float metade = tempoAteQueimar * 0.5f;

                if (t <= metade)
                {
                    float p = t / metade;
                    imagemTimer.color = Color.Lerp(corMeio, corQueimando, p * 0.3f);
                }
                else
                {
                    float p = (t - metade) / metade;
                    imagemTimer.color = Color.Lerp(corMeio, corQueimando, 0.3f + p * 0.7f);
                }

                yield return null;
            }

            if (estadoAtual == EstadoCarne.Assada)
            {
                estadoAtual = EstadoCarne.Queimada;
                sr.sprite = spriteQueimada;
                drag.enabled = true;

                DesativarTimer();
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
