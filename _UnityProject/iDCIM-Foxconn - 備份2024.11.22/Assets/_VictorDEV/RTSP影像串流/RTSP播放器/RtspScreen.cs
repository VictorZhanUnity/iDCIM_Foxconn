using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.RTSP
{
    /// <summary>
    /// [RTSP] 單純播放畫面
    /// </summary>
    [RequireComponent(typeof(UniversalMediaPlayer))]
    [RequireComponent(typeof(RawImage))]
    public class RtspScreen : MonoBehaviour
    {
        [Header(">>> RTSP位址")]
        [SerializeField] private string URL;

        [Header(">>> 在連線時Invoke緩衝進度 {百分比}")]
        public UnityEvent<float> onBufferingEvent = new UnityEvent<float>();

        [Header(">>> 播放/停止時Invoke {是否正在播放}")]
        public UnityEvent<bool> onPlayStatusEvent = new UnityEvent<bool>();

        private UniversalMediaPlayer ump { get; set; }
        private RawImage rawImgRTSP { get; set; }
        private GameObject loadingBar { get; set; }

        /// <summary>
        /// UMP初始化較一般慢，所以需用Coroutine去持續檢查它Init的情況
        /// </summary>
        private List<Coroutine> coroutineList { get; set; } = new List<Coroutine>();

        private void Awake()
        {
            ump = GetComponent<UniversalMediaPlayer>();
            rawImgRTSP = GetComponent<RawImage>();
            loadingBar = transform.GetChild(0).gameObject;
            loadingBar.SetActive(false);
            rawImgRTSP.color = new Color(1, 1, 1, 0);

            CheckUMP_Init(() => AddRenderingTarget(rawImgRTSP.gameObject, true));

            InitRtspListener();
        }

        private void CheckUMP_Init(Action action)
        {
            IEnumerator CheckUMPInit()
            {
                while (ump.RenderingObjects == null) yield return null;
                action.Invoke();
            }
            coroutineList.Add(StartCoroutine(CheckUMPInit()));
        }


        private void InitRtspListener()
        {
            // 註冊事件
            ump.AddPathPreparedEvent(OnPathPreparedHandler);
            ump.AddBufferingEvent(OnBufferingHandler);
            ump.AddImageReadyEvent(OnImageReadyHandler);
            ump.AddPreparedEvent(OnPreparedEventHandler);
            ump.AddPlayingEvent(OnPlayingHandler);
            ump.AddStoppedEvent(OnStoppedHandler);
            ump.AddEncounteredErrorEvent(OnEncounteredErrorEventHandler);
        }

        private void OnPathPreparedHandler(string url)
        {
            rawImgRTSP.color = new Color(1, 1, 1, 0);
            Debug.Log($"OnPathPreparedHandler: {url}");
        }

        private void OnBufferingHandler(float percentage)
        {
            loadingBar.SetActive(percentage != 100);
            Debug.Log($"OnBufferingHandler: {percentage}");
            onBufferingEvent.Invoke(percentage);
        }

        private void OnImageReadyHandler(Texture2D screen)
        {
            Debug.Log($"OnImageReadyHandler");
            rawImgRTSP.color = Color.white;
            rawImgRTSP.texture = screen;
            loadingBar.SetActive(false);
        }

        private void OnPreparedEventHandler(int videoWidth, int videoHeight)
        {
            Debug.Log($"OnPreparedEventHandler: {videoWidth} x {videoHeight}");
        }
        private void OnPlayingHandler()
        {
            loadingBar.SetActive(false);
            Debug.Log($"OnPlayingHandler");
            onPlayStatusEvent.Invoke(true);
        }
        private void OnStoppedHandler()
        {
            Debug.Log($"OnStoppedHandler");
            onPlayStatusEvent.Invoke(false);
        }
        private void OnEncounteredErrorEventHandler()
        {
            Debug.Log($"OnEncounteredErrorEventHandler");
        }

        [ContextMenu("- Play")] private void play() => Play();
        public void Play(string url = "")
        {
            if (string.IsNullOrEmpty(url) == false) URL = url;
            if (string.IsNullOrEmpty(URL)) return;

            ump.Path = URL;
            CheckUMP_Init(ump.Play);
        }

        [ContextMenu("- Stop")]
        public void Stop() => ump.Stop();

        /// <summary>
        /// 新增渲染對像到RTSP渲染器內, 只適用RawImage與MeshRenderer
        /// <para>+ isReset：是否先清空RTSP渲染器對像清單</para>
        /// </summary>
        public void AddRenderingTarget(GameObject target, bool isReset = false)
        {
            if (isReset) Array.Clear(ump.RenderingObjects, 0, ump.RenderingObjects.Length);

            ump.RenderingObjects = ump.RenderingObjects
                  .Concat(new[] { target }) // 合併新的 GameObject
                  .Distinct()                            // 去重，避免重複項目
                  .ToArray();
        }
        /// <summary>
        /// 從RTSP渲染器內移除渲染對像
        /// </summary>
        public void RemoveRenderingTarget(GameObject target)
            => ump.RenderingObjects = ump.RenderingObjects.Where(obj => obj != target).ToArray();

        private void RemoveRtspListener()
        {
            // 移除所有事件
            ump.RemoveBufferingEvent(OnBufferingHandler);
            ump.RemovePathPreparedEvent(OnPathPreparedHandler);
            ump.RemoveImageReadyEvent(OnImageReadyHandler);
            ump.RemovePreparedEvent(OnPreparedEventHandler);
            ump.RemovePlayingEvent(OnPlayingHandler);
            ump.RemoveStoppedEvent(OnStoppedHandler);
            ump.RemoveEncounteredErrorEvent(OnEncounteredErrorEventHandler);
        }

        private void OnDisable()
        {
            Stop();
            coroutineList.ForEach(coroutine =>
            {
                if (coroutine != null) StopCoroutine(coroutine);
            });
            ump.Release();
        }
        private void OnDestroy() => RemoveRtspListener();

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(URL) == false) URL = URL.Trim();
        }
    }
}
