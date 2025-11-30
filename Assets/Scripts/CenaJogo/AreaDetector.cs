using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    
    public class AreaDetector : MonoBehaviour
    {
        public string areaName;  

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
        }
    }

}
