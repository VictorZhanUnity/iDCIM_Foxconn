using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridItem_IAQIndex : MonoBehaviour
{
    public string columnName;
    [SerializeField] private Button btn;
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtValue;

    public UnityEvent<GridItem_IAQIndex> onClick = new UnityEvent<GridItem_IAQIndex>();

    public float value { set => txtValue.SetText(value.ToString()); }
    public Sprite imgICON_Sprite => imgICON.sprite;

    private void Start()
    {
        btn.onClick.AddListener(()=>onClick.Invoke(this));
    }

    private void OnValidate()
    {
        btn ??= GetComponent<Button>();
        imgICON ??= transform.Find("imgICON").GetComponent<Image>();
        txtValue ??= transform.Find("txtValue").GetComponent<TextMeshProUGUI>();
    }
}
