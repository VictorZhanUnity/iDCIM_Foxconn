using TMPro;
using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 自動擷取當前資料索引值
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutoDataIndex : MonoBehaviour
    {
        [Header(">>> 目標資料項")]
        [SerializeField] private Transform target;

        [SerializeField] private TextMeshProUGUI txt;

        private void Awake() => OnValidate();
        private void OnValidate()
        {
            target ??= transform.parent;
            txt ??= GetComponent<TextMeshProUGUI>();
            if (target != null) txt.SetText((target.GetSiblingIndex()+1).ToString());
        }
    }
}
