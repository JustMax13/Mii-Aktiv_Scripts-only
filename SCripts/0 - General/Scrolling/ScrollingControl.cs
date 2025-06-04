using System;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class ScrollingControl : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        public ScrollRect ScrollRect { get => _scrollRect; set => _scrollRect = value; }

        public void StopScrolling()
            => ScrollRect.velocity = Vector2.zero;
    }
}