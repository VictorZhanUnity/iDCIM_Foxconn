using System.Collections.Generic;
using UnityEngine;
using VictorDev.MaterialUtils;

/// 管理管線物件與其顯示圖標
[RequireComponent(typeof(MeshGroupFinder))]
public class PipeAgent : PipeTypeSelector
{
    public void ToShow() => MeshGroupFinderInstance.ToShow();

    private void OnValidate()
    {
        if (name.Contains(pipeType.ToString()) == false)
        {
            name = $"{GetType().Name}_{pipeType}";
        }
    }

    #region Components
    private MeshGroupFinder MeshGroupFinderInstance => _meshGroupFinder ??= GetComponent<MeshGroupFinder>();
    private MeshGroupFinder _meshGroupFinder;
    public List<Transform> ModelFindGroup => MeshGroupFinderInstance.ModelFindGroup;

    #endregion
}