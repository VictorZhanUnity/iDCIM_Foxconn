using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HUD_IAQRealtimeIndexAvg : MonoBehaviour
{
    [Header(">>> 間隔幾秒訪問WebAPI")]
    [Range(0, 60)]
    [SerializeField] private int intervalSendRequest = 5;

    [Header(">>> 更新目前平均溫度")]
    public UnityEvent<string> onUpdateCurrentAvgRT = new UnityEvent<string>();
}
