using UnityEngine;
using System.Collections;

namespace HoverEffectsPro
{
    public class PointerHoverEffectCoroutine : HoverEffectCoroutine
    {
        private class TargetBounds
        {
            public OBB OBB;
            public BoxFace Face;
            public Plane FacePlane;
            public Vector3 FaceCenter;
        }

        private float _elapsedTimeScale;
        private float _signScale;

        private Color _pointerColor;
        private float _pointerScaleFactor;
        private PyramidShape3D _pyramid = new PyramidShape3D();
        private ConeShape3D _cone = new ConeShape3D();

        private ObjectBounds.QueryConfig _boundsQConfig;
        private TargetBounds _targetBounds = new TargetBounds();

        private HoverEffectEnterMode _enterMode;
        private float _enterDuration;
        private HoverEffectExitMode _exitMode;
        private float _exitDuration;

        private HoverEffectSpace _pointerSpace;
        private HoverPointerAxis _pointerAxis;
        private HoverPointerType _pointerType;
        private float _pyramidBaseWidth;
        private float _pyramidBaseDepth;
        private float _pyramidHeight;
        private float _coneBaseRadius;
        private float _coneHeight;
        private float _rotationSpeed;
        private float _positionOffset;
        private Color _firstColor;
        private Color _secondColor;
        private float _colorTransitionDuration;
        private HoverShadeMode _shadeMode;
        private float _firstScaleFactor;
        private float _secondScaleFactor;
        private float _scaleTransitionDuration;

        public override HoverEffectType EffectType { get { return HoverEffectType.Pointer; } }

        protected override void OnRender()
        {
            Material material = MaterialPool.Get.GizmoSolidHandle;
            material.SetZTestEnabled(true);
            material.SetZWriteEnabled(true);
            material.SetColor("_Color", _pointerColor);
            material.SetInt("_IsLit", _shadeMode == HoverShadeMode.Lit ? 1 : 0);
            material.SetVector("_LightDir", Camera.main.transform.forward);
            material.SetFloat("_LightIntensity", 1.23f);
            material.SetPass(0);

            if (_pointerType == HoverPointerType.Cone) _cone.RenderSolid();
            else if (_pointerType == HoverPointerType.Pyramid) _pyramid.RenderSolid();
        }

        protected override bool InitializeBeforePlay()
        {
            _enterMode = _parentEffect.PointerEffectSettings.EnterSettings.EnterMode;
            _enterDuration = _parentEffect.PointerEffectSettings.EnterSettings.EnterDuration;
            _exitMode = _parentEffect.PointerEffectSettings.ExitSettings.ExitMode;
            _exitDuration = _parentEffect.PointerEffectSettings.ExitSettings.ExitDuration;

            _pointerSpace = _parentEffect.PointerEffectSettings.PointerSpace;
            _pointerAxis = _parentEffect.PointerEffectSettings.PointerAxis;
            _pointerType = _parentEffect.PointerEffectSettings.PointerType;
            _pyramidBaseWidth = _parentEffect.PointerEffectSettings.PyramidBaseWidth;
            _pyramidBaseDepth = _parentEffect.PointerEffectSettings.PyramidBaseDepth;
            _pyramidHeight = _parentEffect.PointerEffectSettings.PyramidHeight;
            _coneBaseRadius = _parentEffect.PointerEffectSettings.ConeBaseRadius;
            _coneHeight = _parentEffect.PointerEffectSettings.ConeHeight;
            _rotationSpeed = _parentEffect.PointerEffectSettings.RotationSpeed;
            _positionOffset = _parentEffect.PointerEffectSettings.PositionOffset;
            _firstColor = _parentEffect.PointerEffectSettings.FirstColor;
            _secondColor = _parentEffect.PointerEffectSettings.SecondColor;
            _colorTransitionDuration = _parentEffect.PointerEffectSettings.ColorTransitionDuration;
            _shadeMode = _parentEffect.PointerEffectSettings.ShadeMode;
            _firstScaleFactor = _parentEffect.PointerEffectSettings.FirstScaleFactor;
            _secondScaleFactor = _parentEffect.PointerEffectSettings.SecondScaleFactor;
            _scaleTransitionDuration = _parentEffect.PointerEffectSettings.ScaleTransitionDuration;

            _boundsQConfig = new ObjectBounds.QueryConfig();
            _boundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            _boundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            _pointerScaleFactor = 1.0f;
            _pointerColor = _firstColor;
            _elapsedTimeScale = 0.0f;
            _signScale = 1.0f;

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            if (_enterMode == HoverEffectEnterMode.Instant)
            {
                _pointerColor = _firstColor;

                UpdateTargetBounds();
                UpdatePointerTransform();
            }
            else
            if (_enterMode == HoverEffectEnterMode.LinearTransition)
            {
                float elapsedTimeColor = 0.0f;
                Color startColor = _firstColor.KeepAllButAlpha(0.0f);

                while (elapsedTimeColor < _enterDuration)
                {
                    float t = elapsedTimeColor / _enterDuration;
                    _pointerColor = Color.Lerp(startColor, _firstColor, t);
                    elapsedTimeColor += Time.deltaTime;

                    t = _elapsedTimeScale / _scaleTransitionDuration;
                    _pointerScaleFactor = Mathf.Lerp(_firstScaleFactor, _secondScaleFactor, t);

                    UpdateTargetBounds();
                    UpdatePointerTransform();

                    _elapsedTimeScale += Time.deltaTime * _signScale;
                    if (_elapsedTimeScale >= _scaleTransitionDuration)
                    {
                        _elapsedTimeScale = _scaleTransitionDuration;
                        _signScale = -1.0f;
                        _pointerScaleFactor = _secondScaleFactor;
                    }
                    else
                    if (_elapsedTimeScale <= 0.0f)
                    {
                        _elapsedTimeScale = 0.0f;
                        _signScale = 1.0f;
                        _pointerScaleFactor = _firstScaleFactor;
                    }

                    yield return null;
                }

                _pointerColor = _firstColor;
                _pointerScaleFactor = _firstScaleFactor;
            }
        }

        protected override IEnumerator ExitEffect()
        {
            if (_exitMode == HoverEffectExitMode.Instant)
            {
                _pointerColor = _pointerColor.KeepAllButAlpha(0.0f);
            }
            else
            if (_exitMode == HoverEffectExitMode.LinearTransition)
            {
                float elapsedTimeColor = 0.0f;
                Color startColor = _pointerColor;
                Color targetColor = _pointerColor.KeepAllButAlpha(0.0f);

                while (elapsedTimeColor < _exitDuration)
                {
                    float t = elapsedTimeColor / _exitDuration;
                    _pointerColor = Color.Lerp(startColor, targetColor, t);
                    elapsedTimeColor += Time.deltaTime;

                    t = _elapsedTimeScale / _scaleTransitionDuration;
                    _pointerScaleFactor = Mathf.Lerp(_firstScaleFactor, _secondScaleFactor, t);

                    UpdateTargetBounds();
                    UpdatePointerTransform();

                    _elapsedTimeScale += Time.deltaTime * _signScale;
                    if (_elapsedTimeScale >= _scaleTransitionDuration)
                    {
                        _elapsedTimeScale = _scaleTransitionDuration;
                        _signScale = -1.0f;
                        _pointerScaleFactor = _secondScaleFactor;
                    }
                    else
                    if (_elapsedTimeScale <= 0.0f)
                    {
                        _elapsedTimeScale = 0.0f;
                        _signScale = 1.0f;
                        _pointerScaleFactor = _secondScaleFactor;
                    }

                    yield return null;
                }

                _pointerColor = targetColor;
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            float elapsedTimeColor = 0.0f;
            float signColor = 1.0f;

            while (true)
            {
                float t = elapsedTimeColor / _colorTransitionDuration;
                _pointerColor = Color.Lerp(_firstColor, _secondColor, t);

                t = _elapsedTimeScale / _scaleTransitionDuration;
                _pointerScaleFactor = Mathf.Lerp(_firstScaleFactor, _secondScaleFactor, t);

                UpdateTargetBounds();
                UpdatePointerTransform();

                elapsedTimeColor += Time.deltaTime * signColor;
                if (elapsedTimeColor >= _colorTransitionDuration)
                {
                    elapsedTimeColor = _colorTransitionDuration;
                    signColor = -1.0f;
                    _pointerColor = _secondColor;
                }
                else
                if (elapsedTimeColor <= 0.0f)
                {
                    elapsedTimeColor = 0.0f;
                    signColor = 1.0f;
                    _pointerColor = _firstColor;
                }

                _elapsedTimeScale += Time.deltaTime * _signScale;
                if (_elapsedTimeScale >= _scaleTransitionDuration)
                {
                    _elapsedTimeScale = _scaleTransitionDuration;
                    _signScale = -1.0f;
                    _pointerScaleFactor = _secondScaleFactor;
                }
                else
                if (_elapsedTimeScale <= 0.0f)
                {
                    _elapsedTimeScale = 0.0f;
                    _signScale = 1.0f;
                    _pointerScaleFactor = _secondScaleFactor;
                }

                yield return null;
            }
        }

        private void UpdatePointerTransform()
        {
            Vector3 faceNormal = _targetBounds.FacePlane.normal;
            Vector3 tipPosition = _targetBounds.FaceCenter + faceNormal * _positionOffset;

            if (_pointerType == HoverPointerType.Cone)
            {
                _cone.BaseRadius = _coneBaseRadius * _pointerScaleFactor;
                _cone.Height = _coneHeight * _pointerScaleFactor;

                _cone.AlignTip(-faceNormal);
                _cone.Tip = tipPosition;

                _cone.Rotation = _cone.Rotation * Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, _targetBounds.FacePlane.normal);
            }
            else if (_pointerType == HoverPointerType.Pyramid)
            {
                _pyramid.BaseWidth = _pyramidBaseWidth * _pointerScaleFactor;
                _pyramid.BaseDepth = _pyramidBaseDepth * _pointerScaleFactor;
                _pyramid.Height = _pyramidHeight * _pointerScaleFactor;

                _pyramid.AlignTip(-faceNormal);
                _pyramid.Tip = tipPosition;

                _pyramid.Rotation = _pyramid.Rotation * Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, _targetBounds.FacePlane.normal);
            }
        }

        private void UpdateTargetBounds()
        {
            if (_pointerSpace == HoverEffectSpace.Global)
            {
                AABB worldAABB = AABB.GetInvalid();
                if (_targetInfo.IncludeChildren) worldAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, _boundsQConfig);
                else worldAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, _boundsQConfig);

                if (worldAABB.IsValid) _targetBounds.OBB = new OBB(worldAABB.Center, worldAABB.Size, Quaternion.identity);
                else _targetBounds.OBB = OBB.GetInvalid();
                if (!_targetBounds.OBB.IsValid) return;

                if (_pointerAxis == HoverPointerAxis.Up) _targetBounds.Face = BoxFace.Top;
                else if (_pointerAxis == HoverPointerAxis.Down) _targetBounds.Face = BoxFace.Bottom;
                else if (_pointerAxis == HoverPointerAxis.Right) _targetBounds.Face = BoxFace.Right;
                else if (_pointerAxis == HoverPointerAxis.Left) _targetBounds.Face = BoxFace.Left;
                else if (_pointerAxis == HoverPointerAxis.Forward) _targetBounds.Face = BoxFace.Back;
                else _targetBounds.Face = BoxFace.Front;

                BoxFaceDesc faceDesc = BoxMath.GetFaceDesc(_targetBounds.Face, _targetBounds.OBB.Center, _targetBounds.OBB.Size, _targetBounds.OBB.Rotation);
                _targetBounds.FacePlane = faceDesc.Plane;
                _targetBounds.FaceCenter = faceDesc.Center;
            }
            else
            if (_pointerSpace == HoverEffectSpace.Local)
            {
                OBB worldOBB = OBB.GetInvalid();
                if (_targetInfo.IncludeChildren) worldOBB = ObjectBounds.CalcHierarchyWorldOBB(_targetInfo.TargetObject, _boundsQConfig);
                else worldOBB = ObjectBounds.CalcWorldOBB(_targetInfo.TargetObject, _boundsQConfig);

                _targetBounds.OBB = worldOBB;
                if (!_targetBounds.OBB.IsValid) return;

                if (_pointerAxis == HoverPointerAxis.Up) _targetBounds.Face = BoxFace.Top;
                else if (_pointerAxis == HoverPointerAxis.Down) _targetBounds.Face = BoxFace.Bottom;
                else if (_pointerAxis == HoverPointerAxis.Right) _targetBounds.Face = BoxFace.Right;
                else if (_pointerAxis == HoverPointerAxis.Left) _targetBounds.Face = BoxFace.Left;
                else if (_pointerAxis == HoverPointerAxis.Forward) _targetBounds.Face = BoxFace.Back;
                else _targetBounds.Face = BoxFace.Front;

                BoxFaceDesc faceDesc = BoxMath.GetFaceDesc(_targetBounds.Face, _targetBounds.OBB.Center, _targetBounds.OBB.Size, _targetBounds.OBB.Rotation);
                _targetBounds.FacePlane = faceDesc.Plane;
                _targetBounds.FaceCenter = faceDesc.Center;
            }
        }
    }
}
