using UnityEngine;

public interface IJogadorPersistencia
{
    void LoadData(DadosJogador data);
    void SaveData(DadosJogador data);
}
