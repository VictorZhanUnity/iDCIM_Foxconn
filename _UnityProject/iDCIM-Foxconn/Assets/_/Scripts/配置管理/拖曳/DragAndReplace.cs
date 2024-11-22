using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndReplace : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject replacementPrefab; // 用於替換的3D物件B
    public string containerTag = "BuildContainer_Device";
    public Image dragImagePrefab;
    private Image dragImage;

    public Sprite sprite;
    public Canvas canvas;
    public Vector3 offset = new Vector3(0, 0, -0.075f);

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 創建拖拽用的Image
        dragImage = Instantiate(dragImagePrefab, canvas.transform);
        dragImage.transform.SetAsLastSibling(); // 確保在UI最上層
        dragImage.raycastTarget = false; // 防止拖拽影響UI交互
        dragImage.sprite = sprite;

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
                    if (replacementPrefab.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        targetPos = renderer.bounds.center;
                    }
                    // 創建替換物件B
                    GameObject model = Instantiate(replacementPrefab, hit.transform.position - targetPos - offset, hit.transform.rotation);
                    model.transform.parent = hit.transform.parent;
                    model.transform.SetAsLastSibling();

                    Destroy(hit.collider.gameObject); // 刪除目標物件A
                }
            }
            Destroy(dragImage.gameObject); // 清理拖拽Image
        }
    }
}
