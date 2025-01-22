using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

public class UIManager_Dashboard : MonoBehaviour
{
    [SerializeField] private List<Transform> walls;
    [SerializeField] private GameObject uiObj;
   // [SerializeField] private MaterialHandler materialHandler;

    /// <summary>
    /// �Ȧs�ҫ�, �ѧ�������i�櫢�ƪ����
    /// </summary>
    protected HashSet<Transform> models { get; set; } = new HashSet<Transform>();

    public bool isOn
    {
        set
        {
            RaycastHitManager.RestoreSelectedObjects();

            if (value) MaterialHandler.ReplaceMaterial(walls.ToHashSet());
            else MaterialHandler.RestoreOriginalMaterials();

            uiObj.SetActive(value);
        }
    }
}
