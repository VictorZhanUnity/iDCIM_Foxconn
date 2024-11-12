using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// [iDCIM] 所有功能模組之父類別
/// <para>+ 處理依據關鍵字搜尋卻顯示之物件</para>
/// <para>+ 處理欲顯示之物件</para>
/// </summary>
public abstract class iDCIM_ModuleManager : MonoBehaviour
{
    [Header(">>> 欲顯示的目標物件")]
    [SerializeField] ModelDisplayConfiguration modelForDisplay;

    /// <summary>
    /// 欲顯示的物件
    /// </summary>
    protected List<Transform> modelList => modelForDisplay.modelsList;

    private bool _isOn { get; set; }

    public bool isOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            transform.GetChild(0).gameObject.SetActive(value);
            if (value) ToShow();
            else ToClose();
        }
    }

    private void ToShow()
    {
        MaterialHandler.ReplaceMaterialWithExclude(modelForDisplay.modelsList.ToHashSet());
        OnShowHandler();
    }
    private void ToClose()
    {
        MaterialHandler.RestoreOriginalMaterials();
        OnCloseHandler();
    }

    /// <summary>
    /// 當顯示UI時
    /// </summary>
    protected abstract void OnShowHandler();
    /// <summary>
    /// 當隱藏UI時
    /// </summary>
    protected abstract void OnCloseHandler();

    [ContextMenu("- [Parent] 根據Keywords尋找目標物件")]
    public void FindTargetObjects() => modelForDisplay.FindTargetObjects();
    [ContextMenu("- [Parent] 新增BoxCollider到目標物件")]
    public void AddColliderToObjects() => modelForDisplay.AddColliderToObjects();
    [ContextMenu("- [Parent] 移除目標物件的Collider")]
    public void RemoveColliderFromObjects() => modelForDisplay.RemoveColliderFromObjects();
}
