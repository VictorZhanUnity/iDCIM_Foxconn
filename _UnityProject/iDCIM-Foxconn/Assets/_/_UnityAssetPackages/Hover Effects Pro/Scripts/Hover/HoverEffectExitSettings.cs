using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class HoverEffectExitSettings : Settings
    {
        [SerializeField]
        private HoverEffectExitMode _exitMode = HoverEffectExitMode.LinearTransition;
        [SerializeField]
        private float _exitDuration = 0.7f;

        public HoverEffectExitMode ExitMode { get { return _exitMode; } set { _exitMode = value; } }
        public float ExitDuration { get { return _exitDuration; } set { _exitDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat;
            HoverEffectExitMode newExitMode;

            var content = new GUIContent();
            content.text = "Exit Mode";
            newExitMode = (HoverEffectExitMode)EditorGUILayout.EnumPopup(content, ExitMode);
            if (newExitMode != ExitMode)
            {
                UndoEx.Record(undoRecordObject);
                ExitMode = newExitMode;
            }

            if (ExitMode == HoverEffectExitMode.LinearTransition)
            {
                content.text = "Exit Duration";
                newFloat = EditorGUILayout.FloatField(content, ExitDuration);
                if (newFloat != ExitDuration)
                {
                    UndoEx.Record(undoRecordObject);
                    ExitDuration = newFloat;
                }
            }
        }
        #endif
    }
}
