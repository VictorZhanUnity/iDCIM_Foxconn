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
public abstract class iDCIM_ModuleManager : Module
{
    [Header(">>> [模型處理] 欲顯示的目標物件")]
    [SerializeField] private ModelDisplayConfiguration modelForDisplay;

    /// <summary>
    /// 欲顯示的模型物件
    /// </summary>
    public List<Transform> modelList => modelForDisplay.modelsList;

    public List<Transform> serverRackModels => GetModelsByKeywords("RACK", "ATEN");
    public List<Transform> switchModels => GetModelsByKeywords("Switch");
    public List<Transform> routerModels => GetModelsByKeywords("Router");
    public List<Transform> serverModels => GetModelsByKeywords("Server");

    /// <summary>
    /// 依關鍵字去擷取模型
    /// </summary>
    private List<Transform> GetModelsByKeywords(params string[] keywords)
        => modelList.Where(model => keywords.Any(keyword => model.name.Contains(keyword))).ToList();

    private bool _isOn { get; set; }
    public bool isOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            container.gameObject.SetActive(value);
            if (value) ToShow();
            else ToClose();
        }
    }

    /// <summary>
    /// 顯示UI組件的容器
    /// </summary>
    protected Transform container => _container ??= transform.GetChild(0);
    private Transform _container { get; set; }

    public bool IsOn { set => throw new System.NotImplementedException(); }

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
