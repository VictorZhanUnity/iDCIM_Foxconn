using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Managers;

public class PipeManager : MonoBehaviour
{
    public List<Transform> targets;
    
    [ContextMenu("ReplaceMaterial")]
    public void ReplaceMaterial()
    {
        //MaterialReplacerHandler.ReplaceMaterial(targets);
    }
    
    [ContextMenu("ReplaceMaterial")]
    public void ReplaceMaterialW()
    {
        //MaterialReplacerHandler.ReplaceMaterial(targets);
    }
}
