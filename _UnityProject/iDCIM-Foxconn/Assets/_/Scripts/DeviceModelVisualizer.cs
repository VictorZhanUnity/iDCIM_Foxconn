using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VictorDev.Common;

public class DeviceModelVisualizer : MonoBehaviour
{
    [Header(">>> 模型對像")]
    [SerializeField] protected List<Transform> modelList;

    [Header(">>> 搜尋名稱關鍵字(選填)")]
    [SerializeField] protected string keyName;

    [Header(">>> 處理模型材質替換")]
    [SerializeField] protected MaterialHandler materialHandler;


    /// <summary>
    /// 暫存模型
    /// </summary>
    protected HashSet<Transform> models { get; set; } = new HashSet<Transform>();

    protected virtual void Awake()
    {
        //依照模型建立Landmark與SelectableObject架構
        modelList.ForEach(model =>
        {
            models.Add(model);
            SelectableObject selectableObj = model.AddComponent<SelectableObject>();
        });
    }

    public virtual bool isOn
    {
        set
        {
            GameManager.RestoreSelectedObject();

            if (value) materialHandler.ReplaceMaterial(models);
            else materialHandler.RestoreOriginalMaterials();
        }
    }

    [ContextMenu("- 搜尋場景上包含關鍵字之物件")]
    protected void FindObjectWithKeyName()
    {
        modelList.Clear();
        // 找出所有場景中的GameObject
        List<Transform> allObjects = FindObjectsOfType<Transform>().ToList();
        allObjects.ForEach(transform =>
        {
            if (transform.name.Contains(keyName)) modelList.Add(transform);
        });
    }
}
