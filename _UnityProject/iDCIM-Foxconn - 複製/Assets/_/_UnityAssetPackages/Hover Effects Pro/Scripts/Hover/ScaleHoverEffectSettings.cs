using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class ScaleHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings();

        [SerializeField]
        private HoverScalePivot _scalePivot = HoverScalePivot.MeshPivot;
        [SerializeField]
        private float _firstScaleFactor = 1.2f;
        [SerializeField]
        private float _secondScaleFactor = 0.8f;
        [SerializeField]
        private float _transitionDuration = 0.7f;

        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public HoverScalePivot ScalePivot { get { return _scalePivot; } set { _scalePivot = value; } }
        public float FirstScaleFactor { get { return _firstScaleFactor; } set { _firstScaleFactor = Mathf.Max(1e-5f, value); } }
        public float SecondScaleFactor { get { return _secondScaleFactor; } set { _secondScaleFactor = Mathf.Max(1e-5f, value); } }
        public float TransitionDuration { get { return _transitionDuration; } set { _transitionDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat;
            HoverScalePivot newScalePivot;

            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Scale Pivot";
            newScalePivot = (HoverScalePivot)EditorGUILayout.EnumPopup(content, ScalePivot);
            if (newScalePivot != ScalePivot)
            {
                UndoEx.Record(undoRecordObject);
                ScalePivot = newScalePivot;
            }

            content.text = "First Factor";
            newFloat = EditorGUILayout.FloatField(content, FirstScaleFactor);
            if (newFloat != FirstScaleFactor)
            {
                UndoEx.Record(undoRecordObject);
                FirstScaleFactor = newFloat;
            }

            content.text = "Second Factor";
            newFloat = EditorGUILayout.FloatField(content, SecondScaleFactor);
            if (newFloat != SecondScaleFactor)
            {
                UndoEx.Record(undoRecordObject);
                SecondScaleFactor = newFloat;
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
