using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TableRow<T> : MonoBehaviour
{
    [Header(">>> [資料項]")] private T _data;
   public T data => _data;

    [Header(">>> [Event] - 點擊項目時Invoke")]
    public UnityEvent<T> onClickItem = new UnityEvent<T>();

    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public bool isOn
    {
        get => toggle.isOn;
        set
        {
            toggle.isOn = value;
            OnToggleValueChanged(value);
        }
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickItem.Invoke(_data);
            OnToggleValueChanged(isOn);
        });
    }

    /// <summary>
    /// 當Toggle選取被改變時
    /// </summary>
    protected abstract void OnToggleValueChanged(bool value);

    public void SetData(T data)
    {
        _data = data;
        OnSetDataHandler(data);
    }
    protected abstract void OnSetDataHandler(T data);

    public void ToReset()
    {
        toggleGroup = null;
        isOn = false;
        onClickItem.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
        onClickItem.RemoveAllListeners();
    }
}