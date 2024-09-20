using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace HoverEffectsPro
{
    [CustomEditor(typeof(HoverEffect))]
    public class HoverEffectInspector : Editor
    {
        private HoverEffect _effect;

        public override void OnInspectorGUI()
        {
            bool newBool; Collider newCollider;

            EditorUIEx.SectionHeader("Playable Effects");
            var allEffectTypes = Enum.GetValues(typeof(HoverEffectType)).Cast<HoverEffectType>();
            foreach(var effectType in allEffectTypes)
            {
                bool canPlay = _effect.CanPlayEffect(effectType);
                newBool = EditorGUILayout.ToggleLeft(effectType.ToString(), canPlay);
                if (newBool != canPlay)
                {
                    UndoEx.Record(_effect);
                    _effect.SetCanPlayEffect(effectType, newBool);
                }
            }

            EditorGUILayout.BeginHorizontal();
            const float btnWidth = 100.0f;
            if (GUILayout.Button("Check All", GUILayout.Width(btnWidth)))
            {
                UndoEx.Record(_effect);
                _effect.SetCanPlayAll(true);
            }
            if (GUILayout.Button("Uncheck All", GUILayout.Width(btnWidth)))
            {
                UndoEx.Record(_effect);
                _effect.SetCanPlayAll(false);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            var content = new GUIContent();
            content.text = "Hover Collider";
            newCollider = EditorGUILayout.ObjectField(content, _effect.HoverCollider3D, typeof(Collider), true) as Collider;
            if (newCollider != _effect.HoverCollider3D)
            {
                if (newCollider != null && newCollider.gameObject.IsSceneObject())
                {
                    UndoEx.Record(_effect);
                    _effect.HoverCollider3D = newCollider;
                }
            }

            content.text = "Include Children";
            newBool = EditorGUILayout.ToggleLeft(content, _effect.IncludeChildren);
            if (newBool != _effect.IncludeChildren)
            {
                UndoEx.Record(_effect);
                _effect.IncludeChildren = newBool;
            }

            content.text = "Use Skin Local Bounds";
            newBool = EditorGUILayout.ToggleLeft(content, _effect.UseSkinLocalBounds);
            if (newBool != _effect.UseSkinLocalBounds)
            {
                UndoEx.Record(_effect);
                _effect.UseSkinLocalBounds = newBool;
            }

            EditorGUILayout.Separator();
            if (_effect.CanPlayEffect(HoverEffectType.Color))
            {
                EditorUIEx.SectionHeader("Color Effect Settings");
                _effect.ColorEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Rotation))
            {
                EditorUIEx.SectionHeader("Rotation Effect Settings");
                _effect.RotationEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Scale))
            {
                EditorUIEx.SectionHeader("Scale Effect Settings");
                _effect.ScaleEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Float))
            {
                EditorUIEx.SectionHeader("Float Effect Settings");
                _effect.FloatEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Box))
            {
                EditorUIEx.SectionHeader("Box Effect Settings");
                _effect.BoxEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Camera))
            {
                EditorUIEx.SectionHeader("Camera Effect Settings");
                EditorGUILayout.HelpBox("This effect requires a left click to enter. Right click to exit.", MessageType.Info);
                _effect.CameraEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Pointer))
            {
                EditorUIEx.SectionHeader("Pointer Effect Settings");
                _effect.PointerEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
            if (_effect.CanPlayEffect(HoverEffectType.Bounce))
            {
                EditorUIEx.SectionHeader("Bounce Effect Settings");
                _effect.BounceEffectSettings.RenderEditorGUI(_effect);
                EditorGUILayout.Separator();
            }
        }

        private void OnEnable()
        {
            _effect = target as HoverEffect;
        }
    }
}
