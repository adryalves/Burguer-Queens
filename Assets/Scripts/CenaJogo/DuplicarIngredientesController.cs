using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class DuplicarIngredientesController : MonoBehaviour
    {
        [Header("Prefab e Ponto de Spawn")]
        public GameObject ingredientePrefab;
        public Transform spawnPoint;

        // referência para o item atual gerado por este duplicador
        private GameObject instanciaAtual;

        void OnMouseDown()
        {
            Debug.Log($"CLIQUE no duplicador {name}, instanciaAtual = {(instanciaAtual ? instanciaAtual.name : "null")}");

            // Se já existe um item vivo, não cria outro
            if (instanciaAtual != null)
                return;

            // Cria o novo item
            GameObject novoItem = Instantiate(ingredientePrefab, spawnPoint.position, Quaternion.identity);
            instanciaAtual = novoItem;

            // Ajusta posição inicial para o script de arrastar, se existir
            ArrastarItensController drag = novoItem.GetComponent<ArrastarItensController>();
            if (drag != null)
                drag.SetStartPosition(spawnPoint.position);

            Debug.Log("Instanciei " + novoItem.name);

            // Se for um prato, registra a origem para poder liberar depois
            PratoController prato = novoItem.GetComponent<PratoController>();
            if (prato != null)
            {
                prato.spawnerOrigem = this;
            }
        }

        /// <summary>
        /// Chamado pelo item (por exemplo, Prato) quando for destruído / descartado.
        /// </summary>
        public void NotificarItemSaiu(GameObject item)
        {
            if (instanciaAtual == item)
            {
                instanciaAtual = null;
                Debug.Log($"Duplicador {name}: item {item.name} saiu, liberando novo spawn.");
            }
        }
    }
}
