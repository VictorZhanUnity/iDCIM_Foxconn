using UnityEngine;

public interface IToolTipPanel
{
    bool isOn { get; }
    Vector2 sizeDelta { get; }

    /// <summary>
    /// 顯示資料
    /// </summary>
    /// <param name="data"></param>
    void ShowData(IToolTipPanel_Data data);
    /// <summary>
    /// 關閉
    /// </summary>
    void Close();
    void UpdatePosition(Vector2 position);
}

public interface IToolTipPanel_Data
{
}