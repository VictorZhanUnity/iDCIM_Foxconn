using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_IAQ : MonoBehaviour
{
    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizer;
    [SerializeField] private GameObject canvasObj;

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            canvasObj.SetActive(value);
        }
    }
}
