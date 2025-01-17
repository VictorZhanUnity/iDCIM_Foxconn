using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlarmHistoryManager : ModulePage
{
    public UnityEvent onShowEvent = new UnityEvent();
    
    public override void OnInit(Action onInitComplete = null)
    {
    }

    protected override void InitEventListener()
    {
    }

    protected override void OnCloseHandler()
    {
    }

    protected override void OnShowHandler()
    {
        onShowEvent.Invoke();
    }

    protected override void RemoveEventListener()
    {
    }
}
