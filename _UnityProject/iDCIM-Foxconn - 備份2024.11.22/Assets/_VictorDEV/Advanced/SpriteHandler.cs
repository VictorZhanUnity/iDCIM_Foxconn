using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace VictorDev.Common
{
    /// <summary>
    /// [單例模式] Sprite轉換器
    /// </summary>
    public class SpriteHandler : SingletonMonoBehaviour<SpriteHandler>
    {
        /// <summary>
        /// 透過網址下載圖片轉為Sprite
        /// </summary>
        public static void LoadImageToSprite(string url, Action<Sprite> onSuccess)
        {
            IEnumerator DownloadImageCoroutine(string url, Action<Sprite> onSuccess)
            {
                // 使用 UnityWebRequest 下载图片
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    // 获取Texture
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    onSuccess.Invoke(TextureToSprite(texture));
                }
            }
            Instance.StartCoroutine(DownloadImageCoroutine(url, onSuccess));
        }

        /// <summary>
        /// Texture2D轉換為Sprite
        /// </summary>
        public static Sprite TextureToSprite(Texture2D texture)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Sprite轉成Base64字串
        /// </summary>
        public static string SpriteToBase64String(Sprite sprite) => Texture2DToBase64String(sprite.texture);

        /// <summary>
        /// Texture2D轉成Base64字串
        /// </summary>
        public static string Texture2DToBase64String(Texture2D texture)
        {
            byte[] textureBytes = texture.EncodeToPNG(); // You can also use EncodeToJPG() if preferred
            return System.Convert.ToBase64String(textureBytes);
        }  

        /// <summary>
        /// Base64字串轉成Sprite
        /// </summary>
        public static Sprite Base64StringToSprite(string base64String)
        {
            Texture2D texture = Base64StringToTexture(base64String);
            return TextureToSprite(texture);
        }

        /// <summary>
        /// Base64字串轉成Texture2D
        /// </summary>
        public static Texture2D Base64StringToTexture(string base64String)
        {
            byte[] textureBytes = System.Convert.FromBase64String(base64String);
            Texture2D texture = new Texture2D(2, 2); // 2x2 is a placeholder size, it will be replaced by LoadImage
            texture.LoadImage(textureBytes); // Load byte[] into the texture
            return texture;
        }
    }
}
