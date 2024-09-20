using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class BoxHoverEffectCoroutine : HoverEffectCoroutine
    {
        private ObjectBounds.QueryConfig _renderBoundsQConfig;

        private float _elapsedTimeInflate;
        private float _signInflate;

        private Color _solidColor;
        private Color _wireColor;
        private Vector3 _inflate;

        private HoverEffectEnterMode _enterMode;
        private float _enterDuration;
        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private HoverBoxFillFlags _fillFlags;
        private HoverBoxWireType _wireType;
        private float _wireCornerLinePercentage;
        private Color _firstWireColor;
        private Color _secondWireColor;
        private Color _firstSolidColor;
        private Color _secondSolidColor;
        private float _colorTransitionDuration;
        private Vector3 _firstInflate;
        private Vector3 _secondInflate;
        private float _inflateTransitionDuration;

        public override HoverEffectType EffectType { get { return HoverEffectType.Box; } }

        protected override void OnRender()
        {
            OBB worldOBB = OBB.GetInvalid();
            if (_targetInfo.IncludeChildren) worldOBB = ObjectBounds.CalcHierarchyWorldOBB(_targetInfo.TargetObject, _renderBoundsQConfig);
            else worldOBB = ObjectBounds.CalcWorldOBB(_targetInfo.TargetObject, _renderBoundsQConfig);

            if (worldOBB.IsValid)
            {
                worldOBB.Inflate(_inflate);

                Material material = MaterialPool.Get.SimpleColor;
                material.SetZTestEnabled(true);

                if ((_fillFlags & HoverBoxFillFlags.Solid) != 0)
                {
                    material.SetColor("_Color", _solidColor);
                    material.SetPass(0);   
                
                    Graphics.DrawMeshNow(MeshPool.Get.UnitBox, worldOBB.GetUnitBoxTransform());
                }

                if ((_fillFlags & HoverBoxFillFlags.Wire) != 0)
                {
                    material.SetColor("_Color", _wireColor);
                    material.SetPass(0);

                    if (_wireType == HoverBoxWireType.FullWire)
                        Graphics.DrawMeshNow(MeshPool.Get.UnitWireBox, worldOBB.GetUnitBoxTransform());
                    else
                    if (_wireType == HoverBoxWireType.WireCorners)
                        GraphicsEx.DrawWireCornerBox(worldOBB, _wireCornerLinePercentage);
                }
            }
        }

        protected override bool InitializeBeforePlay()
        {
            _renderBoundsQConfig = new ObjectBounds.QueryConfig();
            _renderBoundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            _renderBoundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            _enterMode = _parentEffect.BoxEffectSettings.EnterSettings.EnterMode;
            _enterDuration = _parentEffect.BoxEffectSettings.EnterSettings.EnterDuration;
            _exitMode = _parentEffect.BoxEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.BoxEffectSettings.ExitSettings.ExitDuration;

            _fillFlags = _parentEffect.BoxEffectSettings.BoxFillFlags;
            _wireType = _parentEffect.BoxEffectSettings.BoxWireType;
            _wireCornerLinePercentage = _parentEffect.BoxEffectSettings.WireCornerLinePercentage;
            _firstWireColor = _parentEffect.BoxEffectSettings.FirstWireColor;
            _secondWireColor = _parentEffect.BoxEffectSettings.SecondWireColor;
            _firstSolidColor = _parentEffect.BoxEffectSettings.FirstSolidColor;
            _secondSolidColor = _parentEffect.BoxEffectSettings.SecondSolidColor;
            _colorTransitionDuration = _parentEffect.BoxEffectSettings.ColorTransitionDuration;
            _firstInflate = _parentEffect.BoxEffectSettings.FirstInflate;
            _secondInflate = _parentEffect.BoxEffectSettings.SecondInflate;
            _inflateTransitionDuration = _parentEffect.BoxEffectSettings.InflateTransitionDuration;

            _signInflate = 1.0f;
            _elapsedTimeInflate = 0.0f;

            _inflate = Vector3.zero;
            _solidColor = _firstSolidColor.KeepAllButAlpha(0.0f);
            _wireColor = _firstWireColor.KeepAllButAlpha(0.0f);

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            if (_enterMode == HoverEffectEnterMode.Instant)
            {
                _wireColor = _firstWireColor;
                _solidColor = _firstSolidColor;
                _inflate = _firstInflate;
            }
            else
            {
                float elapsedTimeColor = 0.0f;
                Color startWireColor = _wireColor;
                Color startSolidColor = _solidColor;

                while (elapsedTimeColor < _enterDuration)
                {
                    float t = elapsedTimeColor / _enterDuration;
                    _wireColor = Color.Lerp(startWireColor, _firstWireColor, t);
                    _solidColor = Color.Lerp(startSolidColor, _firstSolidColor, t);
                    elapsedTimeColor += Time.deltaTime;

                    t = _elapsedTimeInflate / _inflateTransitionDuration;
                    _inflate = Vector3.Lerp(_firstInflate, _secondInflate, t);
                    _elapsedTimeInflate += Time.deltaTime * _signInflate;

                    if (_elapsedTimeInflate >= _inflateTransitionDuration)
                    {
                        _elapsedTimeInflate = _inflateTransitionDuration;
                        _signInflate = -1.0f;
                        _inflate = _secondInflate;
                    }
                    else
                    if (_elapsedTimeInflate <= 0.0f)
                    {
                        _elapsedTimeInflate = 0.0f;
                        _signInflate = 1.0f;
                        _inflate = _firstInflate;
                    }

                    yield return null;
                }

                _wireColor = _firstWireColor;
                _solidColor = _firstSolidColor;
            }
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant)
            {
                _wireColor = _wireColor.KeepAllButAlpha(0.0f);
                _solidColor = _solidColor.KeepAllButAlpha(0.0f);
                _inflate = Vector3.zero;
            }
            else
            {
                float elapsedTimeColor = 0.0f;
                Color startWireColor = _wireColor;
                Color startSolidColor = _solidColor;
                Color targetWireColor = startWireColor.KeepAllButAlpha(0.0f);
                Color targetSolidColor = startSolidColor.KeepAllButAlpha(0.0f);

                while (elapsedTimeColor < _exitDuration)
                {
                    float t = elapsedTimeColor / _exitDuration;
                    _wireColor = Color.Lerp(startWireColor, targetWireColor, t);
                    _solidColor = Color.Lerp(startSolidColor, targetSolidColor, t);
                    elapsedTimeColor += Time.deltaTime;

                    t = _elapsedTimeInflate / _inflateTransitionDuration;
                    _inflate = Vector3.Lerp(_firstInflate, _secondInflate, t);

                    _elapsedTimeInflate += Time.deltaTime * _signInflate;
                    if (_elapsedTimeInflate >= _inflateTransitionDuration)
                    {
                        _signInflate = -1.0f;
                        _elapsedTimeInflate = _inflateTransitionDuration;
                        _inflate = _secondInflate;
                    }
                    else
                    if (_elapsedTimeInflate <= 0.0f)
                    {
                        _signInflate = 1.0f;
                        _elapsedTimeInflate = 0.0f;
                        _inflate = _firstInflate;
                    }

                    yield return null;
                }

                _wireColor = targetWireColor;
                _solidColor = targetSolidColor;
                _inflate = Vector3.zero;
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            float elapsedTimeColor = 0.0f;
            float signColor = 1.0f;

            while (true)
            {
                float t = elapsedTimeColor / _colorTransitionDuration;
                _wireColor = Color.Lerp(_firstWireColor, _secondWireColor, t);
                _solidColor = Color.Lerp(_firstSolidColor, _secondSolidColor, t);

                t = _elapsedTimeInflate / _inflateTransitionDuration;
                _inflate = Vector3.Lerp(_firstInflate, _secondInflate, t);

                elapsedTimeColor += Time.deltaTime * signColor;
                if (signColor > 0.0f && elapsedTimeColor >= _colorTransitionDuration)
                {
                    signColor = -1.0f;
                    elapsedTimeColor = _colorTransitionDuration;
                }
                else
                if (signColor < 0.0f && elapsedTimeColor <= 0.0f)
                {
                    signColor = 1.0f;
                    elapsedTimeColor = 0.0f;
                }

                _elapsedTimeInflate += Time.deltaTime * _signInflate;
                if (_elapsedTimeInflate >= _inflateTransitionDuration)
                {
                    _signInflate = -1.0f;
                    _elapsedTimeInflate = _inflateTransitionDuration;
                    _inflate = _secondInflate;
                }
                else
                if (_elapsedTimeInflate <= 0.0f)
                {
                    _signInflate = 1.0f;
                    _elapsedTimeInflate = 0.0f;
                    _inflate = _firstInflate;
                }

                yield return null;
            }
        }
    }
}
