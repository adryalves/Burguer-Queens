using UnityEngine;

public class playerData : MonoBehaviour
{
    public static playerData Instance;

    public int moedas;
    public int frigideiras;
    public int liquidificadores;
    public int bandejas;
    public int precoFrigideira;
    public int precoLiquidificador;
    public int precoBandeja;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlayerSave save = SaveSystem.Load();

            if (save != null)
            {
                moedas = save.moedas;
                frigideiras = save.frigideiras;
                liquidificadores = save.liquidificadores;
                bandejas = save.bandejas;

                precoFrigideira = save.precoFrigideira;
                precoLiquidificador = save.precoLiquidificador;
                precoBandeja = save.precoBandeja;
            }
            else
            {
                // Pega preços INICIAIS do JSON
                precoFrigideira = lojaDataLoader.Instance.GetItem("frigideira").preco;
                precoLiquidificador = lojaDataLoader.Instance.GetItem("liquidificador").preco;
                precoBandeja = lojaDataLoader.Instance.GetItem("bandeja").preco;

                moedas = lojaDataLoader.Instance.GetMoedas();

                frigideiras = 1;
                liquidificadores = 1;
                bandejas = 1;

                Salvar();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool Comprar(string id)
    {
        LojaItem item = lojaDataLoader.Instance.GetItem(id);

        if (item == null)
        {
            Debug.LogError("Item não encontrado: " + id);
            return false;
        }

        switch (id)
        {
            case "frigideira":
                if (frigideiras >= item.quantidadeMax) return false;
                if (moedas < precoFrigideira) return false;

                moedas -= precoFrigideira;
                frigideiras++;
                precoFrigideira += 50; // AUMENTA +50
                break;

            case "liquidificador":
                if (liquidificadores >= item.quantidadeMax) return false;
                if (moedas < precoLiquidificador) return false;

                moedas -= precoLiquidificador;
                liquidificadores++;
                precoLiquidificador += 50;
                break;

            case "bandeja":
                if (bandejas >= item.quantidadeMax) return false;
                if (moedas < precoBandeja) return false;

                moedas -= precoBandeja;
                bandejas++;
                precoBandeja += 50;
                break;

            default:
                Debug.LogError("ID inválido: " + id);
                return false;
        }

        Salvar();
        return true;
    }

    public void Salvar()
    {
        PlayerSave data = new PlayerSave
        {
            moedas = moedas,
            frigideiras = frigideiras,
            liquidificadores = liquidificadores,
            bandejas = bandejas,

            precoFrigideira = precoFrigideira,
            precoLiquidificador = precoLiquidificador,
            precoBandeja = precoBandeja
        };

        SaveSystem.Save(data);
    }
}
