using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu.ImageAction
{
    public class ImagePicker : MonoBehaviour
    {
        [SerializeField] private Image _targetImage;
        public Image targetImage { get => _targetImage; private set => _targetImage = value; }

        public void PickAndCropImage()
        {
            if (!NativeGallery.IsMediaPickerBusy())
            {
                NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
                {
                    if (path != null)
                    {
                        Texture2D texture = NativeGallery.LoadImageAtPath(path);
                        if (texture != null)
                            CropSelectedImage(texture);
                    }
                }, "Оберіть зображення", "image/*");
            }
            else
                Debug.Log("На жаль обрати картинку зараз неможливо.");
        }

        private void CropSelectedImage(Texture2D texture2D)
        {
            if (texture2D != null)
            {
                ImageCropper.Instance.Show(texture2D, (bool result, Texture originalImage, Texture2D croppedImage) =>
                {
                    if (result)
                    {
                        targetImage.sprite = Sprite.Create(croppedImage, new Rect(0, 0, croppedImage.width, croppedImage.height), Vector2.zero);
                    }
                },
                settings: new ImageCropper.Settings()
                {
                    //ovalSelection = true,
                    imageBackground = Color.clear,
                    selectionMinAspectRatio = 1f,
                    selectionMaxAspectRatio = 1f,
                });
            } else
                Debug.LogWarning("Текстура не була обрізана, бо вона = null.");
        }
    }
}