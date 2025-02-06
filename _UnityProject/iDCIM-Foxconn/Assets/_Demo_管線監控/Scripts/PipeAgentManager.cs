using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.MaterialUtils;

/// 管線種類管理器
public class PipeAgentManager : PipeTypeSelector
{
    [Header(">>> 管線種類列表")] [SerializeField]
    private List<PipeAgent> pipeAgents = new List<PipeAgent>();

    [ContextMenu("- 尋找場景上所有的PipeAgent")]
    private void FindPipeAgents() =>
        pipeAgents = FindObjectsOfType<PipeAgent>().OrderBy(agent => agent.pipeType).ToList();

    [ContextMenu("- 依所選管線種類進行篩選顯示")]
    public void ToShowByPipeType()
    {
        List<PipeAgent> result = pipeAgents.Where(agent => agent.pipeType.Equals(pipeType)).ToList();
        pipeAgents.Except(result).ToList().ForEach(agent => agent.gameObject.SetActive(false));
        ShowPipe(result);
    }

    [ContextMenu("- 依所選管線類別進行篩選顯示")]
    public void ToShowByPipeList()
    {
        List<PipeAgent> result = pipeAgents.Where(agent => agent.PipeName.Equals(PipeName)).ToList();
        pipeAgents.Except(result).ToList().ForEach(agent => agent.gameObject.SetActive(false));
        ShowPipe(result);
    }

    /// 顯示傳來的PipeAgent
    private void ShowPipe(List<PipeAgent> targets)
    {
        RestoreAllMaterials();
        targets.ForEach(agent => agent.gameObject.SetActive(true));
        List<Transform> modelGroup = targets.SelectMany(agent => agent.ModelFindGroup).ToList();
        if (modelGroup.Count > 0) ModelMaterialHandler.ToShow(modelGroup);
        else ModelMaterialHandler.ToHideAll();
    }

    /// 顯示指定管線類型
    public void ShowPipeByType(string pipeType)
    {
        if (pipeType.Equals("ALL", StringComparison.OrdinalIgnoreCase)) ShowAllPipes();
        else
        {
            this.pipeType = pipeType.Trim().StringToEnum<PipeType>();
            ToShowByPipeType();
        }
    }
    [ContextMenu("- 顯示所有管線")]
    public void ShowAllPipes() => ShowPipe(pipeAgents);

    [ContextMenu("- 顯示所有模型材質")]
    public void RestoreAllMaterials() => ModelMaterialHandler.ToShowAll();
}