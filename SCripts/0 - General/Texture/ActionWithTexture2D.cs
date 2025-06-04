using General.Value;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace General
{
    public class ActionWithTexture2D : MonoBehaviour
    {
        public static string TextureExpansion { get; set; } = ".tga";
        public static byte[] TextureToBytes(in Texture2D texture)
        {
            Texture2D copyTexture = DuplicateTexture(texture);
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

            return pixelBytes;
        }
        public static byte[] CompressTextureToBytes(in Texture2D texture)
        {
            Texture2D copyTexture = DuplicateTexture(texture);
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

            return CompressBytes(pixelBytes);
        }

        public static async Task<Texture2D> CreateTexture(byte[] image, ImageValues imageValues, CancellationToken cancellationToken = default)
        {
            Texture2D newTexture = new Texture2D(0, 0, TextureFormat.RGBAFloat, false);
            newTexture.Reinitialize(imageValues.Width, imageValues.Height);

            byte[] decompressImage = null;
            Color32[] newPixels = null;

            try
            {
                await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    decompressImage = DecompressBytes(image);
                    newPixels = new Color32[decompressImage.Length / 4];
                }, cancellationToken);

                if (decompressImage.Length == imageValues.Width * imageValues.Height * 4)
                {
                    int byteIndex = 0;

                    await Task.Run(() =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        for (int i = 0; i < newPixels.Length; i++)
                        {
                            byte r = decompressImage[byteIndex++];
                            byte g = decompressImage[byteIndex++];
                            byte b = decompressImage[byteIndex++];
                            byte a = decompressImage[byteIndex++];

                            newPixels[i] = new Color32(r, g, b, a);
                        }
                    }, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();

                    newTexture.SetPixels32(newPixels);
                    newTexture.Apply();
                }
                else
                    Debug.LogError("Неправильний розмір масива байтів!");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Задача скасована.");
                return null;
            }

            return newTexture;
        }

        public static byte[] CompressBytes(byte[] decompressedBytes)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress))
                {
                    deflateStream.Write(decompressedBytes, 0, decompressedBytes.Length);
                }

                byte[] compressedBytes = compressedStream.ToArray();
                return compressedBytes;
            }
        }
        public static byte[] DecompressBytes(byte[] compressedBytes)
        {
            using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(decompressedStream);
                    }

                    byte[] decompressedBytes = decompressedStream.ToArray();
                    return decompressedBytes;
                }
            }
        }
        public static Texture2D DuplicateTexture(in Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}