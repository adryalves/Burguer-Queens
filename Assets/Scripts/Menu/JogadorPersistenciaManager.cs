using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class JogadorPersistenciaManager : MonoBehaviour
    {
        [Header("Configuração do Arquivo")]
        [SerializeField] private string fileName = "player_data.json";

        private DadosJogador playerData;
        private DadosJogadorHandler dataHandler;
        private List<IJogadorPersistencia> persistenceObjects;

        public static JogadorPersistenciaManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            dataHandler = new DadosJogadorHandler(Application.persistentDataPath, fileName);

        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            persistenceObjects = FindAllPersistenceObjects();
            LoadPlayerData();
        }

        public void LoadPlayerData()
        {
            playerData = dataHandler.Load();

            foreach (var obj in persistenceObjects)
            {
                obj.LoadData(playerData);
            }
        }

         // Retorna a quantidade de moedas salvas para uma fase (fase começa em 1)
        public int GetMoedasDaFase(int fase)
        {
            if (playerData == null)
            {
                LoadPlayerData();
            }

            if (playerData == null)
                return 0;

            int idx = fase - 1;
            if (playerData.moedaoPorFase != null && idx >= 0 && idx < playerData.moedaoPorFase.Count)
                return playerData.moedaoPorFase[idx];

            return 0;
        }

        public void SavePlayerData()
        {
            foreach (var obj in persistenceObjects)
            {
                obj.SaveData(playerData);
            }

            dataHandler.Save(playerData);
        }

        private List<IJogadorPersistencia> FindAllPersistenceObjects()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                   .OfType<IJogadorPersistencia>()
                   .ToList();
        }
    }
}
