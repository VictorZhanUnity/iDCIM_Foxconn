using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class RotationHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterSettings _enterSettings = new HoverEffectEnterSettings();
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings();
        [SerializeField]
        private HoverRotationAxis _rotationAxis = HoverRotationAxis.Y;
        [SerializeField]
        private HoverEffectSpace _rotationSpace = HoverEffectSpace.Global;
        [SerializeField]
        private float _rotationSpeed = 200.0f;

        public HoverEffectEnterSettings EnterSettings { get { return _enterSettings; } }
        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public HoverRotationAxis RotationAxis { get { return _rotationAxis; } set { _rotationAxis = value; } }
        public HoverEffectSpace RotationSpace { get { return _rotationSpace; } set { _rotationSpace = value; } }
        public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat;
            HoverRotationAxis newRotationAxis;
            HoverEffectSpace newRotationSpace;

            EnterSettings.RenderEditorGUI(undoRecordObject);
            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Rotation Axis";
            newRotationAxis = (HoverRotationAxis)EditorGUILayout.EnumPopup(content, RotationAxis);
            if (newRotationAxis != RotationAxis)
            {
                UndoEx.Record(undoRecordObject);
                RotationAxis = newRotationAxis;
            }

            content.text = "Rotation Space";
            newRotationSpace = (HoverEffectSpace)EditorGUILayout.EnumPopup(content, RotationSpace);
            if (newRotationSpace != RotationSpace)
            {
                UndoEx.Record(undoRecordObject);
                RotationSpace = newRotationSpace;
            }

            content.text = "Rotation Speed";
            newFloat = EditorGUILayout.FloatField(content, RotationSpeed);
            if (newFloat != RotationSpeed)
            {
                UndoEx.Record(undoRecordObject);
                RotationSpeed = newFloat;
            }
        }
        #endif
    }
}
