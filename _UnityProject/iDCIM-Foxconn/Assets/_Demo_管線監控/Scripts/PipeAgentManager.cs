using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.MaterialUtils;

/// 管線種類管理器
public class PipeAgentManager : PipeTypeSelector
{
    [Header(">>> 管線種類列表")] [SerializeField]
    private List<PipeAgent> pipeAgents = new List<PipeAgent>();

    [ContextMenu("- 尋找場景上所有的PipeAgent")]
    private void FindPipeAgents() => pipeAgents = FindObjectsOfType<PipeAgent>().ToList();

    [ContextMenu("- 依所選管線種類進行篩選顯示")]
    public void ToShowByPipeType()
    {
        RestoreAllMaterials();
        List<PipeAgent> targets = pipeAgents.Where(agent => agent.pipeType.Equals(pipeType)).ToList();
       if(targets.Count > 0) targets.ForEach(agent => agent.ToShow());
       else ModelMaterialHandler.ToHideAll();
    }

    [ContextMenu("- 依所選管線類別進行篩選顯示")]
    public void ToShowByPipeList()
    {
        RestoreAllMaterials();
        List<PipeAgent> targets = pipeAgents.Where(agent => agent.PipeName.Equals(PipeName)).ToList();
        if(targets.Count > 0) targets.ForEach(agent => agent.ToShow());
        else ModelMaterialHandler.ToHideAll();
    }

    [ContextMenu("- 所有模型顯示材質")]
    public void RestoreAllMaterials() => ModelMaterialHandler.ToShowAll();
}