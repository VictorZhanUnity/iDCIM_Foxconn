using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// 九宮格資料項目
/// </summary>
public class GridItem_9Grid : MonoBehaviour
{
    public GameObject canvasObj;
    [SerializeField] private Image cctvScreen, border;
    [SerializeField] private TextMeshProUGUI txtDeviceName;
    [SerializeField] private Button btnScale;

    public SO_RTSP data;

    public UnityEvent<SO_RTSP> onClickScaleBtn = new UnityEvent<SO_RTSP>();
    public UnityEvent<GridItem_9Grid> onClickCloseBtn = new UnityEvent<GridItem_9Grid>();

    private Color originalColor;

    public ListItem_CCTV listItem;

    private void Start()
    {
        originalColor = border.color;
        btnScale.onClick.AddListener(() => onClickScaleBtn.Invoke(data));
    }

    public void Show(SO_RTSP data)
    {
        this.data = data;
        canvasObj.SetActive(true);
        cctvScreen.sprite = data.sprite;

        listItem.isDisplay = true;
        txtDeviceName.SetText(data.title);
    }

    public void Close()
    {
        onClickCloseBtn.Invoke(this);
        ObjectPoolManager.PushToPool<GridItem_9Grid>(this);

        listItem.isDisplay = false;
    }
    public void ToShining() => ColorHandler.LerpColor(border, Color.red, originalColor);
}
