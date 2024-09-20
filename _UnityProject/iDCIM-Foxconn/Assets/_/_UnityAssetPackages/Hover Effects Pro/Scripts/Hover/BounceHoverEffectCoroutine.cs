using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class BounceHoverEffectCoroutine : HoverEffectCoroutine
    {
        private enum BounceState
        {
            Jumping = 1,
            Falling,
            SquashEnter,
            SquashExit
        }

        private BounceState _bounceState;
        private Plane _landingPlane;
        private Vector3 _bounceAxisVec;
        private float _currentJumpSpeed;
        private float _currentFallSpeed;
        private int _squashAxis;

        private Vector3 _positionRestore;
        private Quaternion _rotationRestore;
        private Vector3 _localScaleRestore;

        private HoverEffectSpace _bounceSpace;
        private HoverBounceAxis _bounceAxis;
        private bool _rotateOnJump;
        private float _jumpSpeed;
        private float _jumpFriction;
        private float _fallSpeed;
        private float _gravity;
        private float _rotationSpeed;
        private bool _squashOnLanding;
        private bool _instantSquashEnter;
        private bool _instantSquashExit;
        private float _squashTransitionDuration;
        private float _squashFactor;

        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private ObjectBounds.QueryConfig _boundsQConfig;

        public override HoverEffectType EffectType { get { return HoverEffectType.Bounce; } }

        protected override bool InitializeBeforePlay()
        {
            _rotateOnJump = _parentEffect.BounceEffectSettings.RotateOnJump;
            _bounceSpace = _parentEffect.BounceEffectSettings.BounceSpace;
            _bounceAxis = _parentEffect.BounceEffectSettings.BounceAxis;
            _jumpSpeed = _parentEffect.BounceEffectSettings.JumpSpeed;
            _fallSpeed = _parentEffect.BounceEffectSettings.FallSpeed;
            _jumpFriction = _parentEffect.BounceEffectSettings.JumpFriction;
            _gravity = _parentEffect.BounceEffectSettings.Gravity;
            _rotationSpeed = _parentEffect.BounceEffectSettings.RotationSpeed;
            _squashOnLanding = _parentEffect.BounceEffectSettings.SquashOnLanding;
            _squashFactor = _parentEffect.BounceEffectSettings.SquashFactor;
            _instantSquashEnter = _parentEffect.BounceEffectSettings.InstantSquashEnter;
            _instantSquashExit = _parentEffect.BounceEffectSettings.InstantSquashExit;
            _squashTransitionDuration = _parentEffect.BounceEffectSettings.SquashTransitionDuration;

            _exitMode = _parentEffect.BounceEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.BounceEffectSettings.ExitSettings.ExitDuration;

            _positionRestore = _targetInfo.TargetTransform.position;
            _rotationRestore = _targetInfo.TargetTransform.rotation;
            _localScaleRestore = _targetInfo.TargetTransform.localScale;

            CalculateBounceAndSquashAxis();
            CalculateLandingPlane();

            _bounceState = BounceState.Jumping;
            _currentJumpSpeed = _jumpSpeed;

            _boundsQConfig = new ObjectBounds.QueryConfig();
            _boundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            _boundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            yield break;
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.LinearTransition)
            {
                Vector3 startPosition = _targetInfo.TargetTransform.position;
                Quaternion startRotation = _targetInfo.TargetTransform.rotation;
                Vector3 startScale = _targetInfo.TargetTransform.localScale;

                AABB worldAABB = AABB.GetInvalid();
                float elapsedTime = 0.0f;
                while (elapsedTime < _exitDuration)
                {
                    float t = elapsedTime / _exitDuration;
                    _targetInfo.TargetTransform.position = Vector3.Lerp(startPosition, _positionRestore, t);

                    if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                    _targetInfo.TargetTransform.SetRotationAroundPivot(Quaternion.Slerp(startRotation, _rotationRestore, t), worldAABB.Center);

                    _targetInfo.TargetTransform.localScale = Vector3.Lerp(startScale, _localScaleRestore, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            _targetInfo.TargetTransform.position = _positionRestore;
            _targetInfo.TargetTransform.rotation = _rotationRestore;
            _targetInfo.TargetTransform.localScale = _localScaleRestore;
        }

        protected override IEnumerator ApplyEffect()
        {
            AABB worldAABB = AABB.GetInvalid();
            float currentSquashFactor = 0.0f;

            while (true)
            {
                if (_bounceState == BounceState.Jumping)
                {               
                    _targetInfo.TargetTransform.position += _bounceAxisVec * _currentJumpSpeed * Time.deltaTime;
                    _currentJumpSpeed -= _jumpFriction * Time.deltaTime;

                    if (_rotateOnJump)
                    {
                        if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                        else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);

                        float rotationSpeed = _rotationSpeed * _currentJumpSpeed / _jumpSpeed;
                        _targetInfo.TargetTransform.RotateAround(worldAABB.Center, _bounceAxisVec, rotationSpeed * Time.deltaTime);
                    }
                
                    if (_currentJumpSpeed <= 0.0f)
                    {
                        _currentJumpSpeed = 0.0f;
                        _currentFallSpeed = _fallSpeed;
                        _bounceState = BounceState.Falling;
                    }
                }
                else
                if (_bounceState == BounceState.Falling)
                {
                    _targetInfo.TargetTransform.position -= _bounceAxisVec * _currentFallSpeed * Time.deltaTime;
                    _currentFallSpeed += _gravity * Time.deltaTime;

                    float d = _landingPlane.GetDistanceToPoint(_targetInfo.TargetTransform.position);
                    if (d <= 0.0f)
                    {
                        _targetInfo.TargetTransform.position = _landingPlane.ProjectPoint(_targetInfo.TargetTransform.position);
                        _currentFallSpeed = 0.0f;
                        _currentJumpSpeed = _jumpSpeed;
                        d = 0.0f;

                        if (_squashOnLanding)
                        {
                            _bounceState = BounceState.SquashEnter;
                            currentSquashFactor = 0.0f;
                        }
                        else _bounceState = BounceState.Jumping;
                    }
                }
                else
                if (_bounceState == BounceState.SquashEnter)
                {
                    if (_instantSquashEnter) 
                    {
                        float scaleFactor = 1.0f - _squashFactor;
                        Vector3 newScale = _localScaleRestore;
                        newScale[_squashAxis] *= scaleFactor;
                        _targetInfo.TargetTransform.localScale = newScale;
                    }
                    else
                    {
                        float elapsedTime = 0.0f;
                        while (elapsedTime < _squashTransitionDuration)
                        {
                            currentSquashFactor = Mathf.Lerp(0.0f, _squashFactor, elapsedTime / _squashTransitionDuration);
                            elapsedTime += Time.deltaTime;

                            float scaleFactor = 1.0f - currentSquashFactor;
                            Vector3 newScale = _localScaleRestore;
                            newScale[_squashAxis] *= scaleFactor;
                            _targetInfo.TargetTransform.localScale = newScale;

                            yield return null;
                        }
                    }
   
                    _bounceState = BounceState.SquashExit;
                }
                else
                if (_bounceState == BounceState.SquashExit)
                {
                    if (_instantSquashExit) _targetInfo.TargetTransform.localScale = _localScaleRestore;
                    else
                    {
                        float elapsedTime = 0.0f;
                        while (elapsedTime < _squashTransitionDuration)
                        {
                            currentSquashFactor = Mathf.Lerp(_squashFactor, 0.0f, elapsedTime / _squashTransitionDuration);
                            elapsedTime += Time.deltaTime;

                            float scaleFactor = 1.0f - currentSquashFactor;

                            Vector3 newScale = _localScaleRestore;
                            newScale[_squashAxis] *= scaleFactor;
                            _targetInfo.TargetTransform.localScale = newScale;

                            yield return null;
                        }

                        _targetInfo.TargetTransform.localScale = _localScaleRestore;
                    }
                    _bounceState = BounceState.Jumping;
                }

                yield return null;
            }
        }

        private void CalculateBounceAndSquashAxis()
        {
            if (_bounceAxis == HoverBounceAxis.Up)
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? _targetInfo.TargetTransform.up : Vector3.up;
                _squashAxis = 1;
            }
            else if (_bounceAxis == HoverBounceAxis.Down)
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? -_targetInfo.TargetTransform.up : -Vector3.up;
                _squashAxis = 1;
            }
            else if (_bounceAxis == HoverBounceAxis.Right)
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? _targetInfo.TargetTransform.right : Vector3.right;
                _squashAxis = 0;
            }
            else if (_bounceAxis == HoverBounceAxis.Left)
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? -_targetInfo.TargetTransform.right : -Vector3.right;
                _squashAxis = 0;
            }
            else if (_bounceAxis == HoverBounceAxis.Forward)
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? _targetInfo.TargetTransform.forward : Vector3.forward;
                _squashAxis = 2;
            }
            else
            {
                _bounceAxisVec = _bounceSpace == HoverEffectSpace.Local ? -_targetInfo.TargetTransform.forward : -Vector3.forward;
                _squashAxis = 2;
            }
        }

        private void CalculateLandingPlane()
        {
            _landingPlane = new Plane(_bounceAxisVec, _positionRestore);
        }
    }
}
