using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// [資產管理] 顯示資產(機櫃/設備)基本資訊
/// </summary>
public class DeviceAssetBaseInfoDisplay : MonoBehaviour
{
    [Header(">>> [目前資料項]")]
    [SerializeField] private Data_iDCIMAsset data;

    [Header(">>> UI組件")]
    [SerializeField] private ListItem_COBie listItemPrefab;

    public Color colorDCR;
    public Color colorDCN;
    public Color colorDCS;

    public void ShowData(Data_iDCIMAsset data)
    {
        this.data = data;
        UpdateUI();
    }

    private void UpdateUI()
    {
        DotweenHandler.ToBlink(txtDeviceName, data.deviceName, 0.1f, 0.2f, true);
        DotweenHandler.ToBlink(txtSystem, data.system, 0.1f, 0.2f, true);
        DotweenHandler.ToBlink(txtManufacturer, data.manufacturer, 0.1f, 0.2f, true);
        DotweenHandler.ToBlink(txtModelNumber, data.modelNumber, 0.1f, 0.2f, true);

        Color tagColor = Color.white;
        switch (data.system)
        {
            case "DCR":
                tagColor = colorDCR;
                break;
            case "DCS":
                tagColor = colorDCS;
                break;
            case "DCN":
                tagColor = colorDCN;
                break;
        }
        imgSystemTag.DOColor(colorDCR, 0.1f).SetEase(Ease.OutQuad);
    }

    #region [Components]
    private TextMeshProUGUI _txtDeviceName { get; set; }
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= transform.Find("txtDeviceName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtModelNumber { get; set; }
    private TextMeshProUGUI txtModelNumber => _txtModelNumber ??= transform.Find("txtModelNumber").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtManufacturer { get; set; }
    private TextMeshProUGUI txtManufacturer => _txtManufacturer ??= transform.Find("txtManufacturer").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtSystem { get; set; }
    private TextMeshProUGUI txtSystem => _txtSystem ??= transform.Find("txtSystem").GetComponent<TextMeshProUGUI>();
    private Image _imgSystemTag { get; set; }
    private Image imgSystemTag => _imgSystemTag ??= transform.Find("imgSystemTag").GetComponent<Image>();
    #endregion
}
