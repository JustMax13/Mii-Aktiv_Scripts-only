using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Value
{
    public class ImageValues
    {
        public int Width;
        public int Height;
        //public int TextureFormat;

        public ImageValues(Texture2D texture2D)
        {
            Width = texture2D.width;
            Height = texture2D.height;
            //TextureFormat = (int)texture2D.format;
        }
        public ImageValues() { }
    }

}
