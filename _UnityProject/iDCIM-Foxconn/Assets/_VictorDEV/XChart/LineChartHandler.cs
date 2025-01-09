using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;
using XCharts.Runtime;

namespace _VictorDEV.XChart
{
    public class LineChartHandler : MonoBehaviour
    {
        /// 設置Serie資料 {serie索引，float資料清單}
        public void SetSeriaDatas(int serieIndex, List<int> datas)
        {
            //確認是否存在於資料內
            DictionaryVisualizer<int, List<int>> targetData = seriaDatas.FirstOrDefault(data => data.key == serieIndex);
            if (targetData == null)
                seriaDatas.Add(new DictionaryVisualizer<int, List<int>>(serieIndex, datas));
            else
                targetData.value = datas;
            //設置資料
            LineChartInstance.series[serieIndex].data.Clear();
            datas.ForEach(data=> LineChartInstance.AddData(serieIndex, data));
        }

        /// 清除圖表的data值與X Axis
        public void ClearChart(bool isIncludeXAxis = false)
        {
            LineChartInstance.series.ForEach(keyPair => keyPair.data.Clear());
            if (isIncludeXAxis)
            {
                XAxis xAxis = LineChartInstance.EnsureChartComponent<XAxis>();
                if (xAxis != null)
                {
                    xAxis.data.Clear();
                    xAxis.refreshComponent();
                }
            }
        }
   
        #region [設定格式]
        ///設定YAxis格式 {最小值，最大值, 顯示格式}
        public void SetYAxisMaxMin(float max, float min = 0, string formatter = "{value}")
        {
            YAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            YAxis.min = min;
            YAxis.max = max;
            YAxis.axisLabel.formatter = formatter;
        }

        ///設定Tooltip格式 {顯示格式}
        public void SetTooltip(string numericFormatter = "0.##")
        {
            ToolTip.numericFormatter = numericFormatter;
            ToolTip.refreshComponent();
        }

        /// 設定Serie格式
        public void SetSerieFormat(int serieIndex, string formatter = "{c}", string numericFormatter = "0.#")
        {
            LineChartInstance.series[serieIndex].label.formatter = formatter;
            LineChartInstance.series[serieIndex].label.numericFormatter = numericFormatter;
        }

        /// 設定DataZoom，控制圖表拖曳顯示資料項
        public void SetDataZoom(int maxShowAmount = 5)
        {
            DataZoom.zoomLock = true; //禁止手動拖曳更動start與end範圍
            DataZoom.end = 100;
            DataZoom.start = DataZoom.end - ((float)maxShowAmount / seriaDatas[0].value.Count) * 100f;

            DataZoom.borderColor = new Color32(1, 1, 1, 0);
            DataZoom.fillerColor = new Color(241f / 255, 195f / 255, 81f / 255, 100f / 255);
            DataZoom.left = 100f;
            DataZoom.right = 70f;
            DataZoom.top = 0.87f;
            DataZoom.refreshComponent();
        }
        #endregion

        #region [Initialize]

        private void OnDisable()
        {
            ClearChart();
        }

        #endregion

        #region [Components]

        [Header("[資料項] - SerieIndex索引, float資料列表")]
        [SerializeField] private List<DictionaryVisualizer<int, List<int>>> seriaDatas = new();

        private LineChart LineChartInstance => _lineChart ??= GetComponent<LineChart>();
        private LineChart _lineChart;

        private YAxis YAxis => _yAxis ??= LineChartInstance.EnsureChartComponent<YAxis>();
        private YAxis _yAxis;

        private Tooltip ToolTip => _toolTip ??= LineChartInstance.EnsureChartComponent<Tooltip>();
        private Tooltip _toolTip;

        private DataZoom DataZoom => _dataZoom ??= LineChartInstance.EnsureChartComponent<DataZoom>();
        private DataZoom _dataZoom;

        #endregion
    }
}