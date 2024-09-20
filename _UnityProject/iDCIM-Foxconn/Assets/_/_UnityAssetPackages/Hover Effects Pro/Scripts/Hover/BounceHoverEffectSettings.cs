using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace HoverEffectsPro
{
    [Serializable]
    public class BounceHoverEffectSettings : Settings
    {
        [SerializeField]
        private HoverEffectExitSettings _exitSettings = new HoverEffectExitSettings()
        {
            ExitDuration = 0.1f
        };

        [SerializeField]
        private HoverEffectSpace _bounceSpace = HoverEffectSpace.Local;
        [SerializeField]
        private HoverBounceAxis _bounceAxis = HoverBounceAxis.Up;
        [SerializeField]
        private float _jumpSpeed = 15.0f;
        [SerializeField]
        private float _jumpFriction = 25.0f;
        [SerializeField]
        private float _fallSpeed = 15.0f;
        [SerializeField]
        private float _gravity = 25.0f;
        [SerializeField]
        private bool _rotateOnJump = true;
        [SerializeField]
        private float _rotationSpeed = 1000.0f;
        [SerializeField]
        private bool _squashOnLanding = true;
        [SerializeField]
        private bool _instantSquashEnter = false;
        [SerializeField]
        private bool _instantSquashExit = false;
        [SerializeField]
        private float _squashTransitionDuration = 0.1f;
        [SerializeField]
        private float _squashFactor = 0.6f;

        public HoverEffectExitSettings ExitSettings { get { return _exitSettings; } }
        public HoverEffectSpace BounceSpace { get { return _bounceSpace; } set { _bounceSpace = value; } }
        public HoverBounceAxis BounceAxis { get { return _bounceAxis; } set { _bounceAxis = value; } }
        public float JumpSpeed { get { return _jumpSpeed; } set { _jumpSpeed = Mathf.Max(1e-3f, value); } }
        public float JumpFriction { get { return _jumpFriction; } set { _jumpFriction = Mathf.Max(1e-5f, value); } }
        public float FallSpeed { get { return _fallSpeed; } set { _fallSpeed = Mathf.Max(1e-3f, value); } }
        public float Gravity { get { return _gravity; } set { _gravity = Mathf.Max(1e-3f, value); } }
        public bool RotateOnJump { get { return _rotateOnJump; } set { _rotateOnJump = value; } }
        public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }
        public bool SquashOnLanding { get { return _squashOnLanding; } set { _squashOnLanding = value; } }
        public bool InstantSquashEnter { get { return _instantSquashEnter; } set { _instantSquashEnter = value; } }
        public bool InstantSquashExit { get { return _instantSquashExit; } set { _instantSquashExit = value; } }
        public float SquashTransitionDuration { get { return _squashTransitionDuration; } set { _squashTransitionDuration = Mathf.Max(1e-4f, value); } }
        public float SquashFactor { get { return _squashFactor; } set { _squashFactor = Mathf.Clamp(value, 1e-4f, 0.98f); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            float newFloat; bool newBool;
            HoverBounceAxis newBounceAxis;
            HoverEffectSpace newBounceSpace;

            ExitSettings.RenderEditorGUI(undoRecordObject);

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Bounce Space";
            newBounceSpace = (HoverEffectSpace)EditorGUILayout.EnumPopup(content, BounceSpace);
            if (newBounceSpace != BounceSpace)
            {
                UndoEx.Record(undoRecordObject);
                BounceSpace = newBounceSpace;
            }

            content.text = "Bounce Axis";
            newBounceAxis = (HoverBounceAxis)EditorGUILayout.EnumPopup(content, BounceAxis);
            if (newBounceAxis != BounceAxis)
            {
                UndoEx.Record(undoRecordObject);
                BounceAxis = newBounceAxis;
            }

            EditorGUILayout.Separator();
            content.text = "Jump Speed";
            newFloat = EditorGUILayout.FloatField(content, JumpSpeed);
            if (newFloat != JumpSpeed)
            {
                UndoEx.Record(undoRecordObject);
                JumpSpeed = newFloat;
            }

            content.text = "Jump Friction";
            newFloat = EditorGUILayout.FloatField(content, JumpFriction);
            if (newFloat != JumpFriction)
            {
                UndoEx.Record(undoRecordObject);
                JumpFriction = newFloat;
            }

            content.text = "Fall Speed";
            newFloat = EditorGUILayout.FloatField(content, FallSpeed);
            if (newFloat != FallSpeed)
            {
                UndoEx.Record(undoRecordObject);
                FallSpeed = newFloat;
            }

            content.text = "Gravity";
            newFloat = EditorGUILayout.FloatField(content, Gravity);
            if (newFloat != Gravity)
            {
                UndoEx.Record(undoRecordObject);
                Gravity = newFloat;
            }

            EditorGUILayout.Separator();
            content.text = "Rotate on Jump";
            newBool = EditorGUILayout.ToggleLeft(content, RotateOnJump);
            if (newBool != RotateOnJump)
            {
                UndoEx.Record(undoRecordObject);
                RotateOnJump = newBool;
            }

            content.text = "Rotation Speed";
            newFloat = EditorGUILayout.FloatField(content, RotationSpeed);
            if (newFloat != RotationSpeed)
            {
                UndoEx.Record(undoRecordObject);
                RotationSpeed = newFloat;
            }

            EditorGUILayout.Separator();
            content.text = "Squash on Landing";
            newBool = EditorGUILayout.ToggleLeft(content, SquashOnLanding);
            if (newBool != SquashOnLanding)
            {
                UndoEx.Record(undoRecordObject);
                SquashOnLanding = newBool;
            }

            content.text = "Instant Squash Enter";
            newBool = EditorGUILayout.ToggleLeft(content, InstantSquashEnter);
            if (newBool != InstantSquashEnter)
            {
                UndoEx.Record(undoRecordObject);
                InstantSquashEnter = newBool;
            }

            content.text = "Instant Squash Exit";
            newBool = EditorGUILayout.ToggleLeft(content, InstantSquashExit);
            if (newBool != InstantSquashExit)
            {
                UndoEx.Record(undoRecordObject);
                InstantSquashExit = newBool;
            }

            if (!InstantSquashEnter || !InstantSquashExit)
            {
                content.text = "Squash Transition Duration";
                newFloat = EditorGUILayout.FloatField(content, SquashTransitionDuration);
                if (newFloat != SquashTransitionDuration)
                {
                    UndoEx.Record(undoRecordObject);
                    SquashTransitionDuration = newFloat;
                }
            }

            content.text = "Squash Factor";
            newFloat = EditorGUILayout.FloatField(content, SquashFactor);
            if (newFloat != SquashFactor)
            {
                UndoEx.Record(undoRecordObject);
                SquashFactor = newFloat;
            }
        }
        #endif
    }
}
