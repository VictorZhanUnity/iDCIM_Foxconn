using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 初始化設置Dropdown列表
    /// <para>+ 依照Enum內容來設置 </para>
    /// </summary>
    public abstract class DropdownDataInitializer<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private void Start() => SetOptions();

        [ContextMenu("- SetOptions")]
        protected void SetOptions()
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            List<string> options = new List<string>();

            foreach (var value in enumValues)
            {
                options.Add(value.ToString());
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }

        private void OnValidate() => dropdown ??= GetComponent<TMP_Dropdown>();
    }
}