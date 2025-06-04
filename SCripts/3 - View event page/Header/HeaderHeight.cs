using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViewMenu
{
    public class HeaderHeight : MonoBehaviour
    {
        [SerializeField] private RectTransform _headerRectTransform;
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _safeArea;

        private void Awake()
        {
            float subtractFromHeight = _background.rect.height - _safeArea.rect.height;
            _headerRectTransform.offsetMin = new Vector2(_headerRectTransform.offsetMin.x, _headerRectTransform.offsetMin.y - subtractFromHeight);
        }
    }
}