using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] protected GameObject displayObject;
    [SerializeField] protected SO_RTSP data;

    public string title => data.title;

    public ListItem listItem;

    public UnityEvent<bool> onToggleEvent = new UnityEvent<bool>();
    public UnityEvent<SO_RTSP, ListItem_CCTV> onSelectedEvent = new UnityEvent<SO_RTSP, ListItem_CCTV>();

    public bool IsOn
    {
        set
        {
            displayObject?.SetActive(value);
            listItem?.SetIsOnWithoutNotify(value);
            onToggleEvent?.Invoke(value);

            if (value) onSelectedEvent.Invoke(data, listItem as ListItem_CCTV);
        }
    }

    public void CreateSoData(string label, string url, Sprite sprite)
    {
        data = ScriptableObject.CreateInstance<SO_RTSP>();
        data.title = label;
        data.url = url;
        data.sprite = sprite;
    }

    public void SetIsOnWithoutNotify(bool isOn)
    {
        displayObject?.SetActive(isOn);
        if (isOn) onSelectedEvent.Invoke(data, listItem as ListItem_CCTV);
    }

    protected virtual void Awake()
    {
        displayObject ??= transform.GetChild(0).gameObject;
    }
}
