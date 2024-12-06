using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDeploy : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header(">>> [Event] 新建設備模型時Invoke {新增設備, 目標機櫃U層}")]
    public UnityEvent<StockDeviceListItem, RackSpacer> onCreateTempDevice = new UnityEvent<StockDeviceListItem, RackSpacer>();

    public Image dragImagePrefab;
    public Vector3 offset = new Vector3(0, 0, -0.075f);
    private StockDeviceListItem _stockDeviceItem { get; set; }
    private StockDeviceListItem stockDeviceItem => _stockDeviceItem ??= GetComponent<StockDeviceListItem>();
    private Canvas _canvas { get; set; }
    private Canvas canvas => _canvas ??= GameManager.mainCanvas;

    private Image dragImage { get; set; }

    private string containerTag { get; set; } = "BuildContainer_Device";

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 創建拖拽用的Image
        dragImage = Instantiate(dragImagePrefab, canvas.transform);
        dragImage.transform.SetAsLastSibling(); // 確保在UI最上層
        dragImage.raycastTarget = false; // 防止拖拽影響UI交互
        //dragImage.sprite = stockDeviceItem.data.dragIcon;

        // 添加半透明效果
        Color color = dragImage.color;
        color.a = 0.7f; // 設置透明度
        dragImage.color = color;

        // 添加放大效果
        dragImage.rectTransform.DOScale(1.2f, 0.1f);

        // 添加陰影效果（可選）
        Shadow shadow = dragImage.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.5f);
        shadow.effectDistance = new Vector2(5, -5);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);
            dragImage.rectTransform.localPosition = localPoint;
        }
    }

    public LayerMask layerMask_RUSpacer;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            //檢測鼠標沒有在UI物件上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 檢測是否拖拽到目標物件A
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // 檢查命中的物件是否是目標物件A
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask_RUSpacer))
                {
                    //取得對像RackSpacer
                    RackSpacer targetRackSapcer = hit.transform.GetComponent<RackSpacer>();

                    Debug.Log($"isAbleToUpload: {targetRackSapcer.isAbleToUpload(stockDeviceItem.data.deviceAsset)}");

                    if (targetRackSapcer.isAbleToUpload(stockDeviceItem.data.deviceAsset))
                    {
                        onCreateTempDevice.Invoke(stockDeviceItem, targetRackSapcer);
                    }
                }
            }
            Destroy(dragImage.gameObject); // 清理拖拽Image
        }
    }
}
