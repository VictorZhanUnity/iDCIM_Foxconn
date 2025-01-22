using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;

public class ModelVisibleHandler : SingletonMonoBehaviour<ModelVisibleHandler>
{
    [Header(">>> 指定要替換的材質")] [SerializeField]
    private Material replaceMaterial;

    [Header(">>> 指定要替換的物件對像")] [SerializeField]
    private Transform targetTransform;
}


