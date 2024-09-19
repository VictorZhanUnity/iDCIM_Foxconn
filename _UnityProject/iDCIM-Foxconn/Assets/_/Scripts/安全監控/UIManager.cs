using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected DeviceModelVisualizer deviceModelVisualizer;
    [SerializeField] protected GameObject uiObj;

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);
        }
    }

    private void Awake()
    {
        uiObj.SetActive(false);
    }

    private void OnValidate()
    {
        deviceModelVisualizer ??= GetComponent<DeviceModelVisualizer>();
        uiObj ??= transform.GetChild(0).gameObject;
    }
}
