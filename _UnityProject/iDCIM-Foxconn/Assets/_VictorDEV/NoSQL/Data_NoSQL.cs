using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;

/// <summary>
/// NoSQL資料項
/// <para>+ 已處理Dictionary於Inspector裡的視覺化</para>
/// <para>+ 儲存Dictionary為sourceData供子類別使用</para>
/// <para>+已處理GetValue判定是否有key值</para>
/// </summary>
public abstract class Data_NoSQL
{
    [Header(">>> 原始JSON資料(僅顯示)")]
    [SerializeField] protected List<DictionaryVisualizer<string, string>> jsonDictForDisplay = new List<DictionaryVisualizer<string, string>>();

    /// <summary>
    /// 原始JSON資料
    /// </summary>
    protected Dictionary<string, string> sourceData { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// 建構子
    /// </summary>
    protected Data_NoSQL(Dictionary<string, string> sourceData)
    {
        this.sourceData = sourceData;
        jsonDictForDisplay = DictionaryVisualizer<string, string>.Parse(sourceData);
    }

    /// <summary>
    /// 依照欄位名稱取值
    /// </summary>
    public string GetValue(string key) => sourceData.ContainsKey(key) ? sourceData[key] : "";
}
