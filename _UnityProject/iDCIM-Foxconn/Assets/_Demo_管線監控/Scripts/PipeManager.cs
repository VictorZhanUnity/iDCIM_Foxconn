using System;
using System.Collections.Generic;
using System.Linq;
using _Demo_管線監控.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using VictorDev.MaterialUtils;

public class PipeManager : MonoBehaviour
{
    public EnumPipeType pipeType;
    
    public List<PipeGroupFinder> pipeGroupList = new List<PipeGroupFinder>();

    [ContextMenu("- 顯示所有管線組")]
    public void ShowAllPipes()
    {
        pipeGroupList.ForEach(pipeGroup=>pipeGroup.ToShow());
    }

    [ContextMenu("- 顯示指定類型的管線組")]
    public void ShowPipes()
    {
        pipeGroupList.FirstOrDefault(pipeGroup => pipeGroup.PipeType == pipeType)?.ToShow();
    }
    
    [ContextMenu("- 恢復所有模型的材質")]
    public void RestoreMaterial() => ModelMaterialHandler.ToShowAll();

    public enum EnumPipeType
    {
        Pipe_Electricity, Pipe_Gas, Pipe_Water, 
    }
}