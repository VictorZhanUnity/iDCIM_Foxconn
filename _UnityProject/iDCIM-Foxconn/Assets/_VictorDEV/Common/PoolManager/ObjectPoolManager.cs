using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VictorDev.Common;
using VictorDev.EditorTool;
using Debug = VictorDev.Common.Debug;

/// <summary>
/// [Singleton] [static]  物件池管理器
/// <para> + 以Dictionary管理與動態生成多種類別的物件池</para>
/// <para> + 若目標物件類型並不存在於物件池，則以目標物件Prefab自動動態生成實例化</para>
/// </summary>
public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
    [Header(">>> 每個類別的物件池最大數量<<< ")]
    [Range(200, 2000)]
    [SerializeField] private int maxSizeOfEachQueue = 2000;

    /// <summary>
    /// 用Dictionary管理動態數量的物件池
    /// </summary>
    private Dictionary<string, PoolContainer> poolDict { get; set; } = new Dictionary<string, PoolContainer>();

    /// <summary>
    /// 將目標物件移動至物件池
    /// <para>+ 需用泛型明確指定類別型態</para>
    /// <para>+ 先行判斷Dictionary裡有無目標對像的Queue</para>
    /// <para>+ 若無則新建Queue與TransforContainer</para>
    /// <para>+ 以目標對像.GetType()為Key，儲存於Dictionary</para>
    /// </summary>
    public static void PushToPool<T>(Transform container, Action actionOnAddedToPool = null) where T : Component
    {
        List<T> childList = container.GetComponentsInChildren<T>().ToList<T>();
        for (int i = 0; i < childList.Count; i++)
        {
            PushToPool<T>(childList[i].GetComponent<T>(), actionOnAddedToPool);
        }
    }

    /// <summary>
    /// 將目標物件移動至物件池
    /// <para>+ 需用泛型明確指定類別型態</para>
    /// <para>+ 先行判斷Dictionary裡有無目標對像的Queue</para>
    /// <para>+ 若無則新建Queue與TransforContainer</para>
    /// <para>+ 以目標對像.GetType()為Key，儲存於Dictionary</para>
    /// </summary>
    public static void PushToPool<T>(T target, Action actionOnAddedToPool = null) where T : Component
    {
        string className = typeof(T).Name;
        PoolContainer poolContainer;

        if (Instance.poolDict.ContainsKey(className) == false)
            Instance.poolDict[className] = new PoolContainer(className);
        poolContainer = Instance.poolDict[className];

        // 若物件池內物件超過最大數量，則直接刪除
        if (poolContainer.PoolCount < Instance.maxSizeOfEachQueue)
        {
            actionOnAddedToPool?.Invoke();
            poolContainer.AddToQueuePool(target);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log($"[{Instance.GetType().Name}] >>>  [ {className} ] 物件超過最大數量: {Instance.maxSizeOfEachQueue}\t直接進行刪除: {target.name}");
            DestroyImmediate(target.gameObject);
#else
            Destroy(target.gameObject);
#endif
        }
    }

    /// <summary>
    /// 從物件池裡裡擷取目標類別物件
    /// <para>+ 用泛型來指定類別型態</para>
    /// <para>+ 若沒有目標物件時，則以prefab動態實例化並回傳</para>
    /// </summary>
    public static T GetInstanceFromQueuePool<T>(T prefab, Transform container, bool isResetPosition = false) where T : Component
    {
        string className = typeof(T).Name;
        if (Instance.poolDict.ContainsKey(className) == false)
        {
            PoolContainer poolContainer = new PoolContainer(className);
            Instance.poolDict[className] = poolContainer;
        }

        return Instance.poolDict[className].GetInstanceFromQueuePool<T>(prefab, container, isResetPosition);
    }

    protected override void OnValidateAfter() => name = $"{objName} [ MaxSize: {maxSizeOfEachQueue}]";

    /*-------------------------------------------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// 物件池Container與Queue
    /// </summary>
    [Serializable]
    private class PoolContainer
    {
        private Queue<Component> queuePool { get; set; } = new Queue<Component>();
        private Transform container { get; set; }

        public string ClassName { get; private set; }
        public int PoolCount => queuePool.Count;

        /// <summary>
        /// 物件容器 {物件類型名稱, 是否為UI物件
        /// </summary>
        public PoolContainer(string typeName)
        {
            ClassName = typeName;
            queuePool = new Queue<Component>();
            container = new GameObject().transform;
            container.parent = Instance.transform;
            RefreshContainerName();
        }

        /// <summary>
        /// 新增目標對像到Queue裡
        /// </summary>
        public void AddToQueuePool(Component target)
        {
            queuePool.Enqueue(target);
            target.transform.SetParent(container, false);
            target.name = ClassName;
            target.gameObject.SetActive(false);
            ResetTarget(target);
            RefreshContainerName();
        }

        /// <summary>
        /// 從Queue裡擷取物件
        /// <para>+ 若沒有目標物件型態時，則以prefab動態實例化並回傳</para>
        /// <para>★ Instantiate與SetParent裡的worldPositionStays，都要設定為false</para>
        /// <para>★ worldPositionStays為false，讓物件世界座標會隨著父物件而變化，不會固定值</para>
        /// <para>+ 若物件世界座標為固定值的話，則物件呈現的位置會因為父物件的不同而不一樣</para>
        /// </summary>
        public T GetInstanceFromQueuePool<T>(T prefab, Transform container, bool isResetPosition) where T : Component
        {
            Component target =
                (queuePool.Count > 0) ? queuePool.Dequeue() : Instantiate(prefab, container, false);

            target.transform.SetParent(container, false);
            target.transform.localScale = Vector3.one;
            target.gameObject.SetActive(true);

            RefreshContainerName();
            return target.GetComponent<T>();
        }

        /// <summary>
        /// 重置目標對像Transforme為0
        /// </summary>
        private void ResetTarget(Component target)
        {
            target.transform.position = Vector3.zero;
            target.transform.rotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
        }

        private void RefreshContainerName() => container.name = $"{ClassName} [Count:{PoolCount}]";
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ObjectPoolManager))]
    private class ObjectPoolManager_Inspector : InspectorEditor<ObjectPoolManager>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (instance.poolDict.Count == 0) return;

            _CreateSpacer();
            _CreateLabelFiled($"[Queue ClassName]\t[Length]");
            _DrawHorizontalLine();

            foreach (PoolContainer poolContainer in instance.poolDict.Values)
            {
                _CreateLabelFiled($"{poolContainer.ClassName}\t{poolContainer.PoolCount}");
            }
        }
    }
#endif
}
