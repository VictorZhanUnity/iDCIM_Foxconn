using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.DoTweenUtils;

/// <summary>
/// 視窗 - 機櫃資訊面板
/// </summary>
public class ServerRackInfoPanel_RE : MonoBehaviour
{
    [Header(">>> 點擊RU設備項目時Invoke")]
    public UnityEvent<Data_DeviceAsset> onClickDevice = new UnityEvent<Data_DeviceAsset>();

    [Header(">>> 點擊機櫃詳細資訊")]
    public UnityEvent<Data_ServerRackAsset> onClickDetailButton = new UnityEvent<Data_ServerRackAsset>();

    [Header(">>> [資料項] ")]
    [SerializeField] private Data_ServerRackAsset data;
    [Header(">>> [Prefab] RUList項目Prefab")]
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;

    /// <summary>
    /// 資產清單目前所選項目
    /// </summary> 
    private ListItem_Device_RE selectedListItem { get; set; }

    private List<DeviceRUItem> ruItemList { get; set; } = new List<DeviceRUItem>();

    

    public void InvokeRackData()
    {
        HideToBackground();
        onClickDetailButton?.Invoke(data);
    }

    /// <summary>
    /// 顯示資料 (From列表點選)
    /// </summary>
    public void ShowData(ListItem_Device_RE target)
    {
        selectedListItem = target;
        ShowData(selectedListItem.data);
    }
    /// <summary>
    /// 顯示資料 (From模型點選)
    /// </summary>
    public void ShowData(Data_iDCIMAsset dataRack)
    {
        deviceBaseInfo.ShowData(dataRack);
        data = (Data_ServerRackAsset)dataRack;

        RaycastHitManager.ToSelectTarget(data.model, false);

        SetupRUList();
        UpdateUI();

        dotween.ToShow();
        ShowToFront();
    }


    /// <summary>
    /// 設定RU設備列表，RU設備座標為listItem自已處理換算
    /// </summary>
    private void SetupRUList()
    {
        //清空資料 
        ruItemList.ForEach(item =>
        {
            item.isOn = false;
            item.OnClickItemEvent.RemoveAllListeners();
        });
        ruItemList.Clear();
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);

        //建立資料
        data.containers.ForEach(deviceAsset =>
        {
            DeviceRUItem item = ObjectPoolManager.GetInstanceFromQueuePool(deviceRUItemPrefab, deviceRUItemContainer);
            item.toggleGroup = toggleGroup;
            item.ShowData(deviceAsset);
            item.OnClickItemEvent.AddListener(OnClickRUDeviceHandler);
            ruItemList.Add(item);
        });
        rackRUList.verticalNormalizedPosition = 1;
    }
    private void OnClickRUDeviceHandler(DeviceRUItem target)
    {
        HideToBackground();
        onClickDevice?.Invoke(target.data);
    }

    private void UpdateUI()
    {
        //設置進度條
        pbWatt.MaxValue = (int)data.information.watt;
        pbRuSpace.MaxValue = (int)data.information.heightU;
        pbWeight.MaxValue = (int)data.information.weight;

        float usageWatt = (int)data.usageOfWatt;
        float usageRuSpace = (int)data.usageOfRU;
        float usageWeight = (int)data.usageOfWeight;

        DotweenHandler.ToLerpValue(0, usageWatt, (value) => pbWatt.value = value, Random.Range(1f, 3f));
        DotweenHandler.ToLerpValue(0, usageRuSpace, (value) => pbRuSpace.value = value, Random.Range(1f, 3f));
        DotweenHandler.ToLerpValue(0, usageWeight, (value) => pbWeight.value = value, Random.Range(1f, 3f));

        pbWatt.value = data.usageOfWatt;
        pbRuSpace.value = data.usageOfRU;
        pbWeight.value = data.usageOfWeight;
    }

    #region [Dotween Effect]
    private void Awake()
    {
        originalPos = transform.position;
        gameObject.SetActive(false);
    }

    private float duration = 0.3f;
    private float scaleValue = 0.9f;
    private float alpha = 0.3f;
    private Vector3 originalPos { get; set; }
    private Vector3 offsetPos = Vector3.zero;

    public void ShowToFront()
    {
        if (gameObject.activeSelf == false) return;

        DOTween.Kill(canvasGroup);
        DOTween.Kill(transform);
        canvasGroup.DOFade(1f, duration).OnUpdate(() => canvasGroup.interactable = canvasGroup.blocksRaycasts = canvasGroup.alpha == 1);
        transform.DOMove(originalPos, duration);
        transform.DOScale(Vector3.one, duration);
    }
    public void HideToBackground()
    {
        DOTween.Kill(canvasGroup);
        DOTween.Kill(transform);
        canvasGroup.DOFade(alpha, duration).OnUpdate(() => canvasGroup.interactable = canvasGroup.blocksRaycasts = canvasGroup.alpha == 1).SetEase(Ease.OutQuad);
        transform.DOMove(originalPos + offsetPos, duration).SetEase(Ease.OutQuad);
        transform.DOScale(scaleValue, duration).SetEase(Ease.OutQuad);
    } 
    #endregion

    #region [Components]
    private DotweenFade2DWithEnabled _dotween { get; set; }
    private DotweenFade2DWithEnabled dotween => _dotween ??= transform.GetComponent<DotweenFade2DWithEnabled>();
    private CanvasGroup _canvasGroup { get; set; }
    private CanvasGroup canvasGroup => _canvasGroup ??= transform.GetComponent<CanvasGroup>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.GetChild(0).Find("Container");
    private DeviceAssetBaseInfoDisplay _deviceBaseInfo { get; set; }
    private DeviceAssetBaseInfoDisplay deviceBaseInfo => _deviceBaseInfo ??= container.Find("組件 - 設備基本資訊").GetComponent<DeviceAssetBaseInfoDisplay>();

    private ScrollRect _rackRUList { get; set; }
    private ScrollRect rackRUList => _rackRUList ??= container.Find("組件 - 機櫃RU內容").GetChild(0).GetChild(0).GetComponent<ScrollRect>();

    private Transform _deviceRUItemContainer { get; set; }
    private Transform deviceRUItemContainer => _deviceRUItemContainer ??= rackRUList.content.GetChild(1).GetComponent<Transform>();

    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= transform.GetComponent<ToggleGroup>();

    private Transform _vLayout { get; set; }
    private Transform vLayout => _vLayout ??= container.Find("組件 - 機櫃資源消耗");
    private ProgressBarController _pbWatt { get; set; }
    private ProgressBarController pbWatt => _pbWatt ??= vLayout.Find("進度條 - 電力").GetComponent<ProgressBarController>();
    private ProgressBarController _pbRuSpace { get; set; }
    private ProgressBarController pbRuSpace => _pbRuSpace ??= vLayout.Find("進度條 - 機櫃空間使用率").GetComponent<ProgressBarController>();
    private ProgressBarController _pbWeight { get; set; }
    private ProgressBarController pbWeight => _pbWeight ??= vLayout.Find("進度條 - 承重").GetComponent<ProgressBarController>();
    #endregion
}