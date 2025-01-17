using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// 倒數計時按鈕
    [RequireComponent(typeof(Image))]
    public class TimerCountDownButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public UnityEvent onCountDownComplete = new UnityEvent();
        public UnityEvent<int> onCountDown = new UnityEvent<int>();

        public int totalSec = 7;
        private int _counter = 0;
        
        private Coroutine _countDownCoroutine;

        private IEnumerator ToCountDown()
        {
            while (_counter < totalSec)
            {
                yield return new WaitForSeconds(1);
                onCountDown.Invoke(++_counter);
                Debug.Log(_counter);
            }

            onCountDownComplete.Invoke();
            OnPointerUp(null);
        }

        #region [Initialize]

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerUp(null);
            _countDownCoroutine = StartCoroutine(ToCountDown());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_countDownCoroutine!=null) StopCoroutine(_countDownCoroutine);
            _counter = 0;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerUp(null);
        }
        private void OnDisable() => OnPointerUp(null);
        #endregion
    }
}