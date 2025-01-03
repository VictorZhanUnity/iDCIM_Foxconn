using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace VictorDev.Common
{
    /// <summary>
    /// [HUD - 訊息通知視窗] - 訊息列表項目-預設
    /// </summary>
    public class NotifyListItem_TextMessage : NotifyListItem
    {
        /// <summary>
        /// 顯示訊息
        /// </summary>
        public void ShowMessage(string title, string msg, Sprite icon = null, Action onClick=null)
        {
            txtTitle.SetText(title.Trim());
            txtMsg.SetText(msg.Trim());
            if (icon != null) imgICON.sprite = icon;
            if (onClick != null)
            {
                buttonRow.onClick.AddListener(onClick.Invoke);
            }
            buttonRow.interactable = onClick != null;
        }

        #region [>>> Components]
        private Image _imgICON { get; set; }
        private Image imgICON => _imgICON ??= container.Find("imgICON").GetComponent<Image>();
        private TextMeshProUGUI txtTitle => _txtTitle ??= container.Find("txtTitle").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtTitle;
        private TextMeshProUGUI txtMsg => _txtMsg ??= container.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtMsg;
        #endregion
    }
}