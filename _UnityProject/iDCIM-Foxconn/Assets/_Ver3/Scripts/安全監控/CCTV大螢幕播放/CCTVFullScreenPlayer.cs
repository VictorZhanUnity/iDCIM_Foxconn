using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        RawImg.gameObject.SetActive(true);
        ToShow();
    }

    #region [Initialize]
    private void OnEnable() => BtnClose.onClick.AddListener(ToClose);
    private void OnDisable()
    {
        RawImg.gameObject.SetActive(false);
        BtnClose.onClick.RemoveListener(ToClose);
        _cctvPanel?.RtspScreen.RemoveRenderingTarget(RawImg.gameObject);
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

public abstract class PopUpWindow : MonoBehaviour
{
    protected void ToShow() => BlackScreen.gameObject.SetActive(true);

    public void ToClose()
    {
        gameObject.SetActive(false);
        bool hasActiveChild = transform.parent.transform.Cast<Transform>()
            .Any(child => child.gameObject.activeSelf && child.gameObject != BlackScreen);
        
        if(hasActiveChild == false) BlackScreen.gameObject.SetActive(false);
    }
    private GameObject BlackScreen => _blackScreen ??= transform.parent.Find("BlackScreen").gameObject;
    private GameObject _blackScreen;
}
