using System.Collections.Generic;
using UnityEngine;

public class PoolManager_Test : MonoBehaviour
{
    [SerializeField] private List<MyPoolItemA> targetListA;
    [SerializeField] private List<MyPoolItemB> targetListB;
    [SerializeField] private List<MyPoolItemC> targetListC;

    public void PushToPool()
    {
        targetListA.ForEach((item) => ObjectPoolManager.PushToPool(item, item.OnReset));
        targetListB.ForEach((item) => ObjectPoolManager.PushToPool(item, item.OnReset));
        targetListC.ForEach((item) => ObjectPoolManager.PushToPool(item, item.OnReset));

        targetListA.Clear();
        targetListB.Clear();
        targetListC.Clear();
    }

    private void GetObjectFromPool<T>(T prefab) where T : Component
    {
        T item = ObjectPoolManager.GetInstanceFromQueuePool(prefab, transform);
    }

    public void GetObjectFromPool(MyPoolItemA prefab) => GetObjectFromPool<MyPoolItemA>(prefab);
    public void GetObjectFromPool(MyPoolItemB prefab) => GetObjectFromPool<MyPoolItemB>(prefab);
    public void GetObjectFromPool(MyPoolItemC prefab) => GetObjectFromPool<MyPoolItemC>(prefab);
}

public abstract class MyPoolItem : MonoBehaviour
{
    public void OnReset()
    {
        Debug.Log($"OnReset: {name} [ {GetType().Name} ]");
    }
}
