using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Dashboard : MonoBehaviour
{
    [SerializeField] private GameObject uiObj;

    public bool isOn
    {
        set
        {
            uiObj.SetActive(value);
        }
    }
}
