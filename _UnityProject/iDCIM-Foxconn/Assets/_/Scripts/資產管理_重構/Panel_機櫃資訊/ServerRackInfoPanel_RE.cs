using DG.Tweening;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// 視窗 - 機櫃資訊面板
/// </summary>
public class ServerRackInfoPanel_RE : MonoBehaviour
{
    [Header(">>> [資料項 ] ")]
    [SerializeField] private Data_ServerRackAsset data;

    [SerializeField] private RackRUList rackRUList;
    [SerializeField] private ProgressBarController pbWatt, pbRuSpace, pbWeight;

    /// <summary>
    /// 所選的資產清單項目
    /// </summary> 
    private ListItem_Device_RE selectedListItem { get; set; }

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(ListItem_Device_RE target)
    {
        selectedListItem = target;
        deviceBaseInfo.ShowData(target.data);
        data = (Data_ServerRackAsset)selectedListItem.data;
        //rackRUList.ShowRULayout(target);
        UpdateUI();
    }
    private void UpdateUI()
    {
        return;

        //設置進度條
        pbWatt.MaxValue = (int)data.information.watt;
        pbRuSpace.MaxValue = (int)data.information.heightU;
        pbWeight.MaxValue = (int)data.information.weight;

        float usageWatt = (int)data.usageOfWatt;
        float usageRuSpace = (int)data.usageOfRU;
        float usageWeight = (int)data.usageOfWeight;

        DotweenHandler.ToLerpValue(0, usageWatt, (value) => pbWatt.value = value, Random.Range(1f, 5));
        DotweenHandler.ToLerpValue(0, usageRuSpace, (value) => pbRuSpace.value = value, Random.Range(1f, 5));
        DotweenHandler.ToLerpValue(0, usageWeight, (value) => pbWeight.value = value, Random.Range(1f, 5));

        pbWatt.value = data.usageOfWatt;
        pbRuSpace.value = data.usageOfRU;
        pbWeight.value = data.usageOfWeight;
    }

    #region [Dotween Effect]
    private void Awake() => originalPos = transform.position;
    private float duration = 0.3f;
    private float scaleValue = 0.9f;
    private float alpha = 0.3f;
    private Vector3 originalPos { get; set; }
    private Vector3 offsetPos = Vector3.zero;
    public void ShowToFront()
    {
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
    private CanvasGroup _canvasGroup { get; set; }
    private CanvasGroup canvasGroup => _canvasGroup ??= transform.GetComponent<CanvasGroup>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.GetChild(0).Find("Container");
    private DeviceAssetBaseInfoDisplay _deviceBaseInfo { get; set; }
    private DeviceAssetBaseInfoDisplay deviceBaseInfo => _deviceBaseInfo ??= container.Find("組件 - 設備基本資訊").GetComponent<DeviceAssetBaseInfoDisplay>();
    #endregion
}