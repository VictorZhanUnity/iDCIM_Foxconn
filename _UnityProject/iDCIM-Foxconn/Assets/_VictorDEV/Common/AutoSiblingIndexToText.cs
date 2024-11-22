using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class AutoSiblingIndexToText : MonoBehaviour
{
    [Header(">>> 文字格式 {value}")]
    public string formater = "{value}";

    [Header(">>> 目標對像")]
    public Transform target;

    [Header(">>> Index調整值")]
    public int offsetIndex = 1;

    private TextMeshPro _txt { get; set; }
    private TextMeshPro txt => _txt ??= GetComponent<TextMeshPro>();

    private void OnEnable()
    {
        formater = formater.Trim();
        string label = formater.Replace("{value}", (target.GetSiblingIndex() + offsetIndex).ToString());
        txt.SetText(label);
    }

    private void OnValidate() => OnEnable();
}
