using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;
using static AlarmHistoryDataManager;

public class AlarmDetailListPanel_Mediator : MonoBehaviour
{
    ///[Prefab] - 視窗 - 告警詳細列表
    private AlarmDetailListPanel PanelPrefab => _panelPrefab ??= transform.GetChild(0).GetComponent<AlarmDetailListPanel>();
    private AlarmDetailListPanel _panelPrefab;

    private RectTransform ParenCanvas => _parenCanvas ??= transform.parent.GetComponent<RectTransform>();
    private RectTransform _parenCanvas;
    
    public void Receive(List<Data_AlarmHistoryData> data, string title)
    {
        AlarmDetailListPanel panel = Instantiate(PanelPrefab, transform);
        panel.ReceiveAlarmDataList(data, title);
        panel.GetComponent<DragPanel>().ParentRectTransform = ParenCanvas;
    }
}