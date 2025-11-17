using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    public class AbrirLojaScript : MonoBehaviour
    {
        public void AbrirLoja()
        {
            SceneManager.LoadScene("CenaLoja");
        }
    }
}
