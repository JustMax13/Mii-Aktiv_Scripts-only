using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Move
{
    public class ChangeContentBottom : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform[] _movableObjectsInContent;
        [SerializeField] private RectTransform _currentPositionBottom;
        [SerializeField] private RectTransform _desiredPositionBottom;
        [SerializeField] private RectTransform _minPositionBottom;

        public void Change()
        {
            // немного не точно пеhеносит
            float distance = 0;
            float localDistance = 0;

            if (_minPositionBottom.position.y < _desiredPositionBottom.position.y)
            {
                distance = _currentPositionBottom.position.y - _minPositionBottom.position.y;
                localDistance = _currentPositionBottom.InverseTransformVector(new Vector2(0, distance)).y;
            }
            else
            {
                distance = _currentPositionBottom.position.y - _desiredPositionBottom.position.y;
                localDistance = _currentPositionBottom.InverseTransformVector(new Vector2(0, distance)).y;
            }

            foreach (var obj in _movableObjectsInContent)
                obj.SetParent(_content.parent);

            _content.offsetMin = new Vector2(_content.offsetMin.x, _content.offsetMin.y - localDistance);

            foreach (var obj in _movableObjectsInContent)
                obj.SetParent(_content);
        }
    }
}