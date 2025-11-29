using Assets.Scripts.CenaJogo;
using UnityEngine;

public class BandejaController : MonoBehaviour
{
    public PratoController pratoAtual;

    void Update()
    {
        if (pratoAtual != null)
        {
            pratoAtual.transform.position =
                transform.position + new Vector3(0, 0.7f, 0);
        }
    }
}
