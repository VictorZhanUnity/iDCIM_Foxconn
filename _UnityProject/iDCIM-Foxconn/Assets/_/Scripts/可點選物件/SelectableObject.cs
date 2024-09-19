using UnityEngine;
using UnityEngine.Events;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] protected GameObject displayObject;

    public UnityEvent<bool> onSelectedEvent = new UnityEvent<bool>();

    public bool IsOn
    {
        set
        {
            SetIsOnWithoutNotify(value);
            onSelectedEvent?.Invoke(value);
        }
    }

    public void SetIsOnWithoutNotify(bool isOn)
    {
        displayObject?.SetActive(isOn);
    }

    protected virtual void Awake()
    {
        displayObject ??= transform.GetChild(0).gameObject;
    }
}
