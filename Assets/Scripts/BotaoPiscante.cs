using UnityEngine;

public class BotaoPiscante : MonoBehaviour
{
    public float velocidade = 5f;
    private CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * velocidade));
        cg.alpha = Mathf.Lerp(0.6f, 1f, alpha);
    }
}
