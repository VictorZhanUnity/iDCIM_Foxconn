using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;
using XCharts.Runtime;

namespace _VictorDEV.XChart
{
    public class LineChartHandler : MonoBehaviour
    {
        /// 設置Seria資料，依照X軸數量自動補齊資料項
        public void SetSeriaDatas(int serieIndex, List<float> datas, string numericFormatter = "0.#")
        {
            LineChartInstance.series[serieIndex].data.Clear();
            datas.ForEach(data=> LineChartInstance.AddData(serieIndex, data));
            int amountToFill = XAxis.data.Count - datas.Count;
            for (int i = 0; i < amountToFill; i++)
            {
                LineChartInstance.AddData(serieIndex, 0);
            }
            
            LineChartInstance.series[serieIndex].label.numericFormatter = numericFormatter;
        }
        
        /// 設置Serie資料 {serie索引，float資料清單}
        public void SetSeriaDatas(int serieIndex, List<int> datas)
        {
            LineChartInstance.series[serieIndex].data.Clear();
            //確認是否存在於資料內
            DictionaryVisualizer<int, List<int>> targetData = seriaDatas.FirstOrDefault(data => data.key == serieIndex);
            if (targetData == null)
                seriaDatas.Add(new DictionaryVisualizer<int, List<int>>(serieIndex, datas));
            else
                targetData.value = datas;
            //設置資料
            datas.ForEach(data=> LineChartInstance.AddData(serieIndex, data));
        }

        public void SetSeriaDatas(int serieIndex, string formater ="{c}", string numericFormatter = "0.#")
        {
            LabelStyle label = LineChartInstance.series[serieIndex].label;
            if (label != null)
            {
                label.formatter = formater;
                label.numericFormatter = numericFormatter;
            }
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

        #region ContextMenu
        [ContextMenu("- 將Y軸與資料格式為°c")]
        public void SetYAxisToTemptureDegree()
        {
            SetSerieFormat(0, "{c}°c", "0.#");
            YAxis.axisLabel.formatter = "{value}°c";
            YAxis.axisLabel.numericFormatter = "F0";
        }
        [ContextMenu("- 將Y軸與資料格式為%")]
        public void SetYAxisToPercentage()
        {
            SetSerieFormat(0, "{c}%", "0.#");
            YAxis.axisLabel.formatter = "{value}%";
            YAxis.axisLabel.numericFormatter = "F0";
        }

        [ContextMenu("- 將X軸設定為00:00~24:00")]
        public void SetXAxisToDayTime()
        {
            List<string> timeStrings = Enumerable.Range(0, 25).Select(value => $"{value:D2}:00").ToList();
            SetXAxisLabels(timeStrings);
        }
        [ContextMenu("- 將X軸設定為1月到12月")]
        public void SetXAxisToMonths()
        { 
            List<string> timeStrings = Enumerable.Range(1, 12).Select(value => $"{value}月").ToList();
            SetXAxisLabels(timeStrings);
        }
        [ContextMenu("- 設定顯示總筆數")]
        public void SetDataShowAmount() => SetDataShowAmount(maxShowAmount);
        #endregion
        
        #region [設定格式]
        ///設定XAxis格式 {文字labe陣列, 顯示格式}
        public void SetXAxisLabels(List<string> labels, string formatter = "{value}")
        {
            XAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            XAxis.data  = labels;
            XAxis.axisLabel.formatter = formatter;
        }
        ///設定YAxis格式 {最小值，最大值, 顯示格式}
        public void SetYAxisMaxMin(float max, float min = 0, string numericFormatter = "0.#")
        {
            YAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            YAxis.min = min;
            YAxis.max = max;
            YAxis.axisLabel.numericFormatter = numericFormatter;
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
        public void SetDataShowAmount(int maxAmount)
        {
            this.maxShowAmount = maxAmount;
            DataZoom.zoomLock = true; //禁止手動拖曳更動start與end範圍
            XAxis.splitNumber = DataZoom.minShowNum = this.maxShowAmount;
            DataZoom.start = DataZoom.end = 0;
            
            /*DataZoom.end = 100;
            DataZoom.start = DataZoom.end - ((float)this.maxShowAmount / seriaDatas[0].value.Count) * 100f;

            DataZoom.borderColor = new Color32(1, 1, 1, 0);
            DataZoom.fillerColor = new Color(241f / 255, 195f / 255, 81f / 255, 100f / 255);
            DataZoom.left = 100f;
            DataZoom.right = 70f;
            DataZoom.top = 0.87f;*/
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
        [Header("X軸顯示總筆數")]
        [SerializeField] private int maxShowAmount = 7;

        private LineChart LineChartInstance => _lineChart ??= GetComponent<LineChart>();
        private LineChart _lineChart;

        private XAxis XAxis => _xAxis ??= LineChartInstance.EnsureChartComponent<XAxis>();
        private XAxis _xAxis;

        private YAxis YAxis => _yAxis ??= LineChartInstance.EnsureChartComponent<YAxis>();
        private YAxis _yAxis;

        private Tooltip ToolTip => _toolTip ??= LineChartInstance.EnsureChartComponent<Tooltip>();
        private Tooltip _toolTip;

        private DataZoom DataZoom => _dataZoom ??= LineChartInstance.EnsureChartComponent<DataZoom>();
        private DataZoom _dataZoom;

        #endregion
    }
}