using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 組件Tab鍵切換處理
    /// </summary>
    public class TabSwitchHandler : MonoBehaviour
    {
        [Header(">>> 欲用Tab切換的組件")]
        [SerializeField] protected List<Selectable> selectableComps;

        private bool isSelected { get; set; } = false;
        private int currentIndex { get; set; }

        private void Update()
        {
            if (isSelected && Input.GetKeyDown(KeyCode.Tab))
            {
                currentIndex = (currentIndex + 1) % selectableComps.Count;
                selectableComps[currentIndex].Select();
            }
        }

        private void OnDeselectHandler(string txt) => isSelected = false;
        private void OnSelectHandler(Selectable target)
        {
            isSelected = true;
            currentIndex = selectableComps.IndexOf(target);
        }

        #region [Event Listener]
        private void OnEnable()
        {
            //OfType<T>：過濾集合中屬於指定類型（TMP_InputField）的元素。
            foreach (TMP_InputField inputField in selectableComps.OfType<TMP_InputField>())
            {
                OnAddListener(inputField);
            }
        }
        private void OnDisable()
        {
            //OfType<T>：過濾集合中屬於指定類型（TMP_InputField）的元素。
            foreach (TMP_InputField inputField in selectableComps.OfType<TMP_InputField>())
            {
                OnRemoveListener(inputField);
            }
        }
        protected virtual void OnAddListener(TMP_InputField inputField)
        {
            inputField.onSelect.AddListener((txt) => OnSelectHandler(inputField));
        }
        protected virtual void OnRemoveListener(TMP_InputField inputField)
        {
            inputField.onSelect.RemoveListener((txt) => OnSelectHandler(inputField));
        }
        #endregion
    }
}
