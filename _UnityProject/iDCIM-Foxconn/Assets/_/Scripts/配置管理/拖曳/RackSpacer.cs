using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RackSpacer : MonoBehaviour
{
    private Transform _rackTarget { get; set; }
    public Transform rackTarget => _rackTarget ??= transform.parent;

    private TextMeshPro _txtRuIndex { get; set; }
    private TextMeshPro txtRuIndex => _txtRuIndex ??= transform.GetChild(0).GetComponent<TextMeshPro>();

    public int RuIndex
    {
        get => int.Parse(txtRuIndex.text);
        set
        {
            txtRuIndex.SetText(value.ToString());
            name = $"U {value}";
        }
    }
}
