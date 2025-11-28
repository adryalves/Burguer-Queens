using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class DadosJogadorHandler
    {
        private string fullPath;

        public DadosJogadorHandler(string directoryPath, string fileName)
        {
            fullPath = Path.Combine(directoryPath, fileName);
        }

        public DadosJogador Load()
        {
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Arquivo não encontrado, criando novo: " + fullPath);

                DadosJogador novo = new DadosJogador();
                Save(novo);
                return novo;
            }

            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<DadosJogador>(json);
        }

        public void Save(DadosJogador data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(fullPath, json);
        }
    }
}
