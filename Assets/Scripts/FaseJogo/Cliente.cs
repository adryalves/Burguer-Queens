using UnityEngine;

public class Cliente : MonoBehaviour
{
    public float velocidade = 10f;

    private Transform destino;
    private ClienteSpawner spawner;
    private bool andando = false;
    private bool saindo = false;

    public Transform pontoSaida; // crie um ponto de saída à direita

    public void AndarPara(Transform novoDestino, ClienteSpawner sp)
    {
        destino = novoDestino;
        spawner = sp;
        andando = true;
        saindo = false;
    }

    void Update()
    {
        if (andando && destino != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino.position,
                velocidade * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, destino.position) < 0.1f)
            {
                andando = false;

                if (!saindo)
                    Invoke(nameof(Sair), 4f); // espera 4 segundos no balcão
            }
        }
    }

    void Sair()
    {
        saindo = true;
        andando = true;
        destino = pontoSaida;
    }

    void OnDestroy()
    {
        if (spawner != null)
            spawner.ClienteSaiu(this);
    }
}
