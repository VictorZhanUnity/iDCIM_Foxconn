using System;
using System.Collections.Generic;
using UnityEngine;

public class CalendarEventManager : MonoBehaviour
{
    // 使用 Dictionary 儲存每一天的事件
    private Dictionary<DateTime, List<string>> events = new Dictionary<DateTime, List<string>>();

    // 添加事件
    public void AddEvent(DateTime date, string eventName)
    {
        if (!events.ContainsKey(date))
        {
            events[date] = new List<string>();
        }
        events[date].Add(eventName);
    }

    // 獲取某天的事件
    public List<string> GetEvents(DateTime date)
    {
        if (events.ContainsKey(date))
        {
            return events[date];
        }
        return new List<string>();
    }
}
