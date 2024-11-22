using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

[RequireComponent(typeof(LineChart))]
public class XChartDataHandler : MonoBehaviour
{
    [Header(">>> 畫面顯示最大筆數")]
    [SerializeField] private int numOfDisplay = 7;

    [Header(">>> DataZoom拉Bar顏色")]
    [SerializeField] private Color colorOfDataZoomDrag = Color.white;

    #region [XChart各個組件]
    private LineChart _chart { get; set; }
    private LineChart chart => _chart ??= GetComponent<LineChart>();
    private XAxis _xAxis { get; set; }
    private XAxis xAxis => _xAxis ??= chart.EnsureChartComponent<XAxis>();
    private YAxis _yAxis { get; set; }
    private YAxis yAxis => _yAxis ??= chart.EnsureChartComponent<YAxis>();
    private Tooltip _toolTip { get; set; }
    private Tooltip toolTip => _toolTip ??= chart.EnsureChartComponent<Tooltip>();
    private DataZoom _dataZoom { get; set; }
    private DataZoom dataZoom => _dataZoom ??= chart.EnsureChartComponent<DataZoom>();
    #endregion

    /// <summary>
    /// 設定Y軸範圍值與顯示樣式
    /// <para>+ numericFormat: "0.## 單位"</para>
    /// </summary>
    public void SetYAxisRangeAndFormat(double min, double max, string numericFormat = null)
    {
        yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxis.min = min;
        yAxis.max = max;
        yAxis.axisLabel.formatter = "";
        if (string.IsNullOrEmpty(numericFormat) == false) yAxis.axisLabel.numericFormatter = numericFormat;
        // yAxis?.refreshComponent();
    }
    /// <summary>
    /// 設定Tooltip顯示樣式
    /// <para>+ numericFormat: "0.## 單位"</para>
    /// </summary>
    public void SetTooltipFormat(string numericFormat)
    {
        toolTip.numericFormatter = numericFormat;
        toolTip.refreshComponent();
    }


    /// <summary>
    /// 設定X軸資料 
    /// </summary>
    public void SetXAxis(List<string> labels)
    {
        xAxis.type = Axis.AxisType.Category;  // 確保 X 軸是類別軸
        ClearXAxisData();
        labels.ForEach(label => chart.AddXAxisData(label));
    }

    public int XAxisSplitNumber { set => xAxis.splitNumber = value; }

    /// <summary>
    /// 設定Series資料
    /// </summary>
    public void SetSeriesData(string serieName, List<float> seriesData)
    {
        ClearSeriesData();
        Serie target = chart.series.FirstOrDefault(serie => serie.serieName == serieName);
        seriesData.ForEach(data => target.AddData(data));
    }

    // <summary>
    /// 設定Series名稱
    /// </summary>
    public void SetSeriesName(string serieName, int serieIndex = 0)
        => chart.series[serieIndex].serieName = serieName;

    // <summary>
    /// 設定Series顯示格式
    /// </summary>
    public void SetSeriesLabelFormat(string serieName, string labelFormat)
    {
        Serie target = chart.series.FirstOrDefault(serie => serie.serieName == serieName);
        target.EnsureComponent<LabelStyle>();
        if (target != null && string.IsNullOrEmpty(labelFormat) == false) target.label.numericFormatter = labelFormat;
    }

    /// <summary>
    /// 設定DataZoom
    /// </summary>
    public void SetDataZoom(int numOfData)
    {
        dataZoom.zoomLock = true; //禁止手動拖曳更動start與end範圍
        dataZoom.end = 100;
        dataZoom.start = dataZoom.end - ((float)numOfDisplay / numOfData) * 100f;
        dataZoom.borderColor = new Color32(1, 1, 1, 0);
        dataZoom.fillerColor = colorOfDataZoomDrag;
        dataZoom.left = 100f;
        dataZoom.right = 70f;
        dataZoom.top = 0.88f;
        dataZoom.bottom = 0.04f;
        //    dataZoom.refreshComponent();
    }

    /// <summary>
    /// 清除X軸
    /// </summary>
    public void ClearXAxisData()
    {
        xAxis.data.Clear();
        //  xAxis.refreshComponent();
    }
    /// <summary>
    /// 清除Series資料
    /// </summary>
    public void ClearSeriesData()
    {
        chart.series.ForEach(serie =>
        {
            serie.data.Clear();
            // serie?.refreshComponent();
        });
    }
}
