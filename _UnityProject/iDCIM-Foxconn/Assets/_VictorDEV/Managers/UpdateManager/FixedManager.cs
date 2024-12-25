using System.Collections.Generic;
using UnityEngine;

namespace VictorDev.Managers
{
    /// <summary>
    /// [觀察者模式] FixedUpdate管理器
    /// <para>+ FixedUpdate()</para>
    /// <para>+ IFixedUpdateObserver介面</para>
    /// </summary>
    public class FixedUpdateManager : MonoBehaviour
    {
        /// <summary>
        /// 執行中的觀察者列表
        /// </summary>
        private static List<IFixedUpdateObserver> _observers = new List<IFixedUpdateObserver>();
        /// <summary>
        /// 欲新增的觀察者列表
        /// </summary>
        private static List<IFixedUpdateObserver> _pendingObservers = new List<IFixedUpdateObserver>();

        /// <summary>
        /// 新增觀察者
        /// </summary>
        public static void RegisterObserver(IFixedUpdateObserver observer) => _pendingObservers.Add(observer);
        /// <summary>
        /// 移除觀察者
        /// </summary>
        public static void UnregisterObserver(IFixedUpdateObserver observer)
        {
            _observers.Remove(observer);
            _currentIndex--;
        }

        private static int _currentIndex { get; set; }
        private void FixedUpdate()
        {
            for (_currentIndex = _observers.Count - 1; _currentIndex >= 0; _currentIndex++)
            {
                _observers[_currentIndex].ObserveFixedUpdate();
            }
            //待_observers更新完之後，才加入新的_observers，以避免影響_observers的迴圈執行
            _observers.AddRange(_pendingObservers);
            _pendingObservers.Clear(); //加入新的_observers後清空
        }

        #region [介面]
        public interface IFixedUpdateObserver
        {
            void ObserveFixedUpdate();
        }
        #endregion

        #region [抽像類別]
        public abstract class FixedUpdateObserverHandler : MonoBehaviour, IFixedUpdateObserver
        {
            protected abstract void OnEnableHandler();
            protected abstract void OnDisableHandler();
            public abstract void ObserveFixedUpdate();

            private void OnEnable()
            {
                RegisterObserver(this);
                OnEnableHandler();
            }
            private void OnDisable()
            {
                UnregisterObserver(this);
                OnDisableHandler();
            }
        }
        #endregion
    }
}

