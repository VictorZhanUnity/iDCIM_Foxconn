using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.DoTweenUtils;

/// <summary>
/// [配置管理] - 上架設備時的資訊輸入框
/// </summary>
public class Panel_StockDeviceUploadInfo : MonoBehaviour
{
    [Header(">>> [資料項] - 所選庫存設備")]
    [SerializeField] private StockDeviceListItem stockDeviceItem;

    [Header(">>> [Prefab] - 訊息通知")]
    [SerializeField] private NotifyListItem notifyPrefab;

    [Header(">>> 上架設備成功時Invoke")]
    public UnityEvent<StockDeviceListItem> onUploadDeviceComplete = new UnityEvent<StockDeviceListItem>();

    public void ShowData(StockDeviceListItem item)
    {
        if (stockDeviceItem != item)
        {
            CancellUploadDevice();
            stockDeviceItem = item;
        }
        UpdateUI();
        gameObject.SetActive(false);
        dotweenFade.ToShow();
    }

    private void UpdateUI()
    {
        txtUploadDevice.SetText(stockDeviceItem.data.deviceAsset.deviceName);
        txtTargetRack.SetText(stockDeviceItem.TargetRackName);
        txtOccupyRU.SetText(stockDeviceItem.OccupyRuSpacer);
    }
    public void ToClose()
    {
        CancellUploadDevice();
        dotweenFade.ToHide();
    }

    private void CancellUploadDevice()
    {
        stockDeviceItem?.CancellUploadDevice();
        stockDeviceItem = null;
    }

    /// <summary>
    /// 上架設備
    /// </summary>
    private void ToUploadDevice()
    {
        loadingScreen.gameObject.SetActive(true);

        //呼叫WebAPI Singleton進行上傳資料
        //
        //
        //

        #region Demo
        IEnumerator LoadingScreen()
        {
            int counter = 0;
            while (counter++ < 2)
            {
                yield return new WaitForSeconds(1);
            }
            OnUploadDeviceSuccess();
        }
        StartCoroutine(LoadingScreen());
        #endregion
    }

    private void OnUploadDeviceSuccess()
    {
        dotweenFade.ToHide();
        stockDeviceItem.ConfirmUploadDevice();
        onUploadDeviceComplete.Invoke(stockDeviceItem);
        NotificationManager.CreateNotifyMessage(notifyPrefab, "設備上架成功!!", stockDeviceItem.data.deviceAsset);
    }

    private void OnEnable()
    {
        btnSend.onClick.AddListener(ToUploadDevice);
        btnCancel.onClick.AddListener(ToClose);
        dotweenFade.OnHideEvent.AddListener(OnDisable);

        inputIdNumber.text = inputDeviceName.text = inputIPAddress.text = inputNote.text = string.Empty;
        loadingScreen.gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        btnSend.onClick.RemoveAllListeners();
        btnCancel.onClick.RemoveAllListeners();
        dotweenFade.OnHideEvent.RemoveAllListeners();
    }
    #region [>>> Components]

    private GameObject _loadingScreen { get; set; }
    private GameObject loadingScreen => _loadingScreen ??= transform.Find("LoadingScreen").gameObject;

    private DoTweenFadeController _dotweenFade { get; set; }
    private DoTweenFadeController dotweenFade => _dotweenFade ??= GetComponent<DoTweenFadeController>();

    private Transform _vlayoutTxt { get; set; }
    private Transform vlayoutTxt => _vlayoutTxt ??= transform.Find("Container").GetChild(0);

    private TextMeshProUGUI _txtUploadDevice { get; set; }
    private TextMeshProUGUI txtUploadDevice => _txtUploadDevice ??= vlayoutTxt.Find("txt上架設備").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTargetRack { get; set; }
    private TextMeshProUGUI txtTargetRack => _txtTargetRack ??= vlayoutTxt.Find("txt目標機櫃").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtOccupyRU { get; set; }
    private TextMeshProUGUI txtOccupyRU => _txtOccupyRU ??= vlayoutTxt.Find("txt所佔U位").GetComponent<TextMeshProUGUI>();

    private Transform _vlayoutInput { get; set; }
    private Transform vlayoutInput => _vlayoutInput ??= transform.Find("Container").GetChild(1);

    private TMP_InputField _inputIdNumber { get; set; }
    private TMP_InputField inputIdNumber => _inputIdNumber ??= vlayoutInput.Find("input編號").GetComponent<TMP_InputField>();
    private TMP_InputField _inputDeviceName { get; set; }
    private TMP_InputField inputDeviceName => _inputDeviceName ??= vlayoutInput.Find("input名稱").GetComponent<TMP_InputField>();
    private TMP_InputField _inputIPAddress { get; set; }
    private TMP_InputField inputIPAddress => _inputIPAddress ??= vlayoutInput.Find("inputIP位址").GetComponent<TMP_InputField>();
    private TMP_InputField _inputNote { get; set; }
    private TMP_InputField inputNote => _inputNote ??= vlayoutInput.Find("input備註").GetComponent<TMP_InputField>();

    private Button _btnSend { get; set; }
    private Button btnSend => _btnSend ??= transform.Find("Container").Find("Button上架").GetComponent<Button>();
    private Button _btnCancell { get; set; }
    private Button btnCancel => _btnCancell ??= transform.Find("Container").Find("Button取消").GetComponent<Button>();

    #endregion
}
