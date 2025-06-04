using System;
using UnityEngine;

namespace General.Move
{
    public class MoveAfterPoint : MonoBehaviour
    {
        [SerializeField] private RectTransform _point;
        [SerializeField] private RectTransform _movableObject;
        [SerializeField] private float _localDistance_Y;

        public Action MoveAfterPointEnd;

        public void Move()
        {
            _movableObject.position = new Vector3(
                _movableObject.position.x,
                _point.position.y + _movableObject.TransformVector(new Vector2(0, _localDistance_Y)).y,
                _movableObject.position.z);

            MoveAfterPointEnd?.Invoke();
        }
    }
}
