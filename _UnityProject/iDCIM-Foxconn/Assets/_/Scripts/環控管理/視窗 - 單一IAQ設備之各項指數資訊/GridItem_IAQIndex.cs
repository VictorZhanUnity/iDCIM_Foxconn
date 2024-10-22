using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridItem_IAQIndex : MonoBehaviour
{
    public string columnName;
    [SerializeField] private Button btn;
    [SerializeField] protected Image imgICON;
    [SerializeField] private TextMeshProUGUI txtValue;

    public UnityEvent<GridItem_IAQIndex> onClick = new UnityEvent<GridItem_IAQIndex>();

    public virtual Data_IAQ data
    {
        set
        {
            float iaqIndex = float.Parse(value.GetValue(columnName));

            DOTween.To(() => float.Parse(txtValue.text), x =>
            {
                // 更新文字
                if (columnName != "RT" && columnName != "RH") txtValue.text = x.ToString("F0");
                else txtValue.text = x.ToString("0.#");
            }, iaqIndex, 0.15f).SetEase(Ease.OutQuart);
        }
    }
    public Sprite imgICON_Sprite => imgICON.sprite;

    private void Start()
    {
        btn.onClick.AddListener(() => onClick.Invoke(this));
    }

    private void OnValidate()
    {
        btn ??= GetComponent<Button>();
        imgICON ??= transform.Find("imgICON").GetComponent<Image>();
        txtValue ??= transform.Find("txtValue").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        DOTween.To(() => 0, x =>
        {
            // 更新文字
            if (columnName != "RT" && columnName != "RH") txtValue.text = x.ToString("F0");
            else txtValue.text = x.ToString("0.#");
        }, float.Parse(txtValue.text), 0.15f).SetEase(Ease.OutQuart);
    }
}
