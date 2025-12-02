
﻿using System.Collections;
using UnityEngine;

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

        [Header("Tempo de preparo (segundos)")]
        public float tempoProcessar = 2f;

        private bool estaProcessando = false;
        private bool copoPronto = false;

        void Awake()
        {
            if (maquinaRenderer == null)
                maquinaRenderer = GetComponent<SpriteRenderer>();
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

            yield return new WaitForSeconds(tempoProcessar);

            if (maquinaRenderer != null && spriteMaquinaVazia != null)
                maquinaRenderer.sprite = spriteMaquinaVazia;

           
            if (copoDestino != null)
                copoDestino.EncherCopo();

            estaProcessando = false;
            copoPronto = true;
        }

        
        public void NotificarCopoLiberado()
        {
            copoPronto = false;
        }
    }
}
