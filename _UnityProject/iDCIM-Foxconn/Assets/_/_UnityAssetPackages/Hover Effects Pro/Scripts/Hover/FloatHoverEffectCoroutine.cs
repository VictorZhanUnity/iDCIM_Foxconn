using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class FloatHoverEffectCoroutine : HoverEffectCoroutine
    {
        private float _angle;
        private Vector3 _positionRestore;
        private float _currentFloatSpeed;
        private Vector3 _floatAxisVec;

        private HoverEffectEnterMode _enterMode;
        private float _enterDuration;
        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private float _floatSpeed;
        private float _altitude;
        private HoverFloatAxis _floatAxis;
        private HoverEffectSpace _floatSpace;

        public override HoverEffectType EffectType { get { return HoverEffectType.Float; } }

        protected override bool InitializeBeforePlay()
        {
            _angle = 0.0f;
            _currentFloatSpeed = 0.0f;
            _positionRestore = _targetInfo.TargetTransform.position;

            _floatSpeed = _parentEffect.FloatEffectSettings.FloatSpeed;
            _altitude = _parentEffect.FloatEffectSettings.Altitude;
            _floatAxis = _parentEffect.FloatEffectSettings.FloatAxis;
            _floatSpace = _parentEffect.FloatEffectSettings.FloatSpace;

            _enterMode = _parentEffect.FloatEffectSettings.EnterSettings.EnterMode;
            _enterDuration = _parentEffect.FloatEffectSettings.EnterSettings.EnterDuration;
            _exitMode = _parentEffect.FloatEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.FloatEffectSettings.ExitSettings.ExitDuration;

            if (_floatSpace == HoverEffectSpace.Global)
            {
                if (_floatAxis == HoverFloatAxis.X) _floatAxisVec = Vector3.right;
                else if (_floatAxis == HoverFloatAxis.Y) _floatAxisVec = Vector3.up;
                else _floatAxisVec = Vector3.forward;
            }
            else
            {
                if (_floatAxis == HoverFloatAxis.X) _floatAxisVec = _targetInfo.TargetTransform.right;
                else if (_floatAxis == HoverFloatAxis.Y) _floatAxisVec = _targetInfo.TargetTransform.up;
                else _floatAxisVec = _targetInfo.TargetTransform.forward;
            }

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            if (_enterMode == HoverEffectEnterMode.Instant) _currentFloatSpeed = _floatSpeed;
            else
            {
                float elapsedTime = 0.0f;
                while (elapsedTime < _enterDuration)
                {
                    float t = elapsedTime / _enterDuration;
                    _currentFloatSpeed = Mathf.Lerp(0.0f, _floatSpeed, t);
                    _angle += Time.deltaTime * _currentFloatSpeed;
                    _angle %= Mathf.PI * 2.0f;
                   
                    _targetInfo.TargetTransform.position = _positionRestore + _floatAxisVec * Mathf.Sin(_angle) * _altitude;
                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                _currentFloatSpeed = _floatSpeed;
                _targetInfo.TargetTransform.position = _positionRestore + _floatAxisVec * Mathf.Sin(_angle) * _altitude;
            }
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant)
            {
                _currentFloatSpeed = _floatSpeed;
                _targetInfo.TargetTransform.position = _positionRestore;
            }
            else
            {
                float elapsedTime = 0.0f;
                float startAltitude = _altitude;
                Vector3 startPosition = _targetInfo.TargetTransform.position;

                while (elapsedTime < _exitDuration)
                {
                    float t = elapsedTime / _exitDuration;
                    _altitude = Mathf.Lerp(startAltitude, 0.0f, t);
                    _angle += Time.deltaTime * _currentFloatSpeed;
                    _angle %= Mathf.PI * 2.0f;

                    _targetInfo.TargetTransform.position = _positionRestore + _floatAxisVec * Mathf.Sin(_angle) * _altitude;

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _altitude = 0.0f;
                _targetInfo.TargetTransform.position = _positionRestore;
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            while (true)
            {
                _angle += Time.deltaTime * _currentFloatSpeed;
                _angle %= Mathf.PI * 2.0f;

                _targetInfo.TargetTransform.position = _positionRestore + _floatAxisVec * Mathf.Sin(_angle) * _altitude;
                yield return null;
            }
        }
    }
}
