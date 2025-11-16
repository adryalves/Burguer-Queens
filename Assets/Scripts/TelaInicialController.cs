using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TelaInicialController : MonoBehaviour
{
    public CanvasGroup fundo;
    public CanvasGroup novoFundo;
    public CanvasGroup botoes;

    public float tempoExibicao = 1.5f;
    public float duracaoFade = 0.5f;

    IEnumerator Start()
    {
        fundo.alpha = 1;
        novoFundo.alpha = 0;
        botoes.alpha = 0;
        botoes.gameObject.SetActive(false);

        yield return new WaitForSeconds(tempoExibicao);

        yield return StartCoroutine(CrossFade(fundo, novoFundo, duracaoFade));

        botoes.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn(botoes, duracaoFade));
    }

    IEnumerator CrossFade(CanvasGroup cg1, CanvasGroup cg2, float dur)
    {
        float t = 0;
        while (t < dur)
        {
            float p = t / dur;

            cg1.alpha = 1 - p;
            cg2.alpha = p;

            t += Time.deltaTime;
            yield return null;
        }

        cg1.alpha = 0;
        cg2.alpha = 1;
    }

    IEnumerator FadeIn(CanvasGroup cg, float dur)
    {
        float t = 0;
        while (t < dur)
        {
            cg.alpha = Mathf.Lerp(0, 1, t / dur);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1;
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("CenaMenu");
    }
}
