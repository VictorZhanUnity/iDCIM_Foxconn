using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Common
{
    /// [HUD - 訊息通知視窗] - 訊息列表項目-預設
    public class NotifyListItemTextMessage : NotifyListItem
    {
        /// 顯示訊息
        public void ShowMessage(string title, string msg, Action onClick = null)
        {
            TxtTitle.SetText(title.Trim());
            TxtMsg.SetText(msg.Trim());
            if (onClick != null)
            {
                ButtonRow.onClick.AddListener(onClick.Invoke);
            }

            ButtonRow.interactable = onClick != null;
        }

        public Sprite Icon
        {
            set => ImgIcon.sprite = value;
        }

        #region Components

        private Image ImgIcon => _imgIcon ??= Container.Find("imgICON").GetComponent<Image>();
        private Image _imgIcon;
        private TextMeshProUGUI TxtTitle => _txtTitle ??= Container.Find("txtTitle").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtTitle;
        private TextMeshProUGUI TxtMsg => _txtMsg ??= Container.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtMsg;

        #endregion
    }
}