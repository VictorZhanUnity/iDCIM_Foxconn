using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;

public class DeviceModelVisualizer : MonoBehaviour
{
    [Header(">>> 模型對像")]
    [SerializeField] protected List<Transform> modelList;

    [Header(">>> 搜尋名稱關鍵字(選填)")]
    [SerializeField] protected string keyName;

    [Header(">>> 處理模型材質替換")]
    [SerializeField] protected MaterialHandler materialHandler;

    public UnityEvent<List<SelectableObject>> onInitlialized = new UnityEvent<List<SelectableObject>>();

    public List<Transform> ModelList => modelList;

    /// <summary>
    /// 暫存模型, 供材質替換進行哈希表比對
    /// </summary>
    protected HashSet<Transform> models { get; set; } = new HashSet<Transform>();

    protected virtual void Start()
    {
        List<SelectableObject> selectableObjects = new List<SelectableObject>();

        //依照模型建立Landmark與SelectableObject架構
        modelList.ForEach(model =>
        {
            models.Add(model);
            SelectableObject selectableObj = model.AddComponent<SelectableObject>();
            selectableObjects.Add(selectableObj); 
        });

        onInitlialized.Invoke(selectableObjects);
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