using General.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class LoadImageByPath : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public async void UploadImageByPathAsync(string path)
        {
            await DatabaseActions.UploadImageAsync(_image.sprite.texture, path);
            _image.sprite = null;
            Debug.Log("Success upload!");
        }
    }
}