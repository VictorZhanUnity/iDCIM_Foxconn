using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Common
{
    public class OnEnabledHandler : MonoBehaviour
    {
        public UnityEvent onEnabledEvent = new UnityEvent();
        public UnityEvent onDisbledEvent = new UnityEvent();

        private void OnEnable() => onEnabledEvent.Invoke();
        private void OnDisable() => onDisbledEvent.Invoke();
    }
}