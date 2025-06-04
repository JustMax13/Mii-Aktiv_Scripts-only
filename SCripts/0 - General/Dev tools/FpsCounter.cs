using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tests
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsText;
        private float _deltaTime;

        void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _fpsText.text = string.Format("{0:0.} FPS", fps);
        }
    }
}