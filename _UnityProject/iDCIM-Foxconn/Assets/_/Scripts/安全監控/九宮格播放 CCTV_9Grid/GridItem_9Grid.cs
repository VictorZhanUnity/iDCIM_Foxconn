using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 九宮格資料項目
/// </summary>
public class GridItem_9Grid : MonoBehaviour
{
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private Image cctvScreen, border;
    [SerializeField] private TextMeshProUGUI txtDeviceName;
    [SerializeField] private Button btnScale;

    public SO_RTSP data;

    public UnityEvent<Texture2D> onClickScaleBtn = new UnityEvent<Texture2D>();
    public UnityEvent<GridItem_9Grid> onClickCloseBtn = new UnityEvent<GridItem_9Grid>();

    private Color originalColor;

    private void Start()
    {
        originalColor = border.color;
        btnScale.onClick.AddListener(() => onClickScaleBtn.Invoke(cctvScreen.sprite.texture));
    }

    public void Show(SO_RTSP data)
    {
        this.data = data;
        canvasObj.SetActive(true);
        cctvScreen.sprite = data.sprite;
    }

    public void Close()
    {
        onClickCloseBtn.Invoke(this);
        ObjectPoolManager.PushToPool<GridItem_9Grid>(this);
        canvasObj.SetActive(false);
    }
    public void ToShining() => ColorHandler.LerpColor(border, Color.red, originalColor);
}
