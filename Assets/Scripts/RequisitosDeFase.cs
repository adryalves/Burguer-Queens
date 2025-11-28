using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FaseRequisito
{
    public int fase;
    public int moedas;
}

[System.Serializable]
public class FaseRequisitosContainer
{
    public List<FaseRequisito> fases;
}

public class RequisitosDeFase : MonoBehaviour
{
    private FaseRequisitosContainer dados;

    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("faseRequisitos");
        dados = JsonUtility.FromJson<FaseRequisitosContainer>(jsonFile.text);
    }

    public int GetRequisitoDaFase(int fase)
    {
        foreach (var item in dados.fases)
        {
            if (item.fase == fase)
                return item.moedas;
        }

        return 0;
    }
}
