using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class ImageState : MonoBehaviour, IGameObjectsState
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _stateSprites;
        public void ChangeObjectState(int stateValue) => _image.sprite = _stateSprites[stateValue];
    }
}