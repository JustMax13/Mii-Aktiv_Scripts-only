using System;
using System.Collections.Generic;
using UnityEngine;

namespace General.ChooseTime
{
    public class CentralNumberPanel : MonoBehaviour
    {
        [SerializeField] private NumberSpawn _numberSpawn;
        [SerializeField] private RectTransform _center;
        [SerializeField] private bool _horizontalScrolling;
        [SerializeField] private float _snapSpeed;
        [SerializeField] private float _maxSpeedForSnap;
        [SerializeField] private ScrollingControl _scrollingControl;

        private bool _isScrolling;
        private Vector2 _contentVector;
        private RectTransform _panelContent;
        private TimePanel _panelInCenter;
        private List<TimePanel> _panels;

        public bool IsScrolling { get => _isScrolling; set => _isScrolling = value; }
        public TimePanel PanelInCenter { get => _panelInCenter; private set => _panelInCenter = value; }

        private void Awake()
        {
            _numberSpawn.SpawnEnd += SetPanels;

            _panels = new List<TimePanel>();
        }
        private void FixedUpdate()
        {
            if (_panels != null)
            {
                if (_panels.Count != 0)
                {
                    // Розрахунок центральної панелі за індексом
                    float minDistance = float.MaxValue;
                    int centralPanelIndex = -1;

                    for (int i = 0; i < _panels.Count; i++)
                    {
                        float currentDis = Math.Abs(_center.position.y - _panels[i].RectTransform.position.y);
                        if (minDistance > currentDis)
                        {
                            minDistance = currentDis;
                            centralPanelIndex = i;
                        }
                    }

                    PanelInCenter = _panels[centralPanelIndex];

                    // Ставимо центральну панель у центр
                    if (_horizontalScrolling)
                    {
                        if (!IsScrolling && Math.Abs(_scrollingControl.ScrollRect.velocity.x) <= _maxSpeedForSnap)
                        {
                            _scrollingControl.StopScrolling();

                            _contentVector = _panelContent.position;
                            _contentVector.x = Mathf.SmoothStep(_panelContent.position.x,
                                _panelContent.position.x + (_center.position.x - PanelInCenter.RectTransform.position.x),
                                _snapSpeed * Time.fixedDeltaTime);
                            _panelContent.position = _contentVector;
                        }
                    }
                    else
                    {
                        if (!IsScrolling && Math.Abs(_scrollingControl.ScrollRect.velocity.y) <= _maxSpeedForSnap)
                        {
                            _scrollingControl.StopScrolling();

                            _contentVector = _panelContent.position;
                            _contentVector.y = Mathf.SmoothStep(_panelContent.position.y,
                                _panelContent.position.y + (_center.position.y - PanelInCenter.RectTransform.position.y),
                                _snapSpeed * Time.fixedDeltaTime);
                            _panelContent.position = _contentVector;
                        }
                    }
                }
            }
        }

        private void SetPanels(List<TimePanel> panels)
        {
            _panels = panels;
            _panelContent = panels[0].transform.parent.GetComponent<RectTransform>();
        }
        public void SetIsScrolling(bool value)
            => IsScrolling = value;
    }
}