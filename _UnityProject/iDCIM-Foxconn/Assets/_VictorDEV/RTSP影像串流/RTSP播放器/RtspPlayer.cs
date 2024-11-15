using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.RTSP
{
    /// <summary>
    /// [RTSP] 播放器
    /// </summary>
    public class RtspPlayer : MonoBehaviour
    {
        [Header(">>> RTSP位址")]
        [SerializeField] private string URL;

        [Header(">>> 在關閉播放器時Invoke")]
        public UnityEvent onCloseEvent = new UnityEvent();

        [Header(">>> 組件")]
        [SerializeField] private RtspScreen rtspScreen;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Button btnClose;

        private void Start()
        {
            btnClose.onClick.AddListener(() =>
            {
                Stop();
                onCloseEvent.Invoke();
            });
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) Play(); else Stop();
            });
        }

        [ContextMenu("- Play")] private void play() => Play();
        public void Play(string url = "")
        {
            if (string.IsNullOrEmpty(url) == false) URL = url;
            rtspScreen.Play(URL);
        }
        [ContextMenu("- Stop")]
        public void Stop() => rtspScreen.Stop();

        private void OnDisable() => rtspScreen.Stop();

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(URL) == false) URL = URL.Trim();
        }
    }
}
