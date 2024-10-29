using System.Collections;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.Async.CoroutineUtils
{
    /// <summary>
    /// 對物件的Material進行變色與Intensity
    /// <para>+ 掛載在目標物件</para>
    /// </summary>
    public class LerpEmission : MonoBehaviour
    {
        [Header(">>> Emission設定")]
        public Color colorMouseOver = Color.yellow;
        public Color colorSelected = Color.yellow;
        public float minIntensity = 0f;
        public float maxIntensity = 0.5f;
        public float duration_Lerp = 0.1f;
        public float duration_Loop = 3f;

        public Material material;

        private IEnumerator iEnumerator { get; set; }

        private Color currentColor { get; set; }

        /// <summary>
        /// 進行循環變色與Intensity
        /// </summary>
        [ContextMenu("- StartLoop")]
        public void StartLoop()
        {
            // 启用Emission
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", colorSelected);
            currentColor = colorSelected;

            iEnumerator = LoopEmission();
            CoroutineHandler.RunCoroutine_Old(iEnumerator);
        }
        private IEnumerator LoopEmission()
        {
            while (true)
            {
                float time = 0f;
                while (time < duration_Loop)
                {
                    time += Time.deltaTime;
                    // 使用 Sin 和 Lerp 函数来循环调整发光强度
                    float intensity = Mathf.Lerp(-5f, 2f, (Mathf.Sin(time * Mathf.PI / duration_Loop) + 1) / 2);
                    SetColor(intensity);
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 進行變色與Intensity
        /// </summary>
        [ContextMenu("- StartLerp")]
        public void StartLerp()
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", colorMouseOver);
            currentColor = colorMouseOver;

            DotweenHandler.ToLerpValue(minIntensity, maxIntensity, SetColor, duration_Lerp);
        }

        [ContextMenu("- Stop")]
        public void Stop()
        {
            material.DisableKeyword("_EMISSION");
            if (iEnumerator != null) CoroutineHandler.CancellCoroutine_Old(iEnumerator);
        }

        private void SetColor(float indensity)
        {
            Color finalColor = currentColor * Mathf.LinearToGammaSpace(indensity);
            material.SetColor("_EmissionColor", finalColor);
        }

        private void OnValidate() => material ??= GetComponent<MeshRenderer>().materials[0];
    }
}
