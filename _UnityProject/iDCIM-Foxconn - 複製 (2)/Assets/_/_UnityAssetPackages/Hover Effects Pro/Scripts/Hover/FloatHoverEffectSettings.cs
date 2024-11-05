using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class FloatHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterSettings _enterSettings = new HoverEffectEnterSettings();
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings();

        [SerializeField]
        private float _altitude = 0.5f;
        [SerializeField]
        private float _floatSpeed = 3.0f;
        [SerializeField]
        private HoverFloatAxis _floatAxis = HoverFloatAxis.Y;
        [SerializeField]
        private HoverEffectSpace _floatSpace = HoverEffectSpace.Global;

        public HoverEffectEnterSettings EnterSettings { get { return _enterSettings; } }
        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public float Altitude { get { return _altitude; } set { _altitude = Mathf.Max(1e-5f, value); } }
        public float FloatSpeed { get { return _floatSpeed; } set { _floatSpeed = Mathf.Max(1e-5f, value); } }
        public HoverFloatAxis FloatAxis { get { return _floatAxis; } set { _floatAxis = value; } }
        public HoverEffectSpace FloatSpace { get { return _floatSpace; } set { _floatSpace = value; } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat;
            HoverFloatAxis newFloatAxis;
            HoverEffectSpace newFloatSpace;

            EnterSettings.RenderEditorGUI(undoRecordObject);
            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Float Axis";
            newFloatAxis = (HoverFloatAxis)EditorGUILayout.EnumPopup(content, FloatAxis);
            if (newFloatAxis != FloatAxis)
            {
                UndoEx.Record(undoRecordObject);
                FloatAxis = newFloatAxis;
            }

            content.text = "Float Space";
            newFloatSpace = (HoverEffectSpace)EditorGUILayout.EnumPopup(content, FloatSpace);
            if (newFloatSpace != FloatSpace)
            {
                UndoEx.Record(undoRecordObject);
                FloatSpace = newFloatSpace;
            }

            content.text = "Float Speed";
            newFloat = EditorGUILayout.FloatField(content, FloatSpeed);
            if (newFloat != FloatSpeed)
            {
                UndoEx.Record(undoRecordObject);
                FloatSpeed = newFloat;
            }

            content.text = "Altitude";
            newFloat = EditorGUILayout.FloatField(content, Altitude);
            if (newFloat != Altitude)
            {
                UndoEx.Record(undoRecordObject);
                Altitude = newFloat;
            }
        }
        #endif
    }
}
