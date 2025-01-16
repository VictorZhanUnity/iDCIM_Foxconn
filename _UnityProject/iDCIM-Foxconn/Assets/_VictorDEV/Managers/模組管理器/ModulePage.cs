using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Managers;

/// <summary>
/// [iDCIM] 所有功能模組之父類別
/// <para>+ 處理依據關鍵字搜尋卻顯示之物件</para>
/// <para>+ 處理欲顯示之物件</para>
/// </summary>
public abstract class ModulePage : Module
{
    [Header(">>> 欲顯示的模型物件")]
    [SerializeField] private ModelDisplayConfiguration modelForDisplay;
    /// <summary>
    /// 欲顯示的模型物件
    /// </summary>
    public List<Transform> modelList => modelForDisplay.modelsList;

    [Header(">>> 頁面物件")]
    [SerializeField] protected GameObject content;

    private void Awake() => IsOn = false;

    public bool IsOn
    {
        get => content.activeSelf;
        set
        {
            if (value) ToShow();
            else ToHide();
        }
    }

    private void ToShow()
    {
        MaterialHandler.ReplaceMaterialWithExclude(modelForDisplay.modelsList.ToHashSet());
        OnShowHandler();
        InitEventListener();
        content.SetActive(true);
    }
    private void ToHide()
    {
        MaterialHandler.RestoreOriginalMaterials();
        OnCloseHandler();
        RemoveEventListener();
        if(content!=null)content.SetActive(false);
    }

    /// <summary>
    /// 當顯示UI時
    /// </summary>
    protected abstract void OnShowHandler();
    /// <summary>
    /// 當隱藏UI時
    /// </summary>
    protected abstract void OnCloseHandler();
    /// <summary>
    /// 啟動監聽
    /// </summary>
    protected abstract void InitEventListener();
    /// <summary>
    /// 移除監聽
    /// </summary>
    protected abstract void RemoveEventListener();

    #region [ContextMenu]
    [ContextMenu("- [Parent] 根據Keywords尋找目標物件")]
    public void FindTargetObjects() => modelForDisplay.FindTargetObjects();
    [ContextMenu("- [Parent] 新增BoxCollider到目標物件")]
    public void AddColliderToObjects() => modelForDisplay.AddColliderToObjects();
    [ContextMenu("- [Parent] 移除目標物件的Collider")]
    public void RemoveColliderFromObjects() => modelForDisplay.RemoveColliderFromObjects();
    #endregion
}
