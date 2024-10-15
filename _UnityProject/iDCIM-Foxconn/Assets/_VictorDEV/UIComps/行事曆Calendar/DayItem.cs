using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI txtDay;
    [SerializeField] private Image imgInclude, imgSelected, imgToday;

    [SerializeField] private DateTime _dateTime;

    public UnityEvent<DateTime> onMouseOverEvent = new UnityEvent<DateTime>();
    public UnityEvent onMouseExitEvent = new UnityEvent();
    public UnityEvent onClickEvent = new UnityEvent();

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime date
    {
        set
        {
            _dateTime = value;
            txtDay.SetText(value.ToString("d "));
            imgToday.gameObject.SetActive(value == DateTime.Today);
        }
    }

    /// <summary>
    /// 是否目前所選擇的月份
    /// </summary>
    public bool isSelectedMonth { set => txtDay.alpha = (value) ? 1 : 0.2f; }

    private void Start()
    {
        btn.onClick.AddListener(onClickEvent.Invoke);
    }

    private void OnMouseOver() => onMouseOverEvent.Invoke(_dateTime);
    private void OnMouseExit() => onMouseExitEvent.Invoke();
}
