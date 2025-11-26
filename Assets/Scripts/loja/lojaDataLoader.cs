using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LojaItem
{
    public string id;
    public int preco;
    public int quantidadeMax;
}

[System.Serializable]
public class LojaData
{
    public int moedas;
    public List<LojaItem> items;
}

public class lojaDataLoader : MonoBehaviour
{
    public static lojaDataLoader Instance;

    private LojaData lojaData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            CarregarJSON();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CarregarJSON()
    {
         TextAsset jsonFile = Resources.Load<TextAsset>("loja_data");

        if (jsonFile == null)
        {
            Debug.LogError("ERRO: Não encontrou o arquivo loja_data.json em Resources!");
            return;
        }

        lojaData = JsonUtility.FromJson<LojaData>(jsonFile.text);

        if (lojaData == null || lojaData.items == null)
        {
            Debug.LogError("ERRO: JSON inválido ou lista de itens vazia!");
        }
    }

    public LojaItem GetItem(string id)
    {
        return lojaData.items.Find(item => item.id == id);
    }

    public int GetMoedas()
    {
        return lojaData.moedas;
    }
}
