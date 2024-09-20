#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public class EditorUIEx
    {
        public static void SectionHeader(string title)
        {
            var style = new GUIStyle("Label");
            style.fontStyle = FontStyle.BoldAndItalic;
            EditorGUILayout.LabelField(title, style);
        }

        public static Enum SelectiveEnumPopup(GUIContent content, Enum selected, List<Enum> allowedValues)
        {
            if (allowedValues.Count == 0) return selected;

            int selectedIndex = allowedValues.IndexOf(selected);
            if (selectedIndex < 0) return selected;

            var allowedLabels = new List<GUIContent>(allowedValues.Count);
            foreach (var enumValue in allowedValues) allowedLabels.Add(new GUIContent(enumValue.ToString()));

            int newSelectedIndex = EditorGUILayout.Popup(content, selectedIndex, allowedLabels.ToArray());
            return allowedValues[newSelectedIndex];
        }
    }
}
#endif