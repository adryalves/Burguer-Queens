using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void Save(PlayerSave data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("SAVE FEITO EM: " + savePath);
    }

    public static PlayerSave Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Nenhum save encontrado. Criando novo.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<PlayerSave>(json);
    }
}
