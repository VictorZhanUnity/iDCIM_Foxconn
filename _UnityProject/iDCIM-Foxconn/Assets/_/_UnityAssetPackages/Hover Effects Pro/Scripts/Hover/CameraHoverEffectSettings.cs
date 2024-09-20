using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class CameraHoverEffectSettings : Settings
    {
        [SerializeField]
        private Camera _targetCamera;

        [SerializeField]
        private HoverCameraFocusType _focusType = HoverCameraFocusType.Smooth;
        [SerializeField]
        private float _focusDuration = 0.3f;
        [SerializeField]
        private float _smoothValue = 5.0f;
        [SerializeField]
        private float _focusScale = 1.5f;

        public Camera TargetCamera { get { return _targetCamera; } set { if (value != null) _targetCamera = value; } }
        public HoverCameraFocusType FocusType { get { return _focusType; } set { _focusType = value; } }
        public float FocusDuration { get { return _focusDuration; } set { _focusDuration = Mathf.Max(1e-5f, value); } }
        public float SmoothValue { get { return _smoothValue; } set { _smoothValue = Math.Max(1e-5f, value); } }
        public float FocusScale { get { return _focusScale; } set { _focusScale = Mathf.Max(1.0f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            Camera newCamera; float newFloat;
            HoverCameraFocusType newFocusType;

            var content = new GUIContent();
            content.text = "Target Camera";
            newCamera = EditorGUILayout.ObjectField(content, TargetCamera, typeof(Camera), true) as Camera;
            if (newCamera != TargetCamera)
            {
                if (newCamera != null && newCamera.gameObject.IsSceneObject())
                {
                    UndoEx.Record(undoRecordObject);
                    TargetCamera = newCamera;
                }
            }

            content.text = "Focus Type";
            newFocusType = (HoverCameraFocusType)EditorGUILayout.EnumPopup(content, FocusType);
            if (newFocusType != FocusType)
            {
                UndoEx.Record(undoRecordObject);
                FocusType = newFocusType;
            }

            if (FocusType == HoverCameraFocusType.Linear)
            {
                content.text = "Duration";
                newFloat = EditorGUILayout.FloatField(content, FocusDuration);
                if (newFloat != FocusDuration)
                {
                    UndoEx.Record(undoRecordObject);
                    FocusDuration = newFloat;
                }
            }
            else
            if (FocusType == HoverCameraFocusType.Smooth)
            {
                content.text = "Smooth Value";
                newFloat = EditorGUILayout.FloatField(content, SmoothValue);
                if (newFloat != SmoothValue)
                {
                    UndoEx.Record(undoRecordObject);
                    SmoothValue = newFloat;
                }
            }

            content.text = "Focus Scale";
            newFloat = EditorGUILayout.FloatField(content, FocusScale);
            if (newFloat != FocusScale)
            {
                UndoEx.Record(undoRecordObject);
                FocusScale = newFloat;
            }
        }
        #endif
    }
}
