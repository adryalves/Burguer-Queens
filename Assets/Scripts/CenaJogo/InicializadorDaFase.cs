using UnityEngine;

public class InicializadorDaFase : MonoBehaviour
{
    void Awake()
    {
        Assets.Scripts.CenaJogo.CarneController.ResetFrigideiraOcupada();
    }
}
