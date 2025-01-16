using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// [清單資料項] - 機房現有設備清單
/// </summary>
public class ListItem_Device_RE : MonoBehaviour
{
    //Demo
    public Transform createTempDeviceModel { get; set; }
    public List<RackSpacer> occupyRackSpacers = new List<RackSpacer>();
    public string OccupyRuSpacerString
    {
        get
        {
            var data = occupyRackSpacers.OrderBy(rack => rack.RuLocation);
            int startIndex = data.First().RuLocation;
            int endIndex = data.Last().RuLocation;
            return (startIndex == endIndex) ? $"U{startIndex}" : $"U{startIndex} ~ U{endIndex}";
        }
    }

    /// <summary>
    /// 取消上傳設備
    /// </summary>
    public void CancellUploadDevice()
    {
        occupyRackSpacers.ForEach(rack => rack.isForceToShow = false);
        occupyRackSpacers.Clear();
        if (createTempDeviceModel != null)
        {
            createTempDeviceModel.gameObject.SetActive(false);
            createTempDeviceModel.transform.parent = null;
        }
    }
    /// <summary>
    /// 確認上傳設備，將設備GameObject移至機櫃GameObject之下
    /// </summary>
    public void ConfirmUploadDevice()
    {
        createTempDeviceModel.parent = occupyRackSpacers[0].transform.parent;
        occupyRackSpacers[0].dataRack.RemoveAvailableRackSpacer(occupyRackSpacers);
        occupyRackSpacers.Clear();

        layoutElement.ignoreLayout = true;
        rectTrans.DOLocalMoveX(-300, 0.2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                this.isOn = false;
                ObjectPoolManager.PushToPool(this);
            });
    }


    public Data_ServerRackAsset targetRack;

    [Header(">>> [資料項]")]
    [SerializeField] private Data_iDCIMAsset _data;

    public ListItem_Device_RE(Data_iDCIMAsset data)
    {
        _data = data;
    }

    public Data_iDCIMAsset data => _data;

    /// <summary>
    /// 點擊該資料項時Invoke
    /// </summary>
    public UnityEvent<ListItem_Device_RE> onClickItemEvent = new UnityEvent<ListItem_Device_RE>();

    /// <summary>
    /// 開/關 Toggle
    /// </summary>
    public bool isOn { get => toggle.isOn; set => toggle.isOn = value; }
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public void ShowData(Data_iDCIMAsset data)
    {
        _data = data;
        UpdateUI(data);
    }

    private void UpdateUI(Data_iDCIMAsset data)
    {
        txtDeviceName.SetText(data.deviceName);
        txtWatt.SetText(data.information.watt.ToString());
        txtWeight.SetText(data.information.weight.ToString());
        txtHeightU.SetText(data.information.heightU.ToString());
    }

    public void SetToggleWithoutNotify(bool isOn)
    {
        OnDisable();
        toggle.isOn = isOn;
        OnEnable();
    }

    #region [Event Listener]
    private void OnEnable() => InitListener();
    private void OnDisable() => RemoveListener();
    private void InitListener()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (transform.TryGetComponent(out DragAndDeploy dragAndDeploy))
            {
                dragAndDeploy.enabled = isOn;
            }
            onClickItemEvent.Invoke(this);
        });
    }
    private void RemoveListener() => toggle.onValueChanged.RemoveAllListeners();
    #endregion

    #region [Components]
    private LayoutElement _layoutElement { get; set; }
    private LayoutElement layoutElement
    {
        get
        {
            if (_layoutElement == null && transform.TryGetComponent(out LayoutElement target) == false)
            {
                _layoutElement = transform.AddComponent<LayoutElement>();
            }
            return _layoutElement;
        }
    }
    private RectTransform _rectTrans { get; set; }
    private RectTransform rectTrans => _rectTrans ??= GetComponent<RectTransform>();

    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    private Transform _hLayout { get; set; }
    private Transform hLayout => _hLayout ??= transform.Find("Container").Find("HLayout");
    private TextMeshProUGUI _txtDeviceName { get; set; }
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= hLayout.parent.Find("txtDeviceName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtWatt { get; set; }
    private TextMeshProUGUI txtWatt => _txtWatt ??= hLayout.Find("txtWatt").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtWeight { get; set; }
    private TextMeshProUGUI txtWeight => _txtWeight ??= hLayout.Find("txtWeight").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtHeightU { get; set; }
    private TextMeshProUGUI txtHeightU => _txtHeightU ??= hLayout.Find("txtHeightU").GetComponent<TextMeshProUGUI>();
    #endregion
}