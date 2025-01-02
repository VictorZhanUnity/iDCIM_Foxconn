using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.RTSP;
using static VictorDev.Common.LogoManager;

public class LogoHandler : MonoBehaviour, ILogoReceiver
{
    #region [>>> Components]
    private Image _imgLogo { get; set; }
    private Image imgLogo => _imgLogo ??= GetComponent<Image>();
    private TextMeshProUGUI _txtCompany { get; set; }
    private TextMeshProUGUI txtCompany => _txtCompany ??= transform.Find("txtCompany").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtCompany_ENG { get; set; }
    private TextMeshProUGUI txtCompany_ENG => _txtCompany_ENG ??= transform.Find("txtCompany_ENG").GetComponent<TextMeshProUGUI>();
    #endregion

    public void Receive(SoData_Logo logo)
    {
        imgLogo.sprite = logo.logo;
        txtCompany.SetText(logo.componey);
        txtCompany_ENG.SetText(logo.componey_ENG);
    }
}
