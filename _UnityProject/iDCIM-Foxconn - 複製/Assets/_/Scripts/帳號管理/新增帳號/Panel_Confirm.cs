using TMPro;
using UnityEngine;
using VictorDev.Common;

public class Panel_Confirm : MonoBehaviour
{
    [SerializeField] private DoTweenFadeController fadeController;
    [SerializeField] private TextMeshProUGUI txtAccount, txtEmail, txtRole, txtCreateDateTime;

    public void ShowData(Data_User data)
    {
        fadeController.FadeIn(true);
        txtAccount.SetText(data.Account);
        txtEmail.SetText(data.EMail);
        txtRole.SetText(data.Role.ToString());
        txtCreateDateTime.SetText(data.CreateDateTime.ToString(DateTimeHandler.FullDateTimeMinuteFormat));
        fadeController.FadeIn();
    }

    public void Close() => fadeController.FadeOut();
}
