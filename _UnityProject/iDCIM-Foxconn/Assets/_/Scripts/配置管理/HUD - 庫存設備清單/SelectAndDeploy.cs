using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectAndDeploy : MonoBehaviour
{
    public Image dragImagePrefab;
    private Image dragImage { get; set; }
    private Canvas _canvas { get; set; }
    private Canvas canvas => _canvas ??= GameManager.mainCanvas;
    private StockDeviceListItem _item { get; set; }
    private StockDeviceListItem item => _item ??= GetComponent<StockDeviceListItem>();

    private string containerTag = "BuildContainer_Device";

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    private void OnToggleEvent(StockDeviceListItem item)
    {
        if (item.isOn) CreateDragImg();
        else Destroy(dragImage.gameObject); // 清理拖拽Image
    }

    private void Update()
    {
        if (dragImage != null)
        {
            Vector2 screenPoint = Input.mousePosition; // 滑鼠位置（螢幕座標）
            Vector2 localPoint;

           if( RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPoint,
                Camera.main,
                out localPoint
            ))
            {
                // localPoint 現在是 targetRect 的本地座標
                dragImage.rectTransform.localPosition = localPoint;
            }
            else
            {
                // 如果螢幕座標不在 RectTransform 內部
                Debug.Log("Mouse is outside the RectTransform.");
            }

            if (Input.GetMouseButtonDown(0)) DeployDevice(); 
        }
    }

    private void CreateDragImg()
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
        dragImage.rectTransform.DOScale(1.2f, 0.7f);

        // 添加陰影效果（可選）
        Shadow shadow = dragImage.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.5f);
        shadow.effectDistance = new Vector2(5, -5);
    }
    private void DeployDevice()
    {
        if (dragImage != null)
        {
            Vector3 offset = new Vector3(0, 0, -0.075f);

            // 檢測是否拖拽到目標物件A
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 檢查命中的物件是否是目標物件A
                if (hit.collider.CompareTag(containerTag))
                {
                    //先取得對像模型的中心點位置
                    Vector3 targetPos = Vector3.zero;
                    if (item.data.model.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        targetPos = renderer.bounds.center;
                    }
                    // 創建替換物件B
                    Transform model = Instantiate(item.data.model, hit.transform.position - targetPos - offset, hit.transform.rotation);
                    model.parent = hit.transform.parent;
                    model.SetAsLastSibling();

                    Destroy(hit.collider.gameObject); // 刪除目標物件A
                }
            Destroy(dragImage.gameObject); // 清理拖拽Image
            }
        }
    }
}
