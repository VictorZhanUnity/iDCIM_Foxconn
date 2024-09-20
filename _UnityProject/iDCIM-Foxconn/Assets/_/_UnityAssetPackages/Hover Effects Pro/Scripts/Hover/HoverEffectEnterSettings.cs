using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class HoverEffectEnterSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterMode _enterMode = HoverEffectEnterMode.LinearTransition;
        [SerializeField]
        private float _enterDuration = 0.7f;

        public HoverEffectEnterMode EnterMode { get { return _enterMode; } set { _enterMode = value; } }
        public float EnterDuration { get { return _enterDuration; } set { _enterDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat;
            HoverEffectEnterMode newEnterMode;

            var content = new GUIContent();
            content.text = "Enter Mode";
            newEnterMode = (HoverEffectEnterMode)EditorGUILayout.EnumPopup(content, EnterMode);
            if (newEnterMode != EnterMode)
            {
                UndoEx.Record(undoRecordObject);
                EnterMode = newEnterMode;
            }

            if (EnterMode == HoverEffectEnterMode.LinearTransition)
            {
                content.text = "Enter Duration";
                newFloat = EditorGUILayout.FloatField(content, EnterDuration);
                if (newFloat != EnterDuration)
                {
                    UndoEx.Record(undoRecordObject);
                    EnterDuration = newFloat;
                }
            }
        }
        #endif
    }
}
