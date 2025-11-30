using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienteSpawner : MonoBehaviour
{
    public GameObject[] clientesPrefabs;
    public Transform spawnPoint;
    public Transform[] pontosFila;  // Ponto_1, Ponto_2, Ponto_3, Ponto_4

    private List<Cliente> filaClientes = new List<Cliente>();

    public float tempoMin = 3f;
    public float tempoMax = 7f;

    void Start()
    {
        StartCoroutine(SpawnClientes());
    }

    IEnumerator SpawnClientes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(tempoMin, tempoMax));

            // Se já tem 4 clientes → NÃO cria mais
            if (filaClientes.Count >= pontosFila.Length)
                continue;

            // Pega cliente aleatório
            int index = Random.Range(0, clientesPrefabs.Length);
            GameObject novo = Instantiate(clientesPrefabs[index], spawnPoint.position, Quaternion.identity);

            Cliente cliente = novo.GetComponent<Cliente>();

            // adiciona na fila
            filaClientes.Add(cliente);

            // manda ele para o ponto da fila correspondente
            cliente.AndarPara(pontosFila[filaClientes.Count - 1], this);
        }
    }

    // Quando o cliente sair da fila
    public void ClienteSaiu(Cliente cli)
    {
        filaClientes.Remove(cli);

        // reorganizar posições
        for (int i = 0; i < filaClientes.Count; i++)
        {
            filaClientes[i].AndarPara(pontosFila[i], this);
        }
    }
}
