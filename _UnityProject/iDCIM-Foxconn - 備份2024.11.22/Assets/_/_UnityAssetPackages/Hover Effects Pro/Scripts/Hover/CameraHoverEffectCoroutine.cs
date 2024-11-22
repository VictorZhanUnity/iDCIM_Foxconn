using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public class CameraHoverEffectCoroutine : HoverEffectCoroutine
    {
        private class TargetCamRestoreData
        {
            public Camera TargetCamera;
            public Vector3 CamPositionRestore;
            public float CamOrthoSizeRestore;
            public CameraHoverEffectCoroutine Owner;
        }
        private static List<TargetCamRestoreData> _targetCamRestoreData = new List<TargetCamRestoreData>();

        private TargetCamRestoreData _camRestoreData;
        private float _targetOrthoSize;
        private AABB _focusAABB;
        private CameraHoverEffectFocus.Data _focusData;
        private Transform _cameraTransform;

        private Camera _targetCamera;
        private HoverCameraFocusType _focusType;
        private float _focusDuration;
        private float _smoothValue;

        public override HoverEffectType EffectType { get { return HoverEffectType.Camera; } }

        public void RestoreCamera()
        {
            if (_camRestoreData != null && _camRestoreData.Owner == this)
            {
                if (_targetCamera != null)
                {
                    _cameraTransform.position = _camRestoreData.CamPositionRestore;
                    _targetCamera.orthographicSize = _camRestoreData.CamOrthoSizeRestore;
                }
            }
        }

        protected override bool InitializeBeforePlay()
        {
            _targetCamera = _parentEffect.CameraEffectSettings.TargetCamera;
            if (_targetCamera == null) _targetCamera = Camera.main;
            if (_targetCamera == null)
            {
                Debug.LogError("CameraHoverEffectCoroutine: Missing target camera. Effect can not be played.");
                return false;
            }

            _focusType = _parentEffect.CameraEffectSettings.FocusType;
            _focusDuration = _parentEffect.CameraEffectSettings.FocusDuration;
            _smoothValue = _parentEffect.CameraEffectSettings.SmoothValue;

            _cameraTransform = _targetCamera.transform;

            var boundsQConfig = new ObjectBounds.QueryConfig();
            boundsQConfig.ObjectTypes = GameObjectType.Mesh | GameObjectType.Sprite;
            boundsQConfig.UseSkinLocalBounds = _targetInfo.UseSkinLocalBounds;

            if (_targetInfo.IncludeChildren) _focusAABB = ObjectBounds.CalcHierarchyWorldAABB(_targetInfo.TargetObject, boundsQConfig);
            else _focusAABB = ObjectBounds.CalcWorldAABB(_targetInfo.TargetObject, boundsQConfig);
            if (!_focusAABB.IsValid) return false;

            _focusData = CameraHoverEffectFocus.CalculateFocusData(_targetCamera, _focusAABB, _parentEffect.CameraEffectSettings);
            _targetOrthoSize = 0.5f * _targetCamera.GetFrustumHeightFromDistance(_focusData.FocusPointOffset);

            _camRestoreData = GetTargetCamRestoreData(_targetCamera);
            if (_camRestoreData == null)
            {
                _camRestoreData = new TargetCamRestoreData();
                _camRestoreData.TargetCamera = _targetCamera;
                _camRestoreData.CamPositionRestore = _cameraTransform.position;
                _camRestoreData.CamOrthoSizeRestore = _targetCamera.orthographicSize;
                _targetCamRestoreData.Add(_camRestoreData);
                _camRestoreData.Owner = this;
            }
            else
            {
                if (_camRestoreData.Owner != null)
                    _camRestoreData.Owner.InstantStop();

                _camRestoreData.Owner = this;
            }

            return true;
        }

        protected override IEnumerator EnterEffect()
        {
            yield break;
        }

        protected override IEnumerator ExitEffect()
        {
            if (_focusType == HoverCameraFocusType.Linear)
            {
                float elapsedTime = 0.0f;

                Vector3 startPos = _cameraTransform.position;
                float orthoSize = _targetCamera.orthographicSize;

                while (elapsedTime < _focusDuration)
                {
                    if (_targetCamera == null) break;

                    float t = elapsedTime / _focusDuration;
                    _cameraTransform.position = Vector3.Lerp(startPos, _camRestoreData.CamPositionRestore, t);
                    _targetCamera.orthographicSize = Mathf.Lerp(orthoSize, _camRestoreData.CamOrthoSizeRestore, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                if (_targetCamera != null)
                {
                    _cameraTransform.position = _camRestoreData.CamPositionRestore;
                    _targetCamera.orthographicSize = _camRestoreData.CamOrthoSizeRestore;
                    _camRestoreData.Owner = null;
                    _targetCamRestoreData.Remove(_camRestoreData);
                }
            }
            else
            if (_focusType == HoverCameraFocusType.Smooth)
            {
                while (true)
                {
                    if (_targetCamera == null) break;

                    float t = Time.deltaTime * _smoothValue;
                    _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _camRestoreData.CamPositionRestore, t);
                    _targetCamera.orthographicSize = Mathf.Lerp(_targetCamera.orthographicSize, _camRestoreData.CamOrthoSizeRestore, t);

                    if (Mathf.Abs(_targetCamera.orthographicSize - _camRestoreData.CamOrthoSizeRestore) < 1e-5f)
                    {
                        _cameraTransform.position = _camRestoreData.CamPositionRestore;
                        _targetCamera.orthographicSize = _camRestoreData.CamOrthoSizeRestore;
                        _camRestoreData.Owner = null;
                        _targetCamRestoreData.Remove(_camRestoreData);
                        break;
                    }

                    yield return null;
                }
            }
        }

        protected override IEnumerator ApplyEffect()
        {
            if (_focusType == HoverCameraFocusType.Linear)
            {
                float elapsedTime = 0.0f;
                Vector3 startPos = _cameraTransform.position;
                float orthoSize = _targetCamera.orthographicSize;

                while (elapsedTime < _focusDuration)
                {
                    if (_targetCamera == null) break;

                    float t = elapsedTime / _focusDuration;
                    _cameraTransform.position = Vector3.Lerp(startPos, _focusData.TargetWorldPosition, t);
                    _targetCamera.orthographicSize = Mathf.Lerp(orthoSize, _targetOrthoSize, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                if (_targetCamera != null)
                {
                    _cameraTransform.position = _focusData.TargetWorldPosition;
                    _targetCamera.orthographicSize = _targetOrthoSize;
                    while (true) yield return null;
                }
                else yield break;
            }
            else
            if (_focusType == HoverCameraFocusType.Smooth)
            {
                while (true)
                {
                    if (_targetCamera == null) break;

                    float t = Time.deltaTime * _smoothValue;
                    _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _focusData.TargetWorldPosition, t);
                    _targetCamera.orthographicSize = Mathf.Lerp(_targetCamera.orthographicSize, _targetOrthoSize, t);

                    if (Mathf.Abs(_targetCamera.orthographicSize - _targetOrthoSize) < 1e-5f)
                    {
                        _cameraTransform.position = _focusData.TargetWorldPosition;
                        _targetCamera.orthographicSize = _targetOrthoSize;
                        break;
                    }

                    yield return null;
                }

                if (_targetCamera != null)
                {
                    _cameraTransform.position = _focusData.TargetWorldPosition;
                    _targetCamera.orthographicSize = _targetOrthoSize;
                    while (true) yield return null;
                }
                else yield break;
            }
        }

        protected override void OnPlayEnd()
        {
            if (_camRestoreData.Owner == this)
                _camRestoreData.Owner = null;
        }

        private static TargetCamRestoreData GetTargetCamRestoreData(Camera targetCamera)
        {
            var dataList = _targetCamRestoreData.FindAll(item => item.TargetCamera == targetCamera);
            return dataList.Count != 0 ? dataList[0] : null;
        }
    }
}
