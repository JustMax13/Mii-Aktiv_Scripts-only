using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class SafeArea : MonoBehaviour
    {
        private void Awake()
        {
            SetSafeArea();
        }

        private void SetSafeArea()
        {
            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            var thisRectTransform = GetComponent<RectTransform>();
            thisRectTransform.anchorMin = anchorMin;
            thisRectTransform.anchorMax = anchorMax;
        }
    }
}