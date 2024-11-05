using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Advanced;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// CCTV甤
/// </summary>
public class CCTV_9Grid : MonoBehaviour
{
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private GridItem_9Grid gridItemPrefab;
    [SerializeField] private FlexibleGridLayoutGroup gridLayoutGroup;

    public UnityEvent<SO_RTSP> onClickScaleBtn = new UnityEvent<SO_RTSP>();

    private int numMax = 9;

    public Dictionary<SO_RTSP, GridItem_9Grid> dictGridItems { get; private set; } = new Dictionary<SO_RTSP, GridItem_9Grid>();

    public bool isON
    {
        get => canvasObj.activeSelf;
        set
        {
            if (value == false) Close();
            canvasObj.SetActive(value);
        }
    }

    public void Play(SO_RTSP data, ListItem_CCTV listItem)
    {
        //狦
        if (dictGridItems.TryGetValue(data, out GridItem_9Grid item))
        {
            item.ToShining();
            return;
        }

        //璝程计秖
        if (dictGridItems.Count >= numMax)
        {
            List<GridItem_9Grid> gridItemList = dictGridItems.Values.ToList();
            gridItemList[0].Close();
            ObjectPoolManager.PushToPool<GridItem_9Grid>(gridItemList[0]);
            gridItemList.RemoveAt(0);
        }

        //ミ兜ヘ
        item = ObjectPoolManager.GetInstanceFromQueuePool<GridItem_9Grid>(gridItemPrefab, gridLayoutGroup.transform);
        item.canvasObj = canvasObj;
        item.listItem = listItem;
        dictGridItems.Add(data, item);
        item.Show(data);

        item.onClickScaleBtn.AddListener(onClickScaleBtn.Invoke);
        item.onClickCloseBtn.AddListener((target) =>
        {
            dictGridItems.Remove(target.data);
            ResetContstraint();
        });

        ResetContstraint();
    }

    private void ResetContstraint() => gridLayoutGroup.ConstraintCount = ConstraintCount_1920x1080[dictGridItems.Count];
    public void Close()
    {
        dictGridItems.Values.ToList().ForEach(item =>
        {
            item.Close();
            item.onClickScaleBtn.RemoveAllListeners();
        });
        dictGridItems.Clear();
    }
    /// <summary>
    /// {冀竟计秖, Constraint计秖}
    /// </summary>
    private Dictionary<int, int> ConstraintCount_1920x1080 = new Dictionary<int, int>()
    {
        {0, 0 },
        { 1, 1 }, { 2, 1 },{ 3, 2 },
        { 4, 2 }, { 5, 3 },{ 6, 3 },
        { 7, 4 }, { 8, 3 },{ 9, 3 },
    };
}
