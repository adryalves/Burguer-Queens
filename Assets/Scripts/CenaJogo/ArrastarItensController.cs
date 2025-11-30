using System;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class ArrastarItensController : MonoBehaviour
    {
        private Vector3 startPosition;
        private bool dragging = false;
        private Vector3 offset;

        private SpriteRenderer sr;
        private int sortingOrderOriginal;

        public bool podeInteragir = true;

        void Start()
        {
            startPosition = transform.position;

           
            sr = GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = GetComponentInChildren<SpriteRenderer>();

            if (sr != null)
                sortingOrderOriginal = sr.sortingOrder;
        }

        void OnMouseDown()
        {
            if (!podeInteragir) return;
           
            var carne = GetComponent<CarneController>();
            if (carne != null && !carne.PodeSerArrastada())
                return;

            dragging = true;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            offset = transform.position - mouse;

           
            if (sr != null && GetComponent<BandejaController>() == null)
                sr.sortingOrder = 1000;
        }

        void OnMouseDrag()
        {
            if (!podeInteragir) return;
            if (!dragging) return;

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            transform.position = mouse + offset;
        }

        void OnMouseUp()
        {
            if (!dragging) return;

            dragging = false;

            
            if (sr != null && GetComponent<BandejaController>() == null)
                sr.sortingOrder = sortingOrderOriginal;

            
            SendMessage("OnDragReleased", SendMessageOptions.DontRequireReceiver);
        }

        public void ResetToStart()
        {
            transform.position = startPosition;
        }

        public void SetStartPosition(Vector3 pos)
        {
            startPosition = pos;
            transform.position = pos;
        }

        public void AtivarInteracao(bool estado)
        {
            podeInteragir = estado;
        }
    }
}
