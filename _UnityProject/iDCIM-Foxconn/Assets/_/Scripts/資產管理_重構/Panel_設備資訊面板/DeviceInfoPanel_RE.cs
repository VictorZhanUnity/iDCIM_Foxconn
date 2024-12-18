using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.DoTweenUtils;


/// <summary>
/// 視窗 - 設備資訊面板(設備 or 機櫃)
/// </summary>
public class DeviceInfoPanel_RE : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_iDCIMAsset dataAsset;

    [Header(">>> [Prefab] 列表項目Prefab")]
    [SerializeField] private ListItem_COBie listItemPrefab;

    /// <summary>
    /// 顯示資料(機櫃 / 設備)
    /// </summary>
    public void ShowData(Data_iDCIMAsset data)
    {
        dataAsset = data;
        deviceBaseInfo.ShowData(dataAsset);

        if (data is Data_DeviceAsset dataDevice)
        {
            txtRuLocation.gameObject.SetActive(dataDevice.rackLocation != 0);
            DotweenHandler.ToBlink(txtRuLocation, $"U{dataDevice.rackLocation.ToString()}");
        }
        else txtRuLocation.gameObject.SetActive(false);

        icon_DCR.gameObject.SetActive(icon_DCR.name.Contains(data.system));
        icon_DCN.gameObject.SetActive(icon_DCN.name.Contains(data.system));
        icon_DCS.gameObject.SetActive(icon_DCS.name.Contains(data.system));

        BuildCOBieList(dataAsset);

        dotween.ToShow();
    }

    /// <summary>
    /// 建立COBie資料清單
    /// </summary>
    private void BuildCOBieList(Data_iDCIMAsset data)
    {
        //移除項目
        ObjectPoolManager.PushToPool<ListItem_COBie>(scrollRect.content);

        data.information.COBieMapData.ToList().ForEach(data =>
        {
            ListItem_COBie item = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_COBie>(listItemPrefab, scrollRect.content);
            item.ShowData(data.Key, data.Value);
        });
        scrollRect.verticalNormalizedPosition = 1;
    }

    private void Awake() => gameObject.SetActive(false);

    #region [Components]
    private DotweenFade2DWithEnabled _dotween { get; set; }
    private DotweenFade2DWithEnabled dotween => _dotween ??= transform.GetComponent<DotweenFade2DWithEnabled>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.Find("Container");
    private Transform _header { get; set; }
    private Transform header => _header ??= container.Find("Header");
    private TextMeshProUGUI _txtRuLocation { get; set; }
    private TextMeshProUGUI txtRuLocation => _txtRuLocation ??= header.Find("txtRuLocation").GetComponent<TextMeshProUGUI>();
    private Image _icon_DCR { get; set; }
    private Image icon_DCR => _icon_DCR ??= header.Find("imgICON_DCR").GetComponent<Image>();
    private Image _icon_DCS { get; set; }
    private Image icon_DCS => _icon_DCS ??= header.Find("imgICON_DCS").GetComponent<Image>();
    private Image _icon_DCN { get; set; }
    private Image icon_DCN => _icon_DCN ??= header.Find("imgICON_DCN").GetComponent<Image>();
    private DeviceAssetBaseInfoDisplay _deviceBaseInfo { get; set; }
    private DeviceAssetBaseInfoDisplay deviceBaseInfo => _deviceBaseInfo ??= container.Find("組件 - 設備基本資訊").GetComponent<DeviceAssetBaseInfoDisplay>();
    private ScrollRect _scrollRect { get; set; }
    private ScrollRect scrollRect => _scrollRect ??= container.Find("ScrollView滑動列表").GetComponent<ScrollRect>();
    #endregion
}
