using UnityEngine;

namespace General
{
    public static class TextureScaler
    {
        public static Texture2D ScaleTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
        {
            var textureCopy = ActionWithTexture2D.DuplicateTexture(sourceTexture);
            Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBAFloat, false);
            Color[] pixels = resizedTexture.GetPixels(0);

            float ratioX = ((float)textureCopy.width) / targetWidth;
            float ratioY = ((float)textureCopy.height) / targetHeight;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = Mathf.FloorToInt(i % targetWidth * ratioX);
                int y = Mathf.FloorToInt(i / targetWidth * ratioY);
                pixels[i] = textureCopy.GetPixel(x, y);
            }

            resizedTexture.SetPixels(pixels);
            resizedTexture.Apply();

            return resizedTexture;
        }
    }

}