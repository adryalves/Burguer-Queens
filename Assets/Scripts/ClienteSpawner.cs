using UnityEngine;
using System.Collections;
using Assets.Scripts.CenaJogo;

public class ClienteSpawner : MonoBehaviour
{
    public GameObject clientePrefab;
    public Transform spawnPoint;

    public int maxClientes = 3;
    public float delaySpawn = 3f;  

    private int clientesVivos = 0;
    private bool esperandoNovo = false;

    void Start()
    {
        SpawnCliente();
    }

    public void ClienteFoiDestruido()
    {
        clientesVivos--;

        if (clientesVivos < maxClientes && !esperandoNovo)
        {
            StartCoroutine(SpawnComDelay());
        }
    }

    IEnumerator SpawnComDelay()
    {
        esperandoNovo = true;
        yield return new WaitForSeconds(delaySpawn);

        SpawnCliente();
        esperandoNovo = false;
    }

    void SpawnCliente()
    {
        GameObject novo = Instantiate(clientePrefab, spawnPoint.position, Quaternion.identity);

        ClienteController cliente = novo.GetComponent<ClienteController>();
        cliente.spawnerOrigem = this;

        clientesVivos++;
    }
}
