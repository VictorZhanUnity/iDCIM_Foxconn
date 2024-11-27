using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;
using static DeviceConfigure_DataHandler;

/// <summary>
/// [配置管理] - 上架設備時的資訊輸入框
/// </summary>
public class Panel_StockDeviceUploadInfo : MonoBehaviour
{
    [Header(">>> [資料項] - 所選庫存設備")]
    [SerializeField] private StockDeviceListItem item;
    [Header(">>> [資料項] - 所選目標機櫃U層")]
    [SerializeField] private RackSpacer rackSpacer;

    [Header(">>> [Event] - 當點擊上架設備按鈕時Invoke")]
    public UnityEvent<StockDeviceSet> onClickUploadDevice = new UnityEvent<StockDeviceSet>();

    [Header(">>> [Event] - 當上架設備完成時Invoke")]
    public UnityEvent onUploadDeviceCompleteEvent = new UnityEvent();

    [Header(">>> [Event] - 當取消上架設備時Invoke")]
    public UnityEvent onClickCancellUpload = new UnityEvent();

    public void ShowData(StockDeviceListItem item, RackSpacer rackSpacer)
    {
        Debug.Log($"Panel_StockDeviceUploadInfo: {item.data.modelNumber} / {rackSpacer.rackTarget.name} / {rackSpacer.RuIndex}");

        this.item = item;
        this.rackSpacer = rackSpacer;

        UpdateUI();
        gameObject.SetActive(false);
        dotweenFade.ToShow();
    }
    public void ToClose()
    {
        onClickCancellUpload.Invoke();
        dotweenFade.ToHide();
    }

    private void UpdateUI()
    {
        txtUploadDevice.SetText(item.data.deviceAsset.deviceName);
        txtTargetRack.SetText(rackSpacer.rackTarget.name);
        txtOccupyRU.SetText($"U{rackSpacer.RuIndex}");
    }

    private void OnEnable()
    {
        inputIdNumber.text = inputDeviceName.text = inputIPAddress.text = inputNote.text = string.Empty;
        btnSend.onClick.AddListener(ToUploadDevice);
        btnCancell.onClick.AddListener(ToClose);
        dotweenFade.OnHideEvent.AddListener(OnDisable);
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
            dotweenFade.ToHide();
            onClickUploadDevice.Invoke(item.data);
        }
        StartCoroutine(LoadingScreen());
        #endregion
    }

    private void OnDisable()
    {
        inputIdNumber.text = inputDeviceName.text = inputIPAddress.text = inputNote.text = string.Empty;
        btnSend.onClick.RemoveAllListeners();
        dotweenFade.OnHideEvent.RemoveAllListeners();
        loadingScreen.gameObject.SetActive(false);
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
    private Button btnCancell => _btnCancell ??= transform.Find("Container").Find("Button取消").GetComponent<Button>();

    #endregion
}
