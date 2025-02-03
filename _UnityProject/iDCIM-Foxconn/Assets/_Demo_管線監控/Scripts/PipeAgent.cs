using System;
using UnityEngine;
using VictorDev.MaterialUtils;

[RequireComponent(typeof(MeshGroupFinder))]
public class PipeAgent : PipeTypeSelector
{
    private MeshGroupFinder MeshGroupFinderInstance => _meshGroupFinder ??= GetComponent<MeshGroupFinder>();
    private MeshGroupFinder _meshGroupFinder;
    public void ToShow() => MeshGroupFinderInstance.ToShow();
    private void OnValidate() => name = $"{GetType().Name}_{pipeType}";
}