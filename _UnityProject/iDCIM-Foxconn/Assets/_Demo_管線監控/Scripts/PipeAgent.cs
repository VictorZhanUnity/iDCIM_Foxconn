using System.Collections.Generic;
using UnityEngine;
using VictorDev.MaterialUtils;

[ExecuteAlways]
[RequireComponent(typeof(MeshGroupFinder))]
public class PipeAgent : PipeTypeSelector
{
    private MeshGroupFinder MeshGroupFinderInstance => _meshGroupFinder ??= GetComponent<MeshGroupFinder>();
    private MeshGroupFinder _meshGroupFinder;
    public List<Transform> ModelFindGroup => MeshGroupFinderInstance.ModelFindGroup;

    public void ToShow() => MeshGroupFinderInstance.ToShow();

    private void OnValidate()
    {
        if (name.Contains(pipeType.ToString()) == false)
        {
            name = $"{GetType().Name}_{pipeType}";
        }
    }
}