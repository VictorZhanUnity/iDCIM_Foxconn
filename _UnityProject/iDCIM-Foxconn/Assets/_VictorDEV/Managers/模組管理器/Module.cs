using System;
using UnityEngine;

namespace VictorDev.Managers
{
    /// <summary>
    /// [生命週期] - 功能模組Template
    /// </summary>
    public abstract class Module : MonoBehaviour, IModule
    {
        public abstract void OnInit(Action onInitComplete = null);
    }

    public interface IModule
    {
        /// <summary>
        /// 初始化，結束後執行onInitComplete
        /// </summary>
        abstract void OnInit(Action onInitComplete = null);
    }
    public interface IJsonParseReceiver
    {
        void ParseJson(string jsonData);
    }

    public interface IPage
    {
        bool IsOn { get; set; }
        /// <summary>
        /// 顯示頁面
        /// </summary>
        abstract void ToShow();
        /// <summary>
        /// 隱藏頁面
        /// </summary>
        abstract void ToHide();
        /// <summary>
        /// 新增監聽事件
        /// </summary>
        abstract void InitEventListener();
        /// <summary>
        /// 移除監聽事件
        /// </summary>
        abstract void RemoveEventListener();
    }
}
