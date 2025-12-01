using System.Collections;
using UnityEngine;
using Assets.Scripts.CenaJogo;

public class PratoController : MonoBehaviour
{
    public bool estaNaBandeja = false;

    private Transform bandejaTransform;
    private BandejaController bandejaController;

    [Header("Offsets de posição")]
    public Vector3 offsetNaBandeja = Vector3.zero;

    [SerializeField] private int baseOrderInLayer = 1000;

    // 🔹 NOVO: quem gerou este prato (duplicador da gaveta)
    [HideInInspector] public DuplicarIngredientesController spawnerOrigem;

    public void OnDragReleased()
    {
        if (estaNaBandeja && bandejaTransform != null)
        {
            transform.SetParent(bandejaTransform);
            transform.localPosition = offsetNaBandeja;
        }
        else
        {
            // se soltar fora da bandeja, o prato é destruído
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        AreaDetector area = col.GetComponent<AreaDetector>();

        if (area != null && area.areaName == "Bandeja")
        {
            BandejaController bc = col.GetComponentInParent<BandejaController>();

            if (bc != null)
            {
                if (bc.pratoAtual != null && bc.pratoAtual != this)
                {
                    Destroy(this.gameObject);
                    return;
                }

                bandejaController = bc;
                bandejaController.pratoAtual = this;
                bandejaTransform = bc.transform;
            }
            else
            {
                bandejaTransform = col.transform;
            }

            estaNaBandeja = true;
            transform.position = bandejaTransform.position;

            var drag = GetComponent<Assets.Scripts.CenaJogo.ArrastarItensController>();
            if (drag != null) Destroy(drag);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        AreaDetector area = col.GetComponent<AreaDetector>();

        if (area != null && area.areaName == "Bandeja")
        {
            estaNaBandeja = false;

            if (bandejaController != null && bandejaController.pratoAtual == this)
            {
                bandejaController.pratoAtual = null;
            }

            bandejaTransform = null;
        }
    }

    public void ColocarIngredienteNoPrato(GameObject ingrediente)
    {
        Transform alvo = transform.Find("IngredientesEmpilhados");
        if (alvo == null)
        {
            Debug.LogWarning("Prato: não encontrou o filho 'IngredientesEmpilhados'. Usando o próprio prato.");
            alvo = transform;
        }

        ingrediente.transform.SetParent(alvo, worldPositionStays: false);

        int index = alvo.childCount - 1;

        float offsetY = 2.0f;

        ingrediente.transform.localPosition = new Vector3(0f, 0.5f + index * offsetY, -1f);

        SpriteRenderer sr = ingrediente.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = baseOrderInLayer + index;
        }

        var drag = ingrediente.GetComponent<Assets.Scripts.CenaJogo.ArrastarItensController>();
        if (drag != null) Destroy(drag);

        var col2D = ingrediente.GetComponent<Collider2D>();
        if (col2D != null) col2D.enabled = false;
    }

    public bool TemAlgumIngredienteNoPrato()
    {
        Transform alvo = transform.Find("IngredientesEmpilhados");
        if (alvo == null)
        {
            alvo = transform;
        }

        return alvo.childCount > 0;
    }

    public string GerarCodigoPorNome()
    {
        Transform pilha = transform.Find("IngredientesEmpilhados");
        if (pilha == null) return string.Empty;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < pilha.childCount; i++)
        {
            string nome = pilha.GetChild(i).name;
            sb.Append(ConverterNomeParaCodigo(nome));
        }

        return sb.ToString();
    }

    private string ConverterNomeParaCodigo(string nome)
    {
        switch (nome)
        {
            case "PaoBase":
                return "B";
            case "Carne":
                return "C";
            case "Queijo":
                return "Q";
            case "PaoCima":
            case "Pao":
                return "P";
            default:
                return "?";
        }
    }

    // 🔹 NOVO: qualquer destruição do prato libera o duplicador
    private void OnDestroy()
    {
        if (spawnerOrigem != null)
        {
            spawnerOrigem.NotificarItemSaiu(this.gameObject);
        }
    }
}
