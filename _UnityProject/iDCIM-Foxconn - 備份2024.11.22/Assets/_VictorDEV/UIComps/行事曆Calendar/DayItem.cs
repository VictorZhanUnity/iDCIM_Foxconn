using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DayItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI txtDay;
    [SerializeField] private Image imgInclude, imgInclude_Left, imgInclude_Right;
    [SerializeField] private Image imgSelected, imgToday;
    [SerializeField] private Image imgIsHaveData;

    [SerializeField] private DateTime _dateTime;

    public UnityEvent<DateTime> onMouseOverEvent = new UnityEvent<DateTime>();
    public UnityEvent onMouseExitEvent = new UnityEvent();
    public UnityEvent<DayItem> onClickEvent = new UnityEvent<DayItem>();

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime date
    {
        get => _dateTime;
        set
        {
            _dateTime = value;
            txtDay.SetText(value.ToString("d "));
            imgToday.gameObject.SetActive(value == DateTime.Today);
        }
    }

    /// <summary>
    /// 設定顯示選擇範圍內的日期高亮
    /// </summary>
    public bool isInclude
    {
        set
        {
            imgInclude.gameObject.SetActive(value);
            if (value) txtDay.alpha = 1;
        }
    }

    public bool isHaveData { set => imgIsHaveData.gameObject.SetActive(value); }

    public bool isStartDate { set => imgInclude_Right.gameObject.SetActive(value); }
    public bool isEndDate { set => imgInclude_Left.gameObject.SetActive(value); }

    public bool isSelectedDate
    {
        set
        {
            imgSelected.gameObject.SetActive(value);
            if (value) txtDay.alpha = 1;
        }
    }

    /// <summary>
    /// 是否目前所選擇的月份
    /// </summary>
    public bool isCurrentSelectedMonth { set => txtDay.alpha = (value) ? 1 : 0.3f; }


    private void Start()
    {
        btn.onClick.AddListener(() => onClickEvent.Invoke(this));
    }

    private void OnMouseOver() => onMouseOverEvent.Invoke(_dateTime);
    private void OnMouseExit() => onMouseExitEvent.Invoke();

    private void OnDisable()
    {
        imgInclude.gameObject.SetActive(false);
        imgInclude_Left.gameObject.SetActive(false);
        imgInclude_Right.gameObject.SetActive(false);
        imgSelected.gameObject.SetActive(false);

        imgToday.gameObject.SetActive(_dateTime != null && _dateTime == DateTime.Today);
    }
}
