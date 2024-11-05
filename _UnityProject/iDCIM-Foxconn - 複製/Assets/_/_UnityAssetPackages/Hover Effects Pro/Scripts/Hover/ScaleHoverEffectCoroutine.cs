using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class ScaleHoverEffectCoroutine : HoverEffectCoroutine
    {
        private Vector3 _localScaleRestore;
        private float _currentScaleFactor;

        private float _elapsedTimeScale;
        private float _signScale;

        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private HoverScalePivot _scalePivot;
        private float _firstScaleFactor;
        private float _secondScaleFactor;
        private float _transitionDuration;
        private bool _factorsAreSettled;

        private ObjectBounds.QueryConfig _boundsQConfig = new ObjectBounds.QueryConfig();

        public override HoverEffectType EffectType { get { return HoverEffectType.Scale; } }

        protected override bool InitializeBeforePlay()
        {
            _boundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            _boundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            _localScaleRestore = _targetInfo.TargetTransform.localScale;
            _transitionDuration = _parentEffect.ScaleEffectSettings.TransitionDuration;
            _scalePivot = _parentEffect.ScaleEffectSettings.ScalePivot;

            _exitMode = _parentEffect.ScaleEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.ScaleEffectSettings.ExitSettings.ExitDuration;

            _firstScaleFactor = 1.0f;
            _secondScaleFactor = _parentEffect.ScaleEffectSettings.FirstScaleFactor;

            _elapsedTimeScale = 0.0f;
            _signScale = 1.0f;
            _factorsAreSettled = false;

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            yield break;
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant)
            {
                _currentScaleFactor = 1.0f;

                if (_scalePivot == HoverScalePivot.MeshPivot) _targetInfo.TargetTransform.localScale = _localScaleRestore;
                else
                {
                    AABB worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.SetLocalScaleFromPivot(_localScaleRestore, worldAABB.Center);
                }
            }
            else
            if (_exitMode == HoverEffectExitMode.LinearTransition)
            {
                AABB worldAABB;
                float startFactor = _currentScaleFactor;
                float elapsedTime = 0.0f;

                while (elapsedTime < _exitDuration)
                {
                    float t = elapsedTime / _exitDuration;
                    _currentScaleFactor = Mathf.Lerp(startFactor, 1.0f, t);

                    if (_scalePivot == HoverScalePivot.Center)
                    {
                        worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                        _targetInfo.TargetTransform.SetLocalScaleFromPivot(_localScaleRestore * _currentScaleFactor, worldAABB.Center);
                    }
                    else _targetInfo.TargetTransform.localScale = _localScaleRestore * _currentScaleFactor;
               
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _currentScaleFactor = 1.0f;

                if (_scalePivot == HoverScalePivot.Center)
                {
                    worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.SetLocalScaleFromPivot(_localScaleRestore, worldAABB.Center);
                }
                else _targetInfo.TargetTransform.localScale = _localScaleRestore;
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            AABB worldAABB;
            while (true)
            {
                float t = _elapsedTimeScale / _transitionDuration;
                _currentScaleFactor = Mathf.Lerp(_firstScaleFactor, _secondScaleFactor, t);

                if (_scalePivot == HoverScalePivot.Center)
                {
                    worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.SetLocalScaleFromPivot(_localScaleRestore * _currentScaleFactor, worldAABB.Center);
                }
                else _targetInfo.TargetTransform.localScale = _localScaleRestore * _currentScaleFactor;

                _elapsedTimeScale += Time.deltaTime * _signScale;
                if (_elapsedTimeScale >= _transitionDuration)
                {
                    if (!_factorsAreSettled)
                    {
                        _firstScaleFactor = _parentEffect.ScaleEffectSettings.FirstScaleFactor;
                        _secondScaleFactor = _parentEffect.ScaleEffectSettings.SecondScaleFactor;
                        _elapsedTimeScale %= _transitionDuration;
                        _currentScaleFactor = _firstScaleFactor;
                        _factorsAreSettled = true;
                    }
                    else
                    {
                        _signScale = -1.0f;
                        _elapsedTimeScale = _transitionDuration;
                        _currentScaleFactor = _secondScaleFactor;
                    }
                }
                else
                if (_elapsedTimeScale <= 0.0f)
                {
                    _signScale = 1.0f;
                    _elapsedTimeScale = 0.0f;
                    _currentScaleFactor = _firstScaleFactor;
                }

                yield return null;
            }
        }
    }
}
