using UnityEngine;
using XCharts.Runtime;

public class DataZoomController : MonoBehaviour
{
    [SerializeField] private LineChart chart;

    private DataZoom dataZoom;
    private XAxis xAxis;

    private void Start()
    {
        dataZoom.enable = true; // 开启DataZoom
        dataZoom.start = 0;     // 起始范围
        dataZoom.end = 100;

        xAxis = chart.EnsureChartComponent<XAxis>();
        xAxis.show = true;        // 确保X轴显示
        xAxis.interval = 1;       // 设置标签显示的间隔，默认为1，表示每个数据点都显示一个标签
    }

    private void Update()
    {
        // 计算当前视图范围内的显示点数
        int dataCount = chart.GetSerie(0).dataCount;
        int visibleCount = Mathf.CeilToInt((dataZoom.end - dataZoom.start) / 100f * dataCount);

        // 根据当前的缩放范围计算标签的显示间隔，确保始终显示5个标签
        if (xAxis.interval == 1) xAxis.interval = Mathf.Max(1, visibleCount / 5);
    }

    private void OnValidate()
    {
        chart ??= GetComponent<LineChart>();
        dataZoom = chart.EnsureChartComponent<DataZoom>();
    }
}
