using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Advanced;

/// CCTV放大畫面視窗
public class CCTVFullScreenPlayer : PopUpWindow
{
    public void Show(CCTV_InfoPanel panel)
    {
        _cctvPanel = panel;
        UpdateUI();
    }

    private void UpdateUI()
    {
        TxtTitle.SetText(_cctvPanel.data.name);
        _cctvPanel.RtspScreen.AddRenderingTarget(RawImg.gameObject);
        gameObject.SetActive(true);
        RawImg.gameObject.SetActive(true);
        ToShow();
    }
    #region [Initialize]
    private void OnEnable() => BtnClose.onClick.AddListener(ToClose);
    private void OnDisable()
    {
        RawImg.gameObject.SetActive(false);
        BtnClose.onClick.RemoveListener(ToClose);
        _cctvPanel.RtspScreen.RemoveRenderingTarget(RawImg.gameObject);
    }
    #endregion
    
    #region [Components]
    [Header("[資料項]")]
    private CCTV_InfoPanel _cctvPanel;
    private RawImage RawImg => _rawImg ??= transform.Find("imgBkg").Find("rawImg").GetComponent<RawImage>();
    private RawImage _rawImg;
    private TextMeshProUGUI TxtTitle => _txtTitle ??= transform.Find("txtTitle").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTitle;
    private Button BtnClose => _btnClose ??= transform.Find("Button關閉").GetComponent<Button>();
    private Button _btnClose;
    #endregion
}

