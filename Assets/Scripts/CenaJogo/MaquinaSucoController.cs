using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CenaJogo
{
    public class MaquinaSucoController : MonoBehaviour
    {
        [Header("Sprites da máquina")]
        public Sprite spriteMaquinaVazia;
        public Sprite spriteMaquinaProcessando;

        [Header("Referências")]
        public SpriteRenderer maquinaRenderer;
        public CopoSucoController copoDestino;
        public Image imagemTimer;

        [Header("Tempo de preparo (segundos)")]
        public float tempoProcessar = 2f;

        private bool estaProcessando = false;
        private bool copoPronto = false;
        private Coroutine rotinaProcessar;

        void Awake()
        {
            if (maquinaRenderer == null)
                maquinaRenderer = GetComponent<SpriteRenderer>();

            if (imagemTimer != null)
            {
                imagemTimer.fillAmount = 0f;
                imagemTimer.enabled = false;
                imagemTimer.color = Color.green;
            }
        }

        public bool PodeReceberLaranja()
        {
            return !estaProcessando && !copoPronto;
        }

        public void ProcessarLaranja()
        {
            if (!PodeReceberLaranja()) return;
            rotinaProcessar = StartCoroutine(ProcessarSucoCoroutine());
        }

        private IEnumerator ProcessarSucoCoroutine()
        {
            estaProcessando = true;

            if (maquinaRenderer != null && spriteMaquinaProcessando != null)
                maquinaRenderer.sprite = spriteMaquinaProcessando;

            if (imagemTimer != null)
            {
                imagemTimer.enabled = true;
                imagemTimer.color = Color.green;
                imagemTimer.fillAmount = 0f;
            }

            float t = 0f;

            while (t < tempoProcessar)
            {
                t += Time.deltaTime;
                if (imagemTimer != null)
                    imagemTimer.fillAmount = Mathf.Clamp01(t / tempoProcessar);
                yield return null;
            }

            if (maquinaRenderer != null && spriteMaquinaVazia != null)
                maquinaRenderer.sprite = spriteMaquinaVazia;

            if (imagemTimer != null)
            {
                imagemTimer.fillAmount = 1f;
                imagemTimer.enabled = false;
            }

            if (copoDestino != null)
                copoDestino.EncherCopo();

            estaProcessando = false;
            copoPronto = true;
            rotinaProcessar = null;
        }

        public void NotificarCopoLiberado()
        {
            copoPronto = false;

            if (rotinaProcessar != null)
            {
                StopCoroutine(rotinaProcessar);
                rotinaProcessar = null;
            }

            estaProcessando = false;

            if (imagemTimer != null)
            {
                imagemTimer.enabled = false;
                imagemTimer.fillAmount = 0f;
            }

            if (maquinaRenderer != null && spriteMaquinaVazia != null)
                maquinaRenderer.sprite = spriteMaquinaVazia;
        }
    }
}
