#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1)
#define INPUT_MOBILE
#else
#define INPUT_MOUSE
#endif

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace HoverEffectsPro
{
    public class HoverEffect : MonoBehaviour
    {
        public delegate void CanPlayHandler(HoverEffect effect, YesNoAnswer answer);
        public delegate void PlayBeginHandler(HoverEffectType effectType);
        public delegate void PlayEndHandler(HoverEffectType effectType);

        public event CanPlayHandler CanPlay;
        public event PlayBeginHandler PlayBegin;
        public event PlayEndHandler PlayEnd;

        private ColorHoverEffectCoroutine _colorEffectCrtn = new ColorHoverEffectCoroutine();
        private RotationHoverEffectCoroutine _rotationEffectCrtn = new RotationHoverEffectCoroutine();
        private ScaleHoverEffectCoroutine _scaleEffectCrtn = new ScaleHoverEffectCoroutine();
        private FloatHoverEffectCoroutine _floatEffectCrtn = new FloatHoverEffectCoroutine();
        private BoxHoverEffectCoroutine _boxEffectCrtn = new BoxHoverEffectCoroutine();
        private CameraHoverEffectCoroutine _cameraEffectCrtn = new CameraHoverEffectCoroutine();
        private PointerHoverEffectCoroutine _pointerEffectCrtn = new PointerHoverEffectCoroutine();
        private BounceHoverEffectCoroutine _bounceEffectCrtn = new BounceHoverEffectCoroutine();
        private List<HoverEffectCoroutine> _allEffectCrtns = new List<HoverEffectCoroutine>();

        private List<HoverEffectCoroutine> _effectsToPlay = new List<HoverEffectCoroutine>();
        private HoverEffectTargetInfo _targetInfo = new HoverEffectTargetInfo();

        [SerializeField]
        private bool _canPlayHoverEffects = true;
        [SerializeField]
        private bool[] _playableEffectFlags = new bool[8]
        {
            false, false, false, false, true, false, false, false
        };
        [SerializeField]
        private bool _includeChildren = false;
        [SerializeField]
        private bool _useSkinLocalBounds = true;
        [SerializeField]
        private Collider _hoverCollider3D;
        [SerializeField]
        private ColorHoverEffectSettings _colorEffectSettings = new ColorHoverEffectSettings();
        [SerializeField]
        private RotationHoverEffectSettings _rotationEffectSettings = new RotationHoverEffectSettings();
        [SerializeField]
        private ScaleHoverEffectSettings _scaleEffectSettings = new ScaleHoverEffectSettings();
        [SerializeField]
        private FloatHoverEffectSettings _floatEffectSettings = new FloatHoverEffectSettings();
        [SerializeField]
        private BoxHoverEffectSettings _boxEffectSettings = new BoxHoverEffectSettings();
        [SerializeField]
        private CameraHoverEffectSettings _cameraEffectSettings = new CameraHoverEffectSettings();
        [SerializeField]
        private PointerHoverEffectSettings _pointerEffectSettings = new PointerHoverEffectSettings();
        [SerializeField]
        private BounceHoverEffectSettings _bounceEffectSettings = new BounceHoverEffectSettings();

        public bool CanPlayHoverEffects { get { return _canPlayHoverEffects; } set { _canPlayHoverEffects = value; } }
        public bool IncludeChildren { get { return _includeChildren; } set { _includeChildren = value; } }
        public bool UseSkinLocalBounds { get { return _useSkinLocalBounds; } set { _useSkinLocalBounds = value; } }
        public Collider HoverCollider3D { get { return _hoverCollider3D; } set { if (value != null) _hoverCollider3D = value; } }
        public ColorHoverEffectSettings ColorEffectSettings { get { return _colorEffectSettings; } }
        public RotationHoverEffectSettings RotationEffectSettings { get { return _rotationEffectSettings; } }
        public ScaleHoverEffectSettings ScaleEffectSettings { get { return _scaleEffectSettings; } }
        public FloatHoverEffectSettings FloatEffectSettings { get { return _floatEffectSettings; } }
        public BoxHoverEffectSettings BoxEffectSettings { get { return _boxEffectSettings; } }
        public CameraHoverEffectSettings CameraEffectSettings { get { return _cameraEffectSettings; } }
        public PointerHoverEffectSettings PointerEffectSettings { get { return _pointerEffectSettings; } }
        public BounceHoverEffectSettings BounceEffectSettings { get { return _bounceEffectSettings; } }

        public bool IsPlayingColor { get { return _colorEffectCrtn.IsPlaying; } }
        public bool IsEnteringColor { get { return _colorEffectCrtn.IsEntering; } }
        public bool IsExitingColor { get { return _colorEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags ColorEffectStateFlags { get { return _colorEffectCrtn.StateFlags; } }

        public bool IsPlayingRotation { get { return _rotationEffectCrtn.IsPlaying; } }
        public bool IsEnteringRotation { get { return _rotationEffectCrtn.IsEntering; } }
        public bool IsExitingRotation { get { return _rotationEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags RotationEffectStateFlags { get { return _rotationEffectCrtn.StateFlags; } }

        public bool IsPlayingScale { get { return _scaleEffectCrtn.IsPlaying; } }
        public bool IsEnteringScale { get { return _scaleEffectCrtn.IsEntering; } }
        public bool IsExitingScale { get { return _scaleEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags ScaleEffectStateFlags { get { return _scaleEffectCrtn.StateFlags; } }

        public bool IsPlayingFloat { get { return _floatEffectCrtn.IsPlaying; } }
        public bool IsEnteringFloat { get { return _floatEffectCrtn.IsEntering; } }
        public bool IsExitingFloat { get { return _floatEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags FloatEffectStateFlags { get { return _floatEffectCrtn.StateFlags; } }

        public bool IsPlayingBox { get { return _boxEffectCrtn.IsPlaying; } }
        public bool IsEnteringBox { get { return _boxEffectCrtn.IsEntering; } }
        public bool IsExitingBox { get { return _boxEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags BoxEffectStateFlags { get { return _boxEffectCrtn.StateFlags; } }

        public bool IsPlayingCamera { get { return _cameraEffectCrtn.IsPlaying; } }
        public bool IsEnteringCamera { get { return _cameraEffectCrtn.IsEntering; } }
        public bool IsExitingCamera { get { return _cameraEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags CameraEffectStateFlags { get { return _cameraEffectCrtn.StateFlags; } }

        public bool IsPlayingPointer { get { return _pointerEffectCrtn.IsPlaying; } }
        public bool IsEnteringPointer { get { return _pointerEffectCrtn.IsEntering; } }
        public bool IsExitingPointer { get { return _pointerEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags PointerEffectStateFlags { get { return _pointerEffectCrtn.StateFlags; } }

        public bool IsPlayingBounce { get { return _bounceEffectCrtn.IsPlaying; } }
        public bool IsEnteringBounce { get { return _bounceEffectCrtn.IsEntering; } }
        public bool IsExitingBounce { get { return _bounceEffectCrtn.IsExiting; } }
        public HoverEffectStateFlags BounceEffectStateFlags { get { return _bounceEffectCrtn.StateFlags; } }

        public bool IsPlayingAnyEffect { get { return IsPlayingColor || IsPlayingRotation || 
                                                        IsPlayingScale || IsPlayingFloat ||
                                                        IsPlayingBox || IsPlayingCamera || 
                                                        IsPlayingPointer || IsPlayingBounce; } }

        public bool CanPlayEffect(HoverEffectType effectType)
        {
            return _playableEffectFlags[(int)effectType];
        }

        public void SetCanPlayEffect(HoverEffectType effectType, bool canPlay)
        {
            _playableEffectFlags[(int)effectType] = canPlay;
        }

        public void SetCanPlayAll(bool canPlay)
        {
            for(int flagIndex = 0; flagIndex < _playableEffectFlags.Length; ++flagIndex)
                _playableEffectFlags[flagIndex] = canPlay;
        }

        public void Stop()
        {
            foreach (var effectCrtn in _allEffectCrtns)
                effectCrtn.Stop();
        }

        public void InstantStop()
        {
            if (_cameraEffectCrtn.IsPlaying)
                _cameraEffectCrtn.RestoreCamera();

            foreach (var effectCrtn in _allEffectCrtns)
                effectCrtn.InstantStop();
        }

        private void Awake()
        {
            if (_hoverCollider3D == null)
                _hoverCollider3D = GetComponent<Collider>();

            RegisterEffectCrtn(_colorEffectCrtn);
            RegisterEffectCrtn(_rotationEffectCrtn);
            RegisterEffectCrtn(_scaleEffectCrtn);
            RegisterEffectCrtn(_floatEffectCrtn);
            RegisterEffectCrtn(_boxEffectCrtn);
            RegisterEffectCrtn(_cameraEffectCrtn);
            RegisterEffectCrtn(_pointerEffectCrtn);
            RegisterEffectCrtn(_bounceEffectCrtn);
        }

        private void Update()
        {
            if (_hoverCollider3D != null)
            {
                Ray ray = GetHoverRay();

                RaycastHit hitInfo3D;
                if (CanPlayHoverEffects && _hoverCollider3D.Raycast(ray, out hitInfo3D, float.MaxValue))
                {
                    if (CanPlay != null)
                    {
                        YesNoAnswer answer = new YesNoAnswer();
                        CanPlay(this, answer);
                        if (answer.HasNo) return;
                    }

                    QueueAll();

                    if (!IsPlayingAnyEffect)
                        OnNoEffectPlaying();
                }
                else
                {
                    foreach (var effectCrtn in _allEffectCrtns)
                    {
                        // When a mouse device is used, we only stop the camera effect when the right mouse button is clicked
                        #if INPUT_MOUSE
                        if (effectCrtn != _cameraEffectCrtn)
                        #endif
                            effectCrtn.Stop();
                    }
                }

                // Stop the camera effect from playing if the right mouse button is pressed
                #if INPUT_MOUSE
                if (Input.GetMouseButtonDown(1) && IsPlayingCamera)
                    _cameraEffectCrtn.Stop();
                #endif
            }
        }

        private void QueueAll()
        {
            _effectsToPlay.Clear();
            if (CanPlayEffect(HoverEffectType.Color))
            {
                if (IsExitingColor) _colorEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_colorEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Rotation))
            {
                if (IsExitingRotation) _rotationEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_rotationEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Scale))
            {
                if (IsExitingScale) _scaleEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_scaleEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Float))
            {
                if (IsExitingFloat) _floatEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_floatEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Box))
            {
                if (IsExitingBox) _boxEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_boxEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Camera))
            {
                #if INPUT_MOBILE
                if (IsExitingCamera) _cameraEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_cameraEffectCrtn);
                #else
                if (Input.GetMouseButtonDown(0))
                {
                    if (!IsPlayingCamera || IsExitingCamera)
                    {
                        _cameraEffectCrtn.InstantStop();
                        _effectsToPlay.Add(_cameraEffectCrtn);
                    }
                }
                #endif
            }
            if (CanPlayEffect(HoverEffectType.Pointer))
            {
                if (IsExitingPointer) _pointerEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_pointerEffectCrtn);
            }
            if (CanPlayEffect(HoverEffectType.Bounce))
            {
                if (IsExitingBounce) _bounceEffectCrtn.InstantStop();
                else _effectsToPlay.Add(_bounceEffectCrtn);
            }

            if (_effectsToPlay.Count != 0)
            {
                PrepareForEffectCrtnPlay();
                foreach (var effect in _effectsToPlay)
                    effect.Play(this, _targetInfo);
            }
        }

        private bool PrepareForEffectCrtnPlay()
        {
            if (_targetInfo.TargetObject == null)
            {
                Transform thisTransform = gameObject.transform;
                _targetInfo.TargetObject = GameObject.Instantiate(gameObject, thisTransform.position, thisTransform.rotation, thisTransform.parent);
                _targetInfo.TargetTransform = _targetInfo.TargetObject.transform;
                _targetInfo.TargetTransform.localScale = thisTransform.lossyScale;
                _targetInfo.TargetObject.DestroyComponents<HoverEffect>();
            }

            _targetInfo.UseSkinLocalBounds = UseSkinLocalBounds;
            _targetInfo.IncludeChildren = IncludeChildren;
            if (IncludeChildren)
            {
                if (_targetInfo.TargetObject != null)
                {
                    _targetInfo.TargetRenderers = new List<Renderer>(_targetInfo.TargetObject.GetComponentsInChildren<Renderer>());

                    var renderers = gameObject.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                        renderer.enabled = false;
                }
            }
            else
            {
                if (_targetInfo.TargetObject != null)
                {
                    _targetInfo.TargetRenderers = new List<Renderer>();
                    var targetRenderer = _targetInfo.TargetObject.GetComponent<Renderer>();
                    if (targetRenderer != null) _targetInfo.TargetRenderers.Add(targetRenderer);

                                    var renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;
                }
            }

            if (!IsPlayingAnyEffect)
            {
                Animation animation = gameObject.GetComponent<Animation>();
                if (animation != null && animation.isPlaying)
                {
                    AnimationClip playingClip = animation.clip;
                    Animation clonedAnimation = _targetInfo.TargetObject.GetComponent<Animation>();
                    clonedAnimation[playingClip.name].time = animation[playingClip.name].time;
                }

                Animator animator = gameObject.GetComponent<Animator>();
                if (animator != null)
                {
                    Animator clonedAnimator = _targetInfo.TargetObject.GetComponent<Animator>();
                    if (clonedAnimator.runtimeAnimatorController != null)
                    {
                        for (int layerIndex = 0; layerIndex < clonedAnimator.layerCount; ++layerIndex)
                        {
                            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);
                            clonedAnimator.Play(currentState.fullPathHash, layerIndex, currentState.normalizedTime);
                        }
                    }
                }
            }

            return true;
        }

        private void OnPlayBegin(HoverEffectType effectType)
        {
            if (PlayBegin != null) PlayBegin(effectType);
        }

        private void OnPlayEnd(HoverEffectType effectType)
        {
            if (effectType == HoverEffectType.Color)
                _colorEffectCrtn.DestroyEffectMaterials();

            if (!IsPlayingAnyEffect)
                OnNoEffectPlaying();

            if (PlayEnd != null) PlayEnd(effectType);
        }

        private void OnNoEffectPlaying()
        {
            if (_targetInfo.TargetObject != null)
            {
                _targetInfo.TargetObject.SetActive(false);
                GameObject.Destroy(_targetInfo.TargetObject);
                _targetInfo.TargetObject = null;
            }

            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
                renderer.enabled = true;
        }

        private void OnDisable()
        {
            if (_targetInfo.TargetObject != null)
                _targetInfo.TargetObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_targetInfo.TargetObject != null)
                _targetInfo.TargetObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if (_targetInfo.TargetObject != null)
                InstantStop();
        }

        private void OnRenderObject()
        {
            foreach (var effectCrtn in _allEffectCrtns)
                effectCrtn.Render();
        }

        private void RegisterEffectCrtn(HoverEffectCoroutine effectCrtn)
        {
            effectCrtn.PlayBegin += OnPlayBegin;
            effectCrtn.PlayEnd += OnPlayEnd;
            _allEffectCrtns.Add(effectCrtn);
        }

        private Ray GetHoverRay()
        {
            #if INPUT_MOBILE
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return Camera.main.ScreenPointToRay(touch.position);
            }
            #else
            return Camera.main.ScreenPointToRay(Input.mousePosition);
            #endif
        }
    }
}
