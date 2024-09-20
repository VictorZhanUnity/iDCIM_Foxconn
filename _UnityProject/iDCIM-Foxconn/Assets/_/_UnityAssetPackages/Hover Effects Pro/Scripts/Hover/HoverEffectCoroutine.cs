using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public abstract class HoverEffectCoroutine
    {
        public delegate void PlayBeginCallback(HoverEffectType effectType);
        public delegate void PlayEndCallback(HoverEffectType effectType);

        public event PlayBeginCallback PlayBegin;
        public event PlayEndCallback PlayEnd;

        private HoverEffectStateFlags _stateFlags = HoverEffectStateFlags.Inactive;

        protected HoverEffect _parentEffect;
        protected HoverEffectTargetInfo _targetInfo;

        protected Coroutine _playEffectCrtn;
        protected Coroutine _enterEffectCrtn;
        protected Coroutine _applyEffectCrtn;
        protected Coroutine _exitEffectCrtn;

        public HoverEffectStateFlags StateFlags { get { return _stateFlags; } }
        public bool IsEntering { get { return _enterEffectCrtn != null; } }
        public bool IsPlaying { get { return _playEffectCrtn != null || _enterEffectCrtn != null || _applyEffectCrtn != null || _exitEffectCrtn != null; } }
        public bool IsExiting { get { return _exitEffectCrtn != null; } }
        public abstract HoverEffectType EffectType { get; }

        public bool Play(HoverEffect parentEffect, HoverEffectTargetInfo targetInfo)
        {
            if (parentEffect == null || IsPlaying) return false;

            _parentEffect = parentEffect;
            _targetInfo = targetInfo;

            if (!InitializeBeforePlay()) return false;
            _playEffectCrtn = _parentEffect.StartCoroutine(PlayEffect());

            if (PlayBegin != null) PlayBegin(EffectType);

            return true;
        }

        public void Stop()
        {
            if (!IsPlaying || IsExiting || _parentEffect == null) return;

            if (_playEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_playEffectCrtn);
                _playEffectCrtn = null;
            }

            if (_enterEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_enterEffectCrtn);
                _enterEffectCrtn = null;
            }

            if (_applyEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_applyEffectCrtn);
                _applyEffectCrtn = null;
            }

            _parentEffect.StartCoroutine(StopEffect());
        }

        public void InstantStop()
        {
            if (!IsPlaying || _parentEffect == null) return;

            if (_playEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_playEffectCrtn);
                _playEffectCrtn = null;
            }

            if (_enterEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_enterEffectCrtn);
                _enterEffectCrtn = null;
            }

            if (_applyEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_applyEffectCrtn);
                _applyEffectCrtn = null;
            }

            if (_exitEffectCrtn != null)
            {
                _parentEffect.StopCoroutine(_exitEffectCrtn);
                _exitEffectCrtn = null;
            }

            _stateFlags = HoverEffectStateFlags.Inactive;

            OnPlayEnd();
            if (PlayEnd != null) PlayEnd(EffectType);
        }

        public void Render()
        {
            if (IsPlaying) OnRender();
        }

        protected virtual void OnRender() 
        { 
        }

        protected abstract bool InitializeBeforePlay();

        protected abstract IEnumerator EnterEffect();
        protected abstract IEnumerator ApplyEffect();
        protected abstract IEnumerator ExitEffect();

        protected virtual void OnPlayEnd()
        {
        }

        private IEnumerator PlayEffect()
        {
            _stateFlags = HoverEffectStateFlags.Playing | HoverEffectStateFlags.Entering;
            _enterEffectCrtn = _parentEffect.StartCoroutine(EnterEffect());
            yield return _enterEffectCrtn;
            _enterEffectCrtn = null;

            _stateFlags = HoverEffectStateFlags.Playing;
            _applyEffectCrtn = _parentEffect.StartCoroutine(ApplyEffect());
            yield return _applyEffectCrtn;
            _applyEffectCrtn = null;

            _stateFlags = HoverEffectStateFlags.Playing | HoverEffectStateFlags.Exiting;
            _exitEffectCrtn = _parentEffect.StartCoroutine(ExitEffect());
            yield return _exitEffectCrtn;
            _exitEffectCrtn = null;

            _playEffectCrtn = null;
            _stateFlags = HoverEffectStateFlags.Inactive;

            OnPlayEnd();
            if (PlayEnd != null) PlayEnd(EffectType);
        }

        private IEnumerator StopEffect()
        {
            _stateFlags = HoverEffectStateFlags.Playing | HoverEffectStateFlags.Exiting;
            _exitEffectCrtn = _parentEffect.StartCoroutine(ExitEffect());
            yield return _exitEffectCrtn;
            _exitEffectCrtn = null;

            _stateFlags = HoverEffectStateFlags.Inactive;
            OnPlayEnd();
            if (PlayEnd != null) PlayEnd(EffectType);
        }   
    }
}
