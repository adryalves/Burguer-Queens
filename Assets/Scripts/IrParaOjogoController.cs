using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class IrParaOjogoController : MonoBehaviour
    {
        public void CarregarCenaJogo()
        {
            SceneManager.LoadScene("CenaFase");
        }
    }
}
