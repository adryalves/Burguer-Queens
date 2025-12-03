using UnityEngine;
using UnityEngine.EventSystems;

public class InicializadorDaFase : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;

        Input.ResetInputAxes();

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        Assets.Scripts.CenaJogo.CarneController.ResetFrigideiraOcupada();
    }
}
