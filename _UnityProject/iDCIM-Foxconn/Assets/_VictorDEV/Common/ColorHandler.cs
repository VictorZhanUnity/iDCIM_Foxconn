using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

public class ColorHandler : SingletonMonoBehaviour<ColorHandler>
{
    public static void LerpColor(Image target, Color start, Color end, float duration=0.7f)
    {
        IEnumerator action()
        {
            float elapsedTime = 0.0f;

            // 確保在 duration 時間內進行顏色的過渡
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // 使用 Lerp 進行顏色過渡
                target.color = Color.Lerp(start * 100, end, t);

                // 等待下一幀再繼續
                yield return null;
            }
            // 確保最後顏色為目標顏色
            target.color = end;
        }
        Instance.StartCoroutine(action());
    }
}
