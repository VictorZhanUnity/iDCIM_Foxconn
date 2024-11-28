using System;
using UnityEngine.UI;

namespace VictorDev.Common
{
    /// <summary>
    /// [管理器] - 訊息通知管理器
    /// </summary>
    public class NotificationManager : SingletonMonoBehaviour<NotificationManager>
    {
        private ScrollRect _scrollRect { get; set; }
        private ScrollRect scrollRect => _scrollRect ??= transform.GetChild(0).GetComponent<ScrollRect>();

        private GraphicRaycaster _rayCaster { get; set; }
        private GraphicRaycaster rayCaster => _rayCaster ??= transform.GetComponent<GraphicRaycaster>();

        /// <summary>
        /// 建立訊息通知 {UI樣式組件, 標題文字, 所攜帶的資料項, 點選項目時行為, 點選關閉鈕時行為}
        /// </summary>
        public static void CreateNotifyMessage(NotifyListItem itemPrefab, string title, INotifyData data, Action<INotifyData> onClickItem = null, Action onClose = null)
        {
            NotifyListItem notifyItem = ObjectPoolManager.GetInstanceFromQueuePool(itemPrefab, Instance.scrollRect.content);
            notifyItem.ShowData(title, data);
            Instance.scrollRect.verticalNormalizedPosition = 1;

            void OnCloseHandler(NotifyListItem target)
            {
                ObjectPoolManager.PushToPool(target);
                onClose?.Invoke();
                target.onCloseEvent.RemoveAllListeners();
                target.onClickEvent.RemoveAllListeners();
            }
            notifyItem.onCloseEvent.AddListener(OnCloseHandler);
            if (onClickItem != null) notifyItem.onClickEvent.AddListener(onClickItem.Invoke);
        }

        private void LateUpdate()
        {
            rayCaster.enabled = scrollRect.content.childCount > 0;
        }
    }
}
