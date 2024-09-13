using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public class ColorHoverEffectCoroutine : HoverEffectCoroutine
    {
        private List<Material> _materials;
        private Color[] _onEnterColors;

        private HoverEffectEnterMode _enterMode;
        private float _enterDuration;
        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private string _colorPropertyName;
        private Color _firstColor;
        private Color _secondColor;
        private float _transitionDuration;

        public override HoverEffectType EffectType { get { return HoverEffectType.Color; } }

        public void DestroyEffectMaterials()
        {
            if (_materials != null)
            {
                foreach (var material in _materials)
                    if (material != null) Material.Destroy(material);
            }
        }

        protected override bool InitializeBeforePlay()
        {
            _colorPropertyName = _parentEffect.ColorEffectSettings.ColorPropertyName;
            _enterMode = _parentEffect.ColorEffectSettings.EnterSettings.EnterMode;
            _enterDuration = _parentEffect.ColorEffectSettings.EnterSettings.EnterDuration;
            _exitMode = _parentEffect.ColorEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.ColorEffectSettings.ExitSettings.ExitDuration;
            _transitionDuration = _parentEffect.ColorEffectSettings.TransitionDuration;
            _firstColor = _parentEffect.ColorEffectSettings.FirstColor;
            _secondColor = _parentEffect.ColorEffectSettings.SecondColor;

            return SetupMaterialsAndColors();
        }

        protected override IEnumerator EnterEffect()
        {
            if (_enterMode == HoverEffectEnterMode.Instant) ApplyColor(_firstColor);
            else
            if (_enterMode == HoverEffectEnterMode.LinearTransition)
            {               
                float elapsedTime = 0.0f;
                while (elapsedTime < _enterDuration)
                {
                    float t = elapsedTime / _enterDuration;
                    for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                        _materials[matIndex].color = Color.Lerp(_onEnterColors[matIndex], _firstColor, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                ApplyColor(_firstColor);
            }
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant) ApplyColors(_onEnterColors);
            else
            if (_exitMode == HoverEffectExitMode.LinearTransition)
            {
                Color[] onExitColors = GetMaterialColors();

                float elapsedTime = 0.0f;
                while (elapsedTime < _exitDuration)
                {
                    float t = elapsedTime / _exitDuration;
                    for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                        _materials[matIndex].color = Color.Lerp(onExitColors[matIndex], _onEnterColors[matIndex], t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                ApplyColors(onExitColors);
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            float elapsedTime = 0.0f;
            float sign = 1.0f;

            while (true)
            {
                float t = elapsedTime / _transitionDuration;
                for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                    _materials[matIndex].color = Color.Lerp(_firstColor, _secondColor, t);

                elapsedTime += Time.deltaTime * sign;
                if (elapsedTime >= _transitionDuration)
                {
                    sign = -1.0f;
                    elapsedTime = _transitionDuration;
                }
                else
                if (elapsedTime <= 0.0f)
                {
                    sign = 1.0f;
                    elapsedTime = 0.0f;
                }

                yield return null;
            }
        }

        private bool SetupMaterialsAndColors()
        {
            if (_targetInfo.TargetRenderers == null || _targetInfo.TargetRenderers.Count == 0)
            {
                Debug.LogError("ColorHoverEffectCoroutine: Missing renderer. Effect can not be played.");
                return false;
            }

            _materials = new List<Material>();
            foreach (var renderer in _targetInfo.TargetRenderers)
                _materials.AddRange(renderer.materials);

            _onEnterColors = new Color[_materials.Count];
            for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                _onEnterColors[matIndex] = _materials[matIndex].GetColor(_colorPropertyName);

            return true;
        }

        private void ApplyColor(Color color)
        {
            foreach (var material in _materials)
                material.SetColor(_colorPropertyName, color);
        }

        private void ApplyColors(Color[] colors)
        {
            for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                _materials[matIndex].SetColor(_colorPropertyName, colors[matIndex]);
        }

        private Color[] GetMaterialColors()
        {
            Color[] colors = new Color[_materials.Count];
            for (int matIndex = 0; matIndex < _materials.Count; ++matIndex)
                colors[matIndex] = _materials[matIndex].GetColor(_colorPropertyName);

            return colors;
        }
    }
}
