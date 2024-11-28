using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDeploy : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header(">>> [Event] 新建設備模型時Invoke {新增設備, 目標機櫃U層}")]
    public UnityEvent<RackSpacer> onCreateTempDevice = new UnityEvent<RackSpacer>();

    public Image dragImagePrefab;
    public Vector3 offset = new Vector3(0, 0, -0.075f);
    private StockDeviceListItem _item { get; set; }
    private StockDeviceListItem item => _item ??= GetComponent<StockDeviceListItem>();
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
        dragImage.sprite = item.data.dragIcon;

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
                out Vector2 localPoint
            );
            dragImage.rectTransform.localPosition = localPoint;
        }
    }

    private Transform model { get; set; }
    private Transform hitContainer { get; set; }

    public RackSpacer rackSpacer { get; private set; }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            // 檢測是否拖拽到目標物件A
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 檢查命中的物件是否是目標物件A
                if (hit.collider.CompareTag(containerTag))
                {
                    //先取得對像模型的中心點位置
                    Vector3 targetPos = Vector3.zero;

                    var createModel = item.data.model;
                    //  createModel = tempDevicePrefab;

                    if (createModel.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        targetPos = renderer.bounds.center;
                    }
                    // 創建替換物件B
                    // Transform model = Instantiate(createModel, hit.transform.position - targetPos - offset, hit.transform.rotation);
                    //Transform model = Instantiate(createModel, hit.transform.position - targetPos, hit.transform.rotation);
                    //model.parent = hit.transform.parent;

                    /*  if (model != null) Destroy(model.gameObject);
                      model = Instantiate(createModel, hit.transform);
                      //  model.position = hit.transform.position;

                      Vector3 center = Vector3.zero;
                      if (hit.transform.TryGetComponent<Renderer>(out Renderer renderer1))
                      {
                          center = renderer1.bounds.center;
                      }

                      Debug.Log($"targetPos: {targetPos}/center: {center}");
                      model.localPosition = Vector3.zero;
                      model.rotation = Quaternion.Euler(Vector3.zero);*/

                    //hit.transform.SetAsLastSibling();

                    //取得對像RackSpacer
                    rackSpacer = hit.transform.parent.GetComponent<RackSpacer>();
                    rackSpacer.CreateTempDevice(item.data.model);
                    // Destroy(hit.collider.gameObject); // 刪除目標物件A

                    //Invoke {建立的模型, 對像RackSpacer}
                    onCreateTempDevice.Invoke(rackSpacer);
                }
            }
            Destroy(dragImage.gameObject); // 清理拖拽Image
        }
    }
}
