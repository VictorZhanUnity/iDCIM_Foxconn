using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class RotationHoverEffectCoroutine : HoverEffectCoroutine
    {
        private ObjectBounds.QueryConfig _boundsQConfig;

        private Quaternion _rotationRestore;
        private Vector3 _rotationAxisVec;
        private float _currentRotationSpeed;

        private float _rotationSpeed;
        private HoverRotationAxis _rotationAxis;
        private HoverEffectSpace _rotationSpace;

        private HoverEffectEnterMode _enterMode;
        private float _enterDuration;
        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        public override HoverEffectType EffectType { get { return HoverEffectType.Rotation; } }

        protected override bool InitializeBeforePlay()
        {
            _rotationRestore = _targetInfo.TargetTransform.rotation;
            _currentRotationSpeed = 0.0f;

            _rotationSpeed = _parentEffect.RotationEffectSettings.RotationSpeed;
            _rotationAxis = _parentEffect.RotationEffectSettings.RotationAxis;
            _rotationSpace = _parentEffect.RotationEffectSettings.RotationSpace;

            _enterMode = _parentEffect.RotationEffectSettings.EnterSettings.EnterMode;
            _enterDuration = _parentEffect.RotationEffectSettings.EnterSettings.EnterDuration;
            _exitMode = _parentEffect.RotationEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.RotationEffectSettings.ExitSettings.ExitDuration;

            if (_rotationSpace == HoverEffectSpace.Global)
            {
                if (_rotationAxis == HoverRotationAxis.X) _rotationAxisVec = Vector3.right;
                else if (_rotationAxis == HoverRotationAxis.Y) _rotationAxisVec = Vector3.up;
                else _rotationAxisVec = Vector3.forward;
            }
            else
            {
                if (_rotationAxis == HoverRotationAxis.X) _rotationAxisVec = _targetInfo.TargetTransform.right;
                else if (_rotationAxis == HoverRotationAxis.Y) _rotationAxisVec = _targetInfo.TargetTransform.up;
                else _rotationAxisVec = _targetInfo.TargetTransform.forward;
            }

            _boundsQConfig = new ObjectBounds.QueryConfig();
            _boundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            _boundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            if (_enterMode == HoverEffectEnterMode.Instant) _currentRotationSpeed = _rotationSpeed;
            else
            if (_enterMode == HoverEffectEnterMode.LinearTransition)
            {
                AABB worldAABB = AABB.GetInvalid();
                float elapsedTime = 0.0f;

                while (elapsedTime < _enterDuration)
                {
                    float t = elapsedTime / _enterDuration;
                    _currentRotationSpeed = Mathf.Lerp(0.0f, _rotationSpeed, t);

                    if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.RotateAround(worldAABB.Center, _rotationAxisVec, _currentRotationSpeed * Time.deltaTime);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _currentRotationSpeed = _rotationSpeed;
            }
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant) _targetInfo.TargetTransform.rotation = _rotationRestore;
            else
            if (_exitMode == HoverEffectExitMode.LinearTransition)
            {
                AABB worldAABB = AABB.GetInvalid();
                Quaternion startRotation = _targetInfo.TargetTransform.rotation;
                float elapsedTime = 0.0f;

                while (elapsedTime < _exitDuration)
                {
                    float t = elapsedTime / _exitDuration;

                    if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.SetRotationAroundPivot(Quaternion.Slerp(startRotation, _rotationRestore, t), worldAABB.Center);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _targetInfo.TargetTransform.rotation = _rotationRestore;
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            AABB worldAABB = AABB.GetInvalid();
            while (true)
            {
                if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                _targetInfo.TargetTransform.RotateAround(worldAABB.Center, _rotationAxisVec, _currentRotationSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}