using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] protected GameObject displayObject;

    public bool IsShow { set => displayObject?.SetActive(value); }

    protected virtual void Awake()
    {
        displayObject ??= transform.GetChild(0).gameObject;
    }
}
