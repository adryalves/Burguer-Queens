using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class ArrastarItensController : MonoBehaviour
    {

        private Vector3 startPosition;
        private bool dragging = false;
        private Vector3 offset;

        void Start()
        {
            startPosition = transform.position;
        }

        void OnMouseDown()
        {
            dragging = true;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            offset = transform.position - mouse;
        
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = 1000;
        }

        void OnMouseDrag()
        {
            if (!dragging) return;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            transform.position = mouse + offset;
        }

        void OnMouseUp()
        {
            dragging = false;
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = 0;
        
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
    }


}
