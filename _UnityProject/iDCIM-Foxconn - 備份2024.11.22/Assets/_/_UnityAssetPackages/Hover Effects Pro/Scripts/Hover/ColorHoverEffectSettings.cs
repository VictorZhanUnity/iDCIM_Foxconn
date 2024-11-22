using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class ColorHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterSettings _enterSettings = new HoverEffectEnterSettings();
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings();

        [SerializeField]
        private string _colorPropertyName = "_Color";
        [SerializeField]
        private Color _firstColor = Color.green;
        [SerializeField]
        private Color _secondColor = Color.white;
        [SerializeField]
        private float _transitionDuration = 0.7f;

        public HoverEffectEnterSettings EnterSettings { get { return _enterSettings; } }
        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public string ColorPropertyName { get { return _colorPropertyName; } set { if (!string.IsNullOrEmpty(value)) _colorPropertyName = value; } }
        public Color FirstColor { get { return _firstColor; } set { _firstColor = value; } }
        public Color SecondColor { get { return _secondColor; } set { _secondColor = value; } }
        public float TransitionDuration { get { return _transitionDuration; } set { _transitionDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat; Color newColor; string newString;

            var content = new GUIContent();
            content.text = "Color Property Name";
            newString = EditorGUILayout.TextField(content, ColorPropertyName);
            if (newString != ColorPropertyName)
            {
                UndoEx.Record(undoRecordObject);
                ColorPropertyName = newString;
            }

            EnterSettings.RenderEditorGUI(undoRecordObject);
            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            content.text = "First Color";
            newColor = EditorGUILayout.ColorField(content, FirstColor);
            if (newColor != FirstColor)
            {
                UndoEx.Record(undoRecordObject);
                FirstColor = newColor;
            }

            content.text = "Second Color";
            newColor = EditorGUILayout.ColorField(content, SecondColor);
            if (newColor != SecondColor)
            {
                UndoEx.Record(undoRecordObject);
                SecondColor = newColor;
            }

            content.text = "Transition Duration";
            newFloat = EditorGUILayout.FloatField(content, TransitionDuration);
            if (newFloat != TransitionDuration)
            {
                UndoEx.Record(undoRecordObject);
                TransitionDuration = newFloat;
            }
        }
        #endif
    }
}
