using System;
using System.Collections;
using System.Collections.Generic;
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

    public UnityEvent<DateTime> onMouseOver = new UnityEvent<DateTime>();
    public UnityEvent onMouseExit = new UnityEvent();

    public DateTime date
    {
        set
        {
            _dateTime = value;
            txtDay.SetText(value.ToString("dd"));

            imgToday.gameObject.SetActive(value == DateTime.Today);
        }
    }

    private void OnMouseOver() => onMouseOver.Invoke(_dateTime);
    private void OnMouseExit() => onMouseExit.Invoke();
}
