using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class PointerHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectEnterSettings _enterSettings = new HoverEffectEnterSettings();
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings();

        [SerializeField]
        private HoverEffectSpace _pointerSpace = HoverEffectSpace.Local;
        [SerializeField]
        private HoverPointerAxis _pointerAxis = HoverPointerAxis.Up;
        [SerializeField]
        private HoverPointerType _pointerType = HoverPointerType.Pyramid;
        [SerializeField]
        private float _pyramidBaseWidth = 0.65f;
        [SerializeField]
        private float _pyramidBaseDepth = 0.65f;
        [SerializeField]
        private float _pyramidHeight = 1.3f;
        [SerializeField]
        private float _coneBaseRadius = 0.325f;
        [SerializeField]
        private float _coneHeight = 1.3f;

        [SerializeField]
        private float _rotationSpeed = 80.0f;
        [SerializeField]
        private float _positionOffset = 0.2f;

        [SerializeField]
        private Color _firstColor = Color.green;
        [SerializeField]
        private Color _secondColor = Color.green;
        [SerializeField]
        private float _colorTransitionDuration = 0.7f;
        [SerializeField]
        private HoverShadeMode _shadeMode = HoverShadeMode.Lit;

        [SerializeField]
        private float _firstScaleFactor = 1.2f;
        [SerializeField]
        private float _secondScaleFactor = 0.8f;
        [SerializeField]
        private float _scaleTransitionDuration = 0.7f;

        public HoverEffectEnterSettings EnterSettings { get { return _enterSettings; } }
        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public HoverEffectSpace PointerSpace { get { return _pointerSpace; } set { _pointerSpace = value; } }
        public HoverPointerAxis PointerAxis { get { return _pointerAxis; } set { _pointerAxis = value; } }
        public HoverPointerType PointerType { get { return _pointerType; } set { _pointerType = value; } }
        public float PyramidBaseWidth { get { return _pyramidBaseWidth; } set { _pyramidBaseWidth = Mathf.Max(1e-2f, value); } }
        public float PyramidBaseDepth { get { return _pyramidBaseDepth; } set { _pyramidBaseDepth = Mathf.Max(1e-2f, value); } }
        public float PyramidHeight { get { return _pyramidHeight; } set { _pyramidHeight = Mathf.Max(1e-2f, value); } }
        public float ConeBaseRadius { get { return _coneBaseRadius; } set { _coneBaseRadius = Mathf.Max(1e-2f, value); } }
        public float ConeHeight { get { return _coneHeight; } set { _coneHeight = Mathf.Max(1e-2f, value); } }
        public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }
        public float PositionOffset { get { return _positionOffset; } set { _positionOffset = value; } }
        public Color FirstColor { get { return _firstColor; } set { _firstColor = value; } }
        public Color SecondColor { get { return _secondColor; } set { _secondColor = value; } }
        public float ColorTransitionDuration { get { return _colorTransitionDuration; } set { _colorTransitionDuration = Mathf.Max(1e-5f, value); } }
        public HoverShadeMode ShadeMode { get { return _shadeMode; } set { _shadeMode = value; } }
        public float FirstScaleFactor { get { return _firstScaleFactor; } set { _firstScaleFactor = Mathf.Max(1e-5f, value); } }
        public float SecondScaleFactor { get { return _secondScaleFactor; } set { _secondScaleFactor = Mathf.Max(1e-5f, value); } }
        public float ScaleTransitionDuration { get { return _scaleTransitionDuration; } set { _scaleTransitionDuration = Mathf.Max(1e-5f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat; Color newColor;
            HoverEffectSpace newEffectSpace;
            HoverPointerType newPointerType;
            HoverShadeMode newShadeMode;
            HoverPointerAxis newPointerAxis;

            EnterSettings.RenderEditorGUI(undoRecordObject);
            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Pointer Space";
            newEffectSpace = (HoverEffectSpace)EditorGUILayout.EnumPopup(content, PointerSpace);
            if (newEffectSpace != PointerSpace)
            {
                UndoEx.Record(undoRecordObject);
                PointerSpace = newEffectSpace;
            }

            content.text = "Pointer Axis";
            newPointerAxis = (HoverPointerAxis)EditorGUILayout.EnumPopup(content, PointerAxis);
            if (newPointerAxis != PointerAxis)
            {
                UndoEx.Record(undoRecordObject);
                PointerAxis = newPointerAxis;
            }

            content.text = "Pointer Type";
            newPointerType = (HoverPointerType)EditorGUILayout.EnumPopup(content, PointerType);
            if (newPointerType != PointerType)
            {
                UndoEx.Record(undoRecordObject);
                PointerType = newPointerType;
            }

            if (PointerType == HoverPointerType.Pyramid)
            {
                content.text = "Pyramid Base Width";
                newFloat = EditorGUILayout.FloatField(content, PyramidBaseWidth);
                if (newFloat != PyramidBaseWidth)
                {
                    UndoEx.Record(undoRecordObject);
                    PyramidBaseWidth = newFloat;
                }

                content.text = "Pyramid Base Depth";
                newFloat = EditorGUILayout.FloatField(content, PyramidBaseDepth);
                if (newFloat != PyramidBaseDepth)
                {
                    UndoEx.Record(undoRecordObject);
                    PyramidBaseDepth = newFloat;
                }

                content.text = "Pyramid Height";
                newFloat = EditorGUILayout.FloatField(content, PyramidHeight);
                if (newFloat != PyramidHeight)
                {
                    UndoEx.Record(undoRecordObject);
                    PyramidHeight = newFloat;
                }
            }
            else
            if (PointerType == HoverPointerType.Cone)
            {
                content.text = "Cone Base Radius";
                newFloat = EditorGUILayout.FloatField(content, ConeBaseRadius);
                if (newFloat != ConeBaseRadius)
                {
                    UndoEx.Record(undoRecordObject);
                    ConeBaseRadius = newFloat;
                }

                content.text = "Cone Height";
                newFloat = EditorGUILayout.FloatField(content, ConeHeight);
                if (newFloat != ConeHeight)
                {
                    UndoEx.Record(undoRecordObject);
                    ConeHeight = newFloat;
                }
            }

            EditorGUILayout.Separator();
            content.text = "Rotation Speed";
            newFloat = EditorGUILayout.FloatField(content, RotationSpeed);
            if (newFloat != RotationSpeed)
            {
                UndoEx.Record(undoRecordObject);
                RotationSpeed = newFloat;
            }

            content.text = "Position Offset";
            newFloat = EditorGUILayout.FloatField(content, PositionOffset);
            if (newFloat != PositionOffset)
            {
                UndoEx.Record(undoRecordObject);
                PositionOffset = newFloat;
            }

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

            content.text = "Color Transition Duration";
            newFloat = EditorGUILayout.FloatField(content, ColorTransitionDuration);
            if (newFloat != ColorTransitionDuration)
            {
                UndoEx.Record(undoRecordObject);
                ColorTransitionDuration = newFloat;
            }

            content.text = "Shade Mode";
            newShadeMode = (HoverShadeMode)EditorGUILayout.EnumPopup(content, ShadeMode);
            if (newShadeMode != ShadeMode)
            {
                UndoEx.Record(undoRecordObject);
                ShadeMode = newShadeMode;
            }

            EditorGUILayout.Separator();
            content.text = "First Scale Factor";
            newFloat = EditorGUILayout.FloatField(content, FirstScaleFactor);
            if (newFloat != FirstScaleFactor)
            {
                UndoEx.Record(undoRecordObject);
                FirstScaleFactor = newFloat;
            }

            content.text = "Second Scale Factor";
            newFloat = EditorGUILayout.FloatField(content, SecondScaleFactor);
            if (newFloat != SecondScaleFactor)
            {
                UndoEx.Record(undoRecordObject);
                SecondScaleFactor = newFloat;
            }

            content.text = "Scale Transition Duration";
            newFloat = EditorGUILayout.FloatField(content, ScaleTransitionDuration);
            if (newFloat != ScaleTransitionDuration)
            {
                UndoEx.Record(undoRecordObject);
                ScaleTransitionDuration = newFloat;
            }
        }
        #endif
    }
}
