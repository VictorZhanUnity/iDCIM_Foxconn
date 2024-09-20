using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class BoxHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterSettings _enterSettings = new HoverEffectEnterSettings()
        {
            EnterDuration = 0.3f
        };
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings()
        {
            ExitDuration = 0.3f
        };

        [SerializeField]
        private HoverBoxFillFlags _boxFillFlags = HoverBoxFillFlags.Wire;
        [SerializeField]
        private HoverBoxWireType _boxWireType = HoverBoxWireType.WireCorners;
        [SerializeField]
        private float _wireCornerLinePercentage = 0.67f;

        [SerializeField]
        private Color _firstWireColor = Color.white;
        [SerializeField]
        private Color _secondWireColor = Color.white;
        [SerializeField]
        private Color _firstSolidColor = Color.green.KeepAllButAlpha(0.1f);
        [SerializeField]
        private Color _secondSolidColor = Color.green.KeepAllButAlpha(0.23f);
        [SerializeField]
        private float _colorTransitionDuration = 0.7f;

        [SerializeField]
        private Vector3 _firstInflate = Vector3Ex.FromValue(0.1f);
        [SerializeField]
        private Vector3 _secondInflate = Vector3Ex.FromValue(0.8f);
        [SerializeField]
        private float _inflateTransitionDuration = 0.7f;

        public HoverEffectEnterSettings EnterSettings { get { return _enterSettings; } }
        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public HoverBoxFillFlags BoxFillFlags { get { return _boxFillFlags; } set { _boxFillFlags = value; } }
        public HoverBoxWireType BoxWireType { get { return _boxWireType; } set { _boxWireType = value; } }
        public float WireCornerLinePercentage { get { return _wireCornerLinePercentage; } set { _wireCornerLinePercentage = Mathf.Clamp(value, 1e-2f, 1.0f); } }
        public Color FirstWireColor { get { return _firstWireColor; } set { _firstWireColor = value; } }
        public Color SecondWireColor { get { return _secondWireColor; } set { _secondWireColor = value; } }
        public Color FirstSolidColor { get { return _firstSolidColor; } set { _firstSolidColor = value; } }
        public Color SecondSolidColor { get { return _secondSolidColor; } set { _secondSolidColor = value; } }
        public float ColorTransitionDuration { get { return _colorTransitionDuration; } set { _colorTransitionDuration = Mathf.Max(1e-5f, value); } }
        public Vector3 FirstInflate { get { return _firstInflate; } set { _firstInflate = Vector3.Max(Vector3.zero, value); } }
        public Vector3 SecondInflate { get { return _secondInflate; } set { _secondInflate = Vector3.Max(Vector3.zero, value); } }
        public float InflateTransitionDuration { get { return _inflateTransitionDuration; } set { _inflateTransitionDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat; Color newColor; Vector3 newVec3;
            HoverBoxFillFlags newFillFlags;
            HoverBoxWireType newWireType;

            EnterSettings.RenderEditorGUI(undoRecordObject);
            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Fill Flags";
            newFillFlags = (HoverBoxFillFlags)EditorGUILayout.EnumFlagsField(content, BoxFillFlags);
            if (newFillFlags != BoxFillFlags)
            {
                UndoEx.Record(undoRecordObject);
                BoxFillFlags = newFillFlags;
            }

            if ((BoxFillFlags & HoverBoxFillFlags.Wire) != 0)
            {
                content.text = "Wire Type";
                newWireType = (HoverBoxWireType)EditorGUILayout.EnumPopup(content, BoxWireType);
                if (newWireType != BoxWireType)
                {
                    UndoEx.Record(undoRecordObject);
                    BoxWireType = newWireType;
                }

                if (BoxWireType == HoverBoxWireType.WireCorners)
                {
                    content.text = "Wire Corner Line Percentage";
                    newFloat = EditorGUILayout.FloatField(content, WireCornerLinePercentage);
                    if (newFloat != WireCornerLinePercentage)
                    {
                        UndoEx.Record(undoRecordObject);
                        WireCornerLinePercentage = newFloat;
                    }
                }

                content.text = "First Wire Color";
                newColor = EditorGUILayout.ColorField(content, FirstWireColor);
                if (newColor != FirstWireColor)
                {
                    UndoEx.Record(undoRecordObject);
                    FirstWireColor = newColor;
                }

                content.text = "Second Wire Color";
                newColor = EditorGUILayout.ColorField(content, SecondWireColor);
                if (newColor != SecondWireColor)
                {
                    UndoEx.Record(undoRecordObject);
                    SecondWireColor = newColor;
                }
            }

            if ((BoxFillFlags & HoverBoxFillFlags.Solid) != 0)
            {
                content.text = "First Solid Color";
                newColor = EditorGUILayout.ColorField(content, FirstSolidColor);
                if (newColor != FirstSolidColor)
                {
                    UndoEx.Record(undoRecordObject);
                    FirstSolidColor = newColor;
                }

                content.text = "Second Solid Color";
                newColor = EditorGUILayout.ColorField(content, SecondSolidColor);
                if (newColor != SecondSolidColor)
                {
                    UndoEx.Record(undoRecordObject);
                    SecondSolidColor = newColor;
                }
            }

            content.text = "Color Transition Duration";
            newFloat = EditorGUILayout.FloatField(content, ColorTransitionDuration);
            if (newFloat != ColorTransitionDuration)
            {
                UndoEx.Record(undoRecordObject);
                ColorTransitionDuration = newFloat;
            }

            EditorGUILayout.Separator();
            content.text = "First Inflate";
            newVec3 = EditorGUILayout.Vector3Field(content, FirstInflate);
            if (newVec3 != FirstInflate)
            {
                UndoEx.Record(undoRecordObject);
                FirstInflate = newVec3;
            }

            content.text = "Second Inflate";
            newVec3 = EditorGUILayout.Vector3Field(content, SecondInflate);
            if (newVec3 != SecondInflate)
            {
                UndoEx.Record(undoRecordObject);
                SecondInflate = newVec3;
            }

            content.text = "Inflate Transition Duration";
            newFloat = EditorGUILayout.FloatField(content, InflateTransitionDuration);
            if (newFloat != InflateTransitionDuration)
            {
                UndoEx.Record(undoRecordObject);
                InflateTransitionDuration = newFloat;
            }
        }
        #endif
    }
}
