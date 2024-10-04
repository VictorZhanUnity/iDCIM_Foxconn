using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListItem_AccessRecord : MonoBehaviour
{
    [Header(">>> 點擊項目時觸發")]
    public UnityEvent<ListItem_AccessRecord> onClickItemEvent = new UnityEvent<ListItem_AccessRecord>();

    [Header(">>> UI物件")]
    [SerializeField] private TextMeshProUGUI txtTimeStamp, txtUserName, txtRole;
    [SerializeField] private Image imgPhoto;
    [SerializeField] private Toggle toggle;

    public bool isOn { get => toggle.isOn;  set => toggle.isOn = value; }

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private Data_AccessRecord data;
    public Data_AccessRecord recordData
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
        txtTimeStamp.SetText(data.AccessTimeStamp.ToString(DateTimeFormatter.FullFormat));
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
