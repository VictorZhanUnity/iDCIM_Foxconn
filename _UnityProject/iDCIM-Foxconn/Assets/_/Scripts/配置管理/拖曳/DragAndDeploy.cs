using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class DragAndDeploy : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerUpHandler
{
    [Header(">>> [Event] 新建設備模型時Invoke {新增設備, 目標機櫃U層}")]
    public UnityEvent<ListItem_Device_RE, RackSpacer> onCreateTempDevice = new UnityEvent<ListItem_Device_RE, RackSpacer>();

    [Header(">>> [Prefab] - 拖曳時的顯示組件")]
    [SerializeField] private Transform dragObjectPrefab;

    [Header(">>> Offset偏移值")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -0.075f);

    #region [Components]
    /// <summary>
    /// RU空格的Tag
    /// </summary>
    public float scale = 1.2f;
    public float alpha = 0.7f;
    public float duration = 0.3f;
    private Transform dragObjectDisplayer { get; set; }
    private Canvas _canvas { get; set; }
    private Canvas canvas => _canvas ??= GameManager.mainCanvas;
    private Camera _mainCamera { get; set; }
    private Camera mainCamera => _mainCamera ??= Camera.main;

    private ListItem_Device_RE _listItem { get; set; }
    private ListItem_Device_RE listItem => _listItem ??= GetComponent<ListItem_Device_RE>();
    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 創建拖拽用的Image
        dragObjectDisplayer = Instantiate(dragObjectPrefab, canvas.transform);
        dragObjectDisplayer.transform.SetAsLastSibling(); // 確保在UI最上層

        // 添加半透明效果
        if (dragObjectDisplayer.TryGetComponent(out CanvasGroup canvasGroup) == false)
        {
            canvasGroup = dragObjectDisplayer.AddComponent<CanvasGroup>();
        }
        canvasGroup.DOFade(alpha, duration).SetEase(Ease.OutQuad);

        // 添加放大效果
        (dragObjectDisplayer.transform as RectTransform).DOScale(scale, duration).SetEase(Ease.OutQuad);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragObjectDisplayer != null)
        {
            //將螢幕座標轉換為該UI組件的Local座標
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);
            (dragObjectDisplayer.transform as RectTransform).localPosition = localPoint;

            //檢測鼠標沒有在UI物件上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 檢測是否拖拽到目標物件A
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // 檢查命中的物件是否是目標物件A
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.TryGetComponent(out RackSpacer targetRackSapcer))
                    {
                        if (listItem.data is Data_DeviceAsset dataDevice)
                        {
                            if (targetRackSapcer.isAbleToUpload(dataDevice))
                            {
                                if (listItem.targetRack != targetRackSapcer.dataRack)
                                {
                                    listItem.occupyRackSpacers.ForEach(ruSpacer => ruSpacer.isForceToShow = false);
                                    listItem.occupyRackSpacers.Clear();
                                }
                                listItem.targetRack = targetRackSapcer.dataRack;
                                listItem.occupyRackSpacers = targetRackSapcer.dataRack.ShowRackSpacer(targetRackSapcer.RuLocation, dataDevice.information.heightU);
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragObjectDisplayer != null)
        {
            //檢測鼠標沒有在UI物件上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 檢測是否拖拽到目標物件A
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // 檢查命中的物件是否是目標物件A
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.TryGetComponent(out RackSpacer targetRackSapcer))
                    {
                        if (listItem.data is Data_DeviceAsset dataDevice)
                        {
                            if (targetRackSapcer.isAbleToUpload(dataDevice))
                            {
                                onCreateTempDevice.Invoke(listItem, targetRackSapcer);
                            }
                        }
                    }
                }
            }
            Destroy(dragObjectDisplayer.gameObject); // 清理拖拽Image
        }
    }

    /// <summary>
    /// 當OnDrag離開時，隱藏
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (dragObjectDisplayer != null)
        {
            //檢測鼠標沒有在UI物件上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 檢測是否拖拽到目標物件A
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // 檢查命中的物件是否是目標物件A
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.TryGetComponent(out RackSpacer targetRackSapcer))
                    {
                        if (listItem.data is Data_DeviceAsset dataDevice)
                        {
                            if (targetRackSapcer.isAbleToUpload(dataDevice))
                            {
                                onCreateTempDevice.Invoke(listItem, targetRackSapcer);
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragObjectDisplayer != null) Destroy(dragObjectDisplayer.gameObject); // 清理拖拽Image
    }
}
