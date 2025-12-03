﻿using System.Collections;
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

        [Header("Timer Visual (Knob)")]
        public Image imagemTimer;       
        public Color corTimer = Color.green;

        [Header("Tempo de preparo (segundos)")]
        public float tempoProcessar = 2f;

        private bool estaProcessando = false;
        private bool copoPronto = false;

        void Awake()
        {
            if (maquinaRenderer == null)
                maquinaRenderer = GetComponent<SpriteRenderer>();

            if (imagemTimer != null)
            {
                imagemTimer.fillAmount = 0f;
                imagemTimer.enabled = false;
                imagemTimer.color = corTimer;
            }
        }

        public bool PodeReceberLaranja()
        {
            return !estaProcessando && !copoPronto;
        }

        public void ProcessarLaranja()
        {
            if (!PodeReceberLaranja()) return;
            StartCoroutine(ProcessarSucoCoroutine());
        }

        private IEnumerator ProcessarSucoCoroutine()
        {
            estaProcessando = true;

            if (maquinaRenderer != null && spriteMaquinaProcessando != null)
                maquinaRenderer.sprite = spriteMaquinaProcessando;

            ReiniciarTimer();
            yield return StartCoroutine(RodarTimer());

            if (maquinaRenderer != null && spriteMaquinaVazia != null)
                maquinaRenderer.sprite = spriteMaquinaVazia;

            if (copoDestino != null)
                copoDestino.EncherCopo(this);

            estaProcessando = false;
            copoPronto = true;

            DesativarTimer();
        }

        private IEnumerator RodarTimer()
        {
            float t = 0f;
            imagemTimer.enabled = true;
            imagemTimer.color = corTimer;

            while (t < tempoProcessar)
            {
                t += Time.deltaTime;
                imagemTimer.fillAmount = t / tempoProcessar;
                yield return null;
            }

            imagemTimer.fillAmount = 1f;
        }

        private void ReiniciarTimer()
        {
            if (imagemTimer == null) return;

            imagemTimer.enabled = true;
            imagemTimer.fillAmount = 0f;
            imagemTimer.color = corTimer;
        }

        private void DesativarTimer()
        {
            if (imagemTimer != null)
            {
                imagemTimer.enabled = false;
                imagemTimer.fillAmount = 0f;
            }
        }

        public void NotificarCopoLiberado()
        {
            copoPronto = false;
        }
    }
}
