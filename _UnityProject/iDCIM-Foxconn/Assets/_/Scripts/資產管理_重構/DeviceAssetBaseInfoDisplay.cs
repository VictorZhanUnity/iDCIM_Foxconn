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

    private Color colorDCR = ColorHandler.HexToColor(0x0DA433);
    private Color colorDCN = ColorHandler.HexToColor(0xFE993E);
    private Color colorDCS = ColorHandler.HexToColor(0xB74F43);

    public void ShowData(Data_iDCIMAsset data)
    {
        this.data = data;
        UpdateUI();
    }

    private void DotweenText(TextMeshProUGUI targetTxt, string txt)
    {
        bool isTxtNull = string.IsNullOrEmpty(txt);
        string strValue = isTxtNull ? "--- EMPTY ---" : txt;
        Color color = isTxtNull ? ColorHandler.HexToColor(0x555555) : Color.white;
        float delay = Random.Range(0f, 0.2f);
        DotweenHandler.ToBlink(targetTxt, strValue, 0.2f, delay);
        targetTxt.DOColor(color, 0.1f).SetDelay(delay).OnComplete(() => targetTxt.fontStyle = isTxtNull ? FontStyles.Italic : FontStyles.Normal);
    }

    private void UpdateUI()
    {
        DotweenText(txtDeviceName, data.deviceName);
        DotweenText(txtSystem, data.system);
        DotweenText(txtManufacturer, data.information.type_manufacturer);
        DotweenText(txtModelNumber, data.information.type_modelNumber);

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
        imgSystemTag.DOColor(tagColor, 0.1f).SetEase(Ease.OutQuad);
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
