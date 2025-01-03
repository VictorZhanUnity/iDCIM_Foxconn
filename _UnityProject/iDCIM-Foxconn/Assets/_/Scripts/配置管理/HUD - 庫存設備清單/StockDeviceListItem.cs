using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.ColorUtils;
using static DeviceConfigure_DataHandler;

/// <summary>
/// [清單資料項] - 機房現有設備清單項目
/// </summary>
public class StockDeviceListItem : MonoBehaviour
{
    [Header(">>> [資料項] - StockDeviceSet")]
    [SerializeField] private StockDeviceSet _data;
    public StockDeviceSet data => _data;

    public Transform createTempDeviceModel { get; set; }

    /// <summary>
    /// 點擊該資料項時Invoke(On/Off)
    /// </summary>
    public UnityEvent<StockDeviceListItem> onSelectDeviceModel = new UnityEvent<StockDeviceListItem>();

    /// <summary>
    /// 當Drag產生暫時的設備時Invoke
    /// </summary>a
    public UnityEvent<StockDeviceListItem> onCreateTempDeviceModel = new UnityEvent<StockDeviceListItem>();

    /// <summary>
    /// 目前指向哪一個RackSpacer
    /// </summary>
    private List<RackSpacer> occupyRackSpacer { get; set; } = new List<RackSpacer>();
    /// <summary>
    /// 設備所佔目標機櫃名稱
    /// </summary>
    public string TargetRackName => occupyRackSpacer[0].parentRack.name;
    /// <summary>
    /// 設備所佔目標機櫃U層數
    /// </summary>
    public string OccupyRuSpacer
    {
        get
        {
            var data = occupyRackSpacer.OrderBy(rack => rack.RuLocation);
            int startIndex = data.First().RuLocation;
            int endIndex = data.Last().RuLocation;
            return (startIndex == endIndex) ? $"U{startIndex}" : $"U{startIndex} ~ U{endIndex}";
        }
    }

    /// <summary>
    /// 開/關 Toggle
    /// </summary>
    public bool isOn { get => toggle.isOn; set => toggle.isOn = value; }
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    /// <summary>
    /// 顯示資訊
    /// </summary>
    public void ShowData(StockDeviceSet data)
    {
        _data = data;
        txtDeviceName.SetText(data.deviceAsset.deviceName);
        txtWatt.SetText(data.deviceAsset.information.watt.ToString());
        txtWeight.SetText(data.deviceAsset.information.weight.ToString());
        txtHeightU.SetText(data.deviceAsset.information.heightU.ToString());

        name = txtDeviceName.text;
    }
    public void SetToggleWithoutNotify(bool isOn)
    {
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.isOn = isOn;
        dragController.enabled = isOn;
        ChangeColor(isOn);
        OnEnable();
    }

    private void ChangeColor(bool isOn)
    {
        Color targetColor = (isOn) ? Color.white : ColorHandler.HexToColor(0x999999);
        txtDeviceName.color = txtWatt.color = txtWeight.color = txtHeightU.color = targetColor;
        imgWatt.color = imgWeight.color = imgHeightU.color = targetColor;
        imgBorder.color = targetColor;
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            dragController.enabled = isOn;
            onSelectDeviceModel.Invoke(this);
            if (createTempDeviceModel != null) createTempDeviceModel.gameObject.SetActive(isOn);
            occupyRackSpacer.ForEach(rackSpacer => rackSpacer.isForceToShow = isOn);
            ChangeColor(isOn);
        });

        //dragController.onCreateTempDevice.AddListener(OnCreateTempDeviceHandler);
        dragController.enabled = false;
    }

    /// <summary>
    /// 當Drag產生暫時的設備時
    /// </summary>
    private void OnCreateTempDeviceHandler(StockDeviceListItem stockItem, RackSpacer rackSpacer)
    {
        CancellUploadDevice();

        Data_ServerRackAsset targetRack = rackSpacer.dataRack;
        occupyRackSpacer = targetRack.ShowRackSpacer(rackSpacer.RuLocation, stockItem.data.deviceAsset.information.heightU);

        //建立設備模型
        createTempDeviceModel ??= Instantiate(data.deviceAsset.model);
        createTempDeviceModel.transform.SetParent(rackSpacer.container, false);
        createTempDeviceModel.localPosition = Vector3.zero;
        createTempDeviceModel.localRotation = Quaternion.Euler(0, 90, 0);
        createTempDeviceModel.gameObject.SetActive(true);
        createTempDeviceModel.DOLocalMove(Vector3.zero, 0.3f).From(Vector3.left * 0.3f).SetEase(Ease.OutQuad).SetAutoKill(true);

        onCreateTempDeviceModel.Invoke(this);
    }


    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
        dragController.onCreateTempDevice.RemoveAllListeners();
        layoutElement.ignoreLayout = false;
        dragController.enabled = false;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 取消上傳設備
    /// </summary>
    public void CancellUploadDevice()
    {
        occupyRackSpacer.ForEach(rack => rack.isForceToShow = false);
        occupyRackSpacer.Clear();
        if (createTempDeviceModel != null)
        {
            createTempDeviceModel.gameObject.SetActive(false);
            createTempDeviceModel.transform.parent = null;
        }
    }

    /// <summary>
    /// 確認上傳設備
    /// </summary>
    public void ConfirmUploadDevice()
    {
        createTempDeviceModel.parent = occupyRackSpacer[0].transform.parent;
        occupyRackSpacer[0].dataRack.RemoveAvailableRackSpacer(occupyRackSpacer);
        occupyRackSpacer.Clear();

        layoutElement.ignoreLayout = true;
        rectTrans.DOLocalMoveX(-300, 0.2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                this.isOn = false;
                ObjectPoolManager.PushToPool(this);
            });
    }

    #region [Components]
    private DragAndDeploy _dragController { get; set; }
    private DragAndDeploy dragController => _dragController ??= GetComponent<DragAndDeploy>();

    private LayoutElement _layoutElement { get; set; }
    private LayoutElement layoutElement => _layoutElement ??= GetComponent<LayoutElement>();
    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    private TextMeshProUGUI _txtDeviceName, _txtWatt, _txtWeight, _txtHeightU;
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= transform.GetChild(0).Find("txtDeviceName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtWatt => _txtWatt ??= transform.GetChild(0).Find("HLayout").Find("txtWatt").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtWeight => _txtWeight ??= transform.GetChild(0).Find("HLayout").Find("txtWeight").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtHeightU => _txtHeightU ??= transform.GetChild(0).Find("HLayout").Find("txtHeightU").GetComponent<TextMeshProUGUI>();
    private Image _imgWatt, _imgWeight, _imgHeightU, _imgBorder;
    private Image imgWatt => _imgWatt ??= txtWatt.transform.Find("imgWatt").GetComponent<Image>();
    private Image imgWeight => _imgWeight ??= txtWeight.transform.Find("imgWeight").GetComponent<Image>();
    private Image imgHeightU => _imgHeightU ??= txtHeightU.transform.Find("imgHeightU").GetComponent<Image>();
    private Image imgBorder => _imgBorder ??= transform.GetChild(0).Find("imgBorder").GetComponent<Image>();

    private RectTransform _rectTrans { get; set; }
    private RectTransform rectTrans => _rectTrans ??= GetComponent<RectTransform>();
    #endregion

}