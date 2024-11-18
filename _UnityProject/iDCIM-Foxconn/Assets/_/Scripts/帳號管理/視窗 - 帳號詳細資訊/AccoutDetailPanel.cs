using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.DoTweenUtils;

public class AccoutDetailPanel : MonoBehaviour
{
    [Header(">>> 用戶資料")]
    [SerializeField] private Data_User data;
    [Header(">>> 點擊關閉按鈕時")]
    public UnityEvent<Data_User> onClickCloseBtn = new UnityEvent<Data_User>();

    [Header(">>> UI")]
    [SerializeField] private Image imgPhoto, imgStatus;
    [SerializeField] private TextMeshProUGUI txtUserName, txtAccount, txtRole, txtNetType, txtStatus;
    [SerializeField] private TextMeshProUGUI txtLastLoginDateTime, txtEditDateTime, txtCreateDateTime, txtSuspendDate;
    [SerializeField] private Button btnClose;
    [SerializeField] private DoTweenFadeController fadeController;
    [SerializeField] private Color colorActivate, colorForbbiden;

    public Data_User userData => data;

    public void ShowData(Data_User userData)
    {
        data = userData;
        UpdateUI();
        fadeController.ToShow(true);
    }

    private void UpdateUI()
    {
        txtUserName.SetText(data.UserName);
        txtAccount.SetText(data.Account);
        txtRole.SetText(data.Role.ToString());
        txtNetType.SetText(data.NetType.ToString());
        txtStatus.SetText(data.Status.ToString());
        imgStatus.color = data.Status == Config_Enum.enumAccountStatus.啟用 ? colorActivate : colorForbbiden;

        if (string.IsNullOrEmpty(data.GetValue("LastLoginDateTime")))
        {
            txtLastLoginDateTime.SetText("尚未登入");
        }
        else
        {
            txtLastLoginDateTime.SetText(data.LastLoginDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
        }

        txtEditDateTime.SetText(data.EditDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
        txtCreateDateTime.SetText(data.CreateDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
        txtSuspendDate.SetText(data.SuspendDateTime);

        imgPhoto.sprite = data.UserPhoto;
    }

    private void Start()
    {
        btnClose.onClick.AddListener(() => onClickCloseBtn.Invoke(data));
        fadeController.OnHideEvent.AddListener(() =>
        {
            ObjectPoolManager.PushToPool<AccoutDetailPanel>(this);
        });
    }

    public void Close()
    {
        fadeController.ToHide();
    }
}
