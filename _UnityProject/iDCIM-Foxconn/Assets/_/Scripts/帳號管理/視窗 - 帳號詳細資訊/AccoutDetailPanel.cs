using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccoutDetailPanel : MonoBehaviour
{
    [Header(">>> 用戶資料")]
    [SerializeField] private Data_User data;
    [Header(">>> 點擊關閉按鈕時")]
    public UnityEvent<Data_User> onClickCloseBtn = new UnityEvent<Data_User>();

    [Header(">>> UI")]
    [SerializeField] private Image imgPhoto;
    [SerializeField] private TextMeshProUGUI txtUserName, txtAccount, txtRole, txtNetType, txtStatus;
    [SerializeField] private TextMeshProUGUI txtLastLoginDateTime, txtEditDateTime, txtCreateDateTime, txtSuspendDate;
    [SerializeField] private Button btnClose;
    [SerializeField] private DoTweenFadeController fadeController;

    public Data_User userData => data;

    public void ShowData(Data_User userData)
    {
        data = userData;
        UpdateUI();
        fadeController.FadeIn(true);
    }

    private void UpdateUI()
    {
        txtUserName.SetText(data.UserName);
        txtAccount.SetText(data.Account);
        txtRole.SetText(data.Role.ToString());
        txtNetType.SetText(data.NetType.ToString());
        txtNetType.SetText(data.Status.ToString());

        txtLastLoginDateTime.SetText(data.LastLoginDateTime.ToString(DateTimeFormatter.FullFormat));
        txtEditDateTime.SetText(data.EditDateTime.ToString(DateTimeFormatter.FullFormat));
        txtCreateDateTime.SetText(data.CreateDateTime.ToString(DateTimeFormatter.FullFormat));
        txtSuspendDate.SetText(data.SuspendDateTime);

        imgPhoto.sprite = data.UserPhoto;
    }

    private void Start()
    {
        btnClose.onClick.AddListener(() => onClickCloseBtn.Invoke(data));
        fadeController.OnFadeOutEvent.AddListener(() =>
        {
            ObjectPoolManager.PushToPool<AccoutDetailPanel>(this);
        });
    }

    public void Close()
    {
        fadeController.FadeOut();
    }
}
