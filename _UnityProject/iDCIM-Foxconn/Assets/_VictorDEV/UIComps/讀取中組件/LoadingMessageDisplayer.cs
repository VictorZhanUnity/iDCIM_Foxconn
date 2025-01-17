using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    public class LoadingMessageDisplayer : PopUpWindow
    {
        private IEnumerator LoopingMessage()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (++_counter <= 3)
                {
                    TxtMsg.text = $"{TxtMsg.text}.";
                }
                else
                {
                    ButtonCancell.gameObject.SetActive(true);
                    _counter = 0;
                    TxtMsg.text = _originalTxt;
                }
            }
        }

        #region [Initialize]
        private void Awake() => _originalTxt = TxtMsg.text.Trim();

        private void OnEnable()
        {
            ButtonCancell.onClick.AddListener(()=>gameObject.SetActive(false));
            ButtonCancell.gameObject.SetActive(false);
            StartCoroutine(LoopingMessage());
            ToShow();
        }

        private void OnDisable()
        {
            ButtonCancell.onClick.RemoveListener(()=>gameObject.SetActive(false));
            StopCoroutine(LoopingMessage());
            ToClose();
            _counter = 0;
            TxtMsg.SetText(_originalTxt);
        }
        #endregion

        #region [Components]
        private int _counter;
        private string _originalTxt;
        private TextMeshProUGUI TxtMsg => _txtMsg ??= transform.Find("txtMsg").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI _txtMsg;

        private Button ButtonCancell => _buttonCancell ??= TxtMsg.transform.Find("ButtonCancell").GetComponent<Button>();
        private Button _buttonCancell;
        #endregion
    }
}