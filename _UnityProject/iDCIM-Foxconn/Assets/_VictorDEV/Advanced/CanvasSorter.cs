using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

public class CanvasSorter : SingletonMonoBehaviour<CanvasSorter>
{
    [SerializeField] private List<Canvas> allCanvases = new List<Canvas>();
    private int maxOrder => allCanvases.Max(canvas => canvas.sortingOrder);


    /// <summary>
    /// 將目標Canvas置於最前面
    /// </summary>
    public static void MoveCanvasToFront(Canvas targetCanvas)
    {
        if (Instance.allCanvases.Count == 0) Instance.allCanvases = FindObjectsOfType<Canvas>().ToList();
        targetCanvas.sortingOrder = Instance.maxOrder + 1;
    }
}
