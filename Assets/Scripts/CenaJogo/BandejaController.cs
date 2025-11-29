using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class BandejaController : MonoBehaviour
    {
       
        public PratoController pratoAtual;

        void Update()
        {
           
            if (pratoAtual != null)
            {
                
                pratoAtual.transform.position =
                    transform.position + new Vector3(0f, 0.1f, 0f);
            }
        }
    }
}
