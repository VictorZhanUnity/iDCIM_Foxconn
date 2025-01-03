using System;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Common
{
    /// <summary>
    /// [管理器] - 訊息通知管理器
    /// </summary>
    public class NotificationManager : SingletonMonoBehaviour<NotificationManager>
    {
        /// <summary>
        /// 建立訊息通知 {UI樣式組件, 標題文字, 所攜帶的資料項, 點選項目時行為, 點選關閉鈕時行為}
        /// </summary>
        public static T CreateNotifyMessage<T>(T itemPrefab, Action<NotifyListItem> onClickItem = null, Action onClose = null) where T: NotifyListItem
        {
            T notifyItem = ObjectPoolManager.GetInstanceFromQueuePool(itemPrefab, Instance.scrollRect.content);
            Instance.scrollRect.verticalNormalizedPosition = 1;

            void OnCloseHandler(NotifyListItem target)
            {
                ObjectPoolManager.PushToPool(target);
                onClose?.Invoke();
                target.onCloseEvent.RemoveAllListeners();
            }
            notifyItem.onCloseEvent.AddListener(OnCloseHandler);
            return notifyItem;
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                CreateNotifyMessage(defaultItemPrefab);
            }


            rayCaster.enabled = scrollRect.content.childCount > 0;
        }

        #region [Componenets]
        public NotifyListItem defaultItemPrefab;

        private ScrollRect _scrollRect { get; set; }
        private ScrollRect scrollRect => _scrollRect ??= transform.GetChild(0).GetComponent<ScrollRect>();

        private GraphicRaycaster _rayCaster { get; set; }
        private GraphicRaycaster rayCaster => _rayCaster ??= transform.GetComponent<GraphicRaycaster>();
        #endregion
    }
}
