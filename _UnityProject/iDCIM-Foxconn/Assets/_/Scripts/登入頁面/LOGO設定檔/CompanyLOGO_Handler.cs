using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.RTSP;

public class CompanyLOGO_Handler : MonoBehaviour
{
    [Header(">>> [ScriptableObject] 公司LOGO設定, 以第一個資料項做為顯示")]
    public List<SoData_Logo> logoList;


    [ContextMenu("- OnValidate")]
    private void OnValidate()
    {
        if (logoList.Count > 0 && logoList[0] != null)
        {
            SoData_Logo target = logoList[0];
            imgLogo.sprite = target.logo;
            txtCompany.SetText(target.componey);
            txtCompany_ENG.SetText(target.componey_ENG);
        }
    }


    #region [>>> Components]
    private Image _imgLogo { get; set; }
    private Image imgLogo => _imgLogo ??= GetComponent<Image>();
    private TextMeshProUGUI _txtCompany { get; set; }
    private TextMeshProUGUI txtCompany => _txtCompany ??= transform.Find("txtCompany").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtCompany_ENG { get; set; }
    private TextMeshProUGUI txtCompany_ENG => _txtCompany_ENG ??= transform.Find("txtCompany_ENG").GetComponent<TextMeshProUGUI>();
    #endregion
}
