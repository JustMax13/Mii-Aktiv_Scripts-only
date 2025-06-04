using General;
using General.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tests
{
    public class PhotoCopy : MonoBehaviour
    {
        [SerializeField] private Image _copyFrom;
        [SerializeField] private RawImage _copyTo;

        public void CopyAndSet()
        {
            Texture2D copyTexture = ActionWithTexture2D.DuplicateTexture(_copyFrom.sprite.texture);
            Color[] pixels = copyTexture.GetPixels();
            byte[] pixelBytes = new byte[pixels.Length * 4];

            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixel = pixels[i];
                int byteIndex = i * 4;

                pixelBytes[byteIndex] = (byte)(pixel.r * 255);
                pixelBytes[byteIndex + 1] = (byte)(pixel.g * 255);
                pixelBytes[byteIndex + 2] = (byte)(pixel.b * 255);
                pixelBytes[byteIndex + 3] = (byte)(pixel.a * 255);
            }

            int width = _copyFrom.sprite.texture.width;
            int height = _copyFrom.sprite.texture.height;

            Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
            Color32[] newPixels = newTexture.GetPixels32();

            if (pixelBytes.Length == width * height * 4)
            {
                int byteIndex = 0;

                for (int i = 0; i < pixels.Length; i++)
                {
                    byte r = pixelBytes[byteIndex++];
                    byte g = pixelBytes[byteIndex++];
                    byte b = pixelBytes[byteIndex++];
                    byte a = pixelBytes[byteIndex++];

                    newPixels[i] = new Color32(r, g, b, a);
                }

                newTexture.SetPixels32(newPixels);
                newTexture.Apply();
            }
            else
            {
                Debug.LogError("Неправильный размер массива байтов!");
            }

            _copyTo.texture = newTexture;
        }
    }
}