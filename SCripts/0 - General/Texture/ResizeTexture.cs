using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class ResizeTexture : MonoBehaviour
    {
        [SerializeField] private Image _toResize;
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public void Resize() => _toResize.sprite =
            Sprite.Create(TextureScaler.ScaleTexture(_toResize.sprite.texture, _width, _height), 
                new Rect(0, 0, _width, _height), 
                Vector2.one * 0.5f);
    }
}