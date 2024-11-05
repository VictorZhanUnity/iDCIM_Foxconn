using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace VictorDev.Common
{
    /// <summary>
    /// 音檔播放器
    /// <para>+ 若要全域使用的話，另外建立一個Singleton來控制就好</para>
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        [Header(">>> 目前播放音檔的網址")]
        [TextArea(1, 5)]
        [SerializeField] private string currentAudioURL = "https://tinyurl.com/29ou6dao";

        [Header(">>> 當音檔下載進度時Invoke")]
        public UnityEvent<string> onAudioDownloadProgress = new UnityEvent<string>();

        [Header(">>> 當音檔下載完畢時Invoke")]
        public UnityEvent<AudioClip> onAudioDownloadComplete = new UnityEvent<AudioClip>();

        [Header(">>> 暫存音檔最大數量")]
        [Range(0, 10)]
        [SerializeField] private int maxOfAudioCache = 5;
        [SerializeField] private AudioSource audioSource;

        /// <summary>
        /// 暫存音檔 {網址, AudioClip}
        /// </summary>
        private Dictionary<string, AudioClip> audioCached { get; set; } = new Dictionary<string, AudioClip>();
        private Coroutine coroutine_PlayAudio { get; set; }
        private Coroutine coroutine_PlayTime { get; set; }

        /// <summary>
        /// 如果想要手動重新下載，可以通過清除緩存來達成
        /// </summary>
        public void ClearCache() => audioCached.Clear();

        /// <summary>
        /// 播放AudioClip音檔
        /// </summary>
        public void PlayAudioClip(AudioClip clip, float startSec = -1, float endSec = -1)
        {
            audioSource.clip = clip;
            audioSource.Play();

            Debug.Log($">>> Audio播放中: {clip.name}");

            // 播放開迄秒數處理
            if (startSec != -1 && endSec != -1)
            {
                IEnumerator StopAudioAfterEndTime(float startTime, float endTime)
                {
                    // 等待直到達到結束時間
                    while (audioSource.time < endTime)
                    {
                        yield return null; // 等待下一幀
                    }
                    Stop();
                }

                audioSource.time = startSec;
                CheckToStopCoroutine(coroutine_PlayTime);
                coroutine_PlayTime = StartCoroutine(StopAudioAfterEndTime(startSec, endSec));
                Debug.Log($"===> StartSec: {startSec} / EndSec: {endSec}");
            }
        }

        /// <summary>
        /// 透過網址下載播放音檔
        /// </summary>
        /// <param name="audioType">音檔類型{Unity 對 OGG 格式有更好的支持}</param>
        public void PlayAudioFromURL(string url, float startSec = -1, float endSec = -1
            , bool isForceReDownload = false, AudioType audioType = AudioType.MPEG)
        {
            url = currentAudioURL = url.Trim();
            CheckToStopCoroutine(coroutine_PlayAudio);
            coroutine_PlayAudio = StartCoroutine(DownloadAndPlayMP3(url, startSec, endSec, isForceReDownload, audioType));
        }

        private void CheckToStopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }

        [ContextMenu("- 停止播放音檔")]
        public void Stop()
        {
            print("===> Stop Audio");
            audioSource.Stop();
        }

        public void Resume() => audioSource.Play();
        public void Pause() => audioSource.Pause();

        IEnumerator DownloadAndPlayMP3(string url, float startSec, float endSec, bool isForceReDownload, AudioType audioType)
        {
            // 檢查是否已經下載並緩存音檔
            if (audioCached.ContainsKey(url) && isForceReDownload == false)
            {
                PlayAudioClip(audioCached[url]);
                yield break; // 已有緩存，不需要重新下載
            }

            // 沒有緩存時，開始下載
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                www.SendWebRequest(); // 先發送請求，然後等待

                // 開始下載音檔
                while (!www.isDone)
                {
                    float progress = www.downloadProgress; // 下載進度 (0 到 1)

                    string progressStr = (progress * 100).ToString("0.##");
                    print($"===> Downloading: {progressStr}%");
                    onAudioDownloadProgress.Invoke(progressStr);

                    yield return null; // 等待下一幀
                }

                // 檢查請求結果
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"MP3下載錯誤：{www.error} / URL: {url}");
                }
                else
                {
                    //若超過暫存最大數量，則移除第一個
                    if (audioCached.Count >= maxOfAudioCache)
                    {
                        audioCached.Remove(audioCached.Keys.First());
                    }

                    // 暫存Audio並播放
                    audioCached[url] = DownloadHandlerAudioClip.GetContent(www);
                    audioCached[url].name = $"[Download] {url}";

                    onAudioDownloadComplete.Invoke(audioCached[url]);
                    PlayAudioClip(audioCached[url], startSec, endSec);

#if UNITY_EDITOR
                    ClearCache();
#endif
                }
            }
        }

        [ContextMenu("- 播放目前音檔網址")]
        private void PlayCurrentAudioURL() => PlayAudioFromURL(currentAudioURL, 0, 3);

        private void Start() => OnValidate();
        private void OnValidate()
        {
            if (audioSource == null && TryGetComponent<AudioSource>(out audioSource) == false)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }
}
