using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public class CheckAnd : MonoBehaviour
    {
        [SerializeField] private ResizeTexture resizeTexture;
        public void CheckAndResize()
        {
            if (CreateInterfaceValues.DefaultAvatar.texture != CreateInterfaceValues.Avatar.sprite.texture)
                resizeTexture.Resize();
        }
    }
}