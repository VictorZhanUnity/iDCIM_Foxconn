using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

public class ColorHandler : SingletonMonoBehaviour<ColorHandler>
{
    public static void LerpColor(Image target, Color start, Color end, float duration = 0.7f)
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

    /// <summary>
    /// 將Hex十六進制(0xFFFFFF)轉成Color
    /// <para>+ int hex = 0xFFFFFF</para>
    /// </summary>
    public static Color HexToColor(int hexColor, float alpha = 1f)
    {
        // 將十六進制顏色值轉換為 Color（除以255.0f以正確縮放到0到1之間）
        float r = ((hexColor >> 16) & 0xFF) / 255.0f;
        float g = ((hexColor >> 8) & 0xFF) / 255.0f;
        float b = (hexColor & 0xFF) / 255.0f;
        return new Color(r, g, b, alpha); // Alpha 設為 1.0，表示完全不透明
    }
}
