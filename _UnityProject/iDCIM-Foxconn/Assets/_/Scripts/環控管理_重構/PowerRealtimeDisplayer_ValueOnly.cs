using System.Collections.Generic;
using System.Linq;
using TMPro;
using VictorDev.Common;

/// <summary>
/// 市電、UPS、PDU電力顯示器
/// </summary>
public class PowerRealtimeDisplayer_ValueOnly : PowerRealtimeDisplayer
{
    private TextMeshProUGUI firstTxt => txtCompList[0];

    /// <summary>
    /// 更新UI
    /// </summary>
    override protected void UpdateUI()
    {
        //將資料依Status與Value進行分組
        string keyword = firstTxt.name.Trim();
        float value = datas.FirstOrDefault(data => data.tagName.Contains(keyword)).value;
        //設定值
        DotweenHandler.ToBlink(firstTxt, value.ToString("0.##"), 0.1f, 0.2f, true);
    }
}

