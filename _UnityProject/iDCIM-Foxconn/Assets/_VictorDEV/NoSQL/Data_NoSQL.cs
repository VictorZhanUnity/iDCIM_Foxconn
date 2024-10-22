using System.Collections.Generic;
using TMPro;
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
    /// 原始JSON資料(單一項)
    /// </summary>
    protected Dictionary<string, string> sourceData { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// 建構子
    /// </summary>
    protected Data_NoSQL(Dictionary<string, string> sourceData)
    {
        this.sourceData = sourceData;
        if (this.sourceData != null) jsonDictForDisplay = DictionaryVisualizer<string, string>.Parse(this.sourceData);
    }

    /// <summary>
    /// 依照欄位名稱取值
    /// </summary>
    public string GetValue(string key) => sourceData.ContainsKey(key) ? sourceData[key] : "";

    /// <summary>
    /// 判別TextMeshProUGUI組件的Name，來取得特定Key欄位的值
    /// <para>+ txtUserName => key為UserName</para>
    /// </summary>
    public void SetValueByName(ref List<TextMeshProUGUI> txtList, string keyWord = "txt")
    {
        txtList.ForEach(txt =>
        {
            string key = txt.name.Split(keyWord)[1];
            txt.SetText(GetValue(key));
        });
    }
}
