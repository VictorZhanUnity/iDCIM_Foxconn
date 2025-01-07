using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

public class ListItem_AccessRecord : MonoBehaviour
{
    [Header(">>> �I�����خ�Ĳ�o")]
    public UnityEvent<ListItem_AccessRecord> onClickItemEvent = new UnityEvent<ListItem_AccessRecord>();

    [Header(">>> UI����")]
    [SerializeField] private TextMeshProUGUI txtTimeStamp, txtUserName, txtRole;
    [SerializeField] private Image imgPhoto;
    [SerializeField] private Toggle toggle;

    public bool isOn { get => toggle.isOn;  set => toggle.isOn = value; }

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private Data_AccessRecord_OLD data;
    public Data_AccessRecord_OLD recordData
    {
        get => data;
        set
        {
            data = value;
            UpdateUI();
        }
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) => onClickItemEvent.Invoke(this));
    }

    private void UpdateUI()
    {
        imgPhoto.sprite = data.UserPhoto;
        txtTimeStamp.SetText(data.AccessTimeStamp.ToString(DateTimeHandler.FullDateTimeFormat));
        txtUserName.SetText(data.UserName);
        txtRole.SetText(data.Role.ToString());
    }


    private void OnValidate()
    {
        toggle ??= GetComponent<Toggle>();
        imgPhoto ??= transform.GetChild(0).Find("imgPhoto").GetComponent<Image>();
        txtTimeStamp ??= transform.Find("txtTimeStamp").GetComponent<TextMeshProUGUI>();
        txtUserName ??= transform.Find("txtUserName").GetComponent<TextMeshProUGUI>();
        txtRole ??= transform.Find("txtRole").GetComponent<TextMeshProUGUI>();
    }
}
