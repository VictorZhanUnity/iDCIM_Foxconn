using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Common;

public class DeviceModelVisualizer : MonoBehaviour
{
    [Header(">>> 模型對像")]
    [SerializeField] private List<Transform> modelList;

    [Header(">>> 搜尋名稱關鍵字(選填)")]
    [SerializeField] private string keyName;

    [Header(">>> 處理模型材質替換")]
    [SerializeField] private MaterialHandler materialHandler;

    [Space(20)]

    [Header(">>> 地標類型")]
    [SerializeField] private LandmarkCategory landmarkCategory;
    [Header(">>> 地標樣式")]
    [SerializeField] private Landmark landMarkPrefab;
    [Header(">>> 地標高度")]
    [SerializeField] private float offsetHeight;

    /// <summary>
    /// 暫存模型
    /// </summary>
    private HashSet<Transform> models { get; set; } = new HashSet<Transform>();



    private void Awake()
    {
        modelList.ForEach(model =>
        {
            models.Add(model);
            Landmark landMark = ObjectPoolManager.GetInstanceFromQueuePool<Landmark>(landMarkPrefab, LandmarkManager.container);
            landMark.Initialize(model, offsetHeight, landmarkCategory);
        });
    }

    public bool isOn
    {
        set
        {
            if (value) materialHandler.ReplaceMaterial(models);
            else materialHandler.RestoreOriginalMaterials();
        }
    }

    [ContextMenu("- 搜尋場景上包含關鍵字之物件")]
    private void FindObjectWithKeyName()
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
