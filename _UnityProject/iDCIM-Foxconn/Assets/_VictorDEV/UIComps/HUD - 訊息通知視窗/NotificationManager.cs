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
        public static void ShowMessage(string title, string msg, Action onClick = null)
        {
            NotifyListItemTextMessage item = Instantiate(Instance.defaultItemPrefab, Instance.ScrollRect.content);
            item.ShowMessage(title, msg, onClick);
        }
        
        /// 建立訊息通知 {UI樣式組件, 標題文字, 所攜帶的資料項, 點選項目時行為, 點選關閉鈕時行為}
        public static T CreateNotifyMessage<T>(T itemPrefab, Action<NotifyListItem> onClickItem = null, Action onClose = null) where T: NotifyListItem
        {
            //T notifyItem = ObjectPoolManager.GetInstanceFromQueuePool(itemPrefab, Instance.scrollRect.content);
            T notifyItem = Instantiate(itemPrefab, Instance.ScrollRect.content);
            Instance.ScrollRect.verticalNormalizedPosition = 1;

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


            RayCaster.enabled = ScrollRect.content.childCount > 0;
        }

      
        
        #region [Componenets]
        [Header("[Prefab] 列表資料項")]
        public NotifyListItemTextMessage defaultItemPrefab;
        private GraphicRaycaster RayCaster => _rayCaster ??= transform.GetComponent<GraphicRaycaster>();
        private GraphicRaycaster _rayCaster;
        private ScrollRect ScrollRect => _scrollRect ??= transform.GetChild(0).GetComponent<ScrollRect>();
        private ScrollRect _scrollRect;
        #endregion
    }
}
