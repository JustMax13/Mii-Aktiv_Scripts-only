using System;
using TMPro;
using UnityEngine;
using ViewMenu;

namespace General
{
    public class FollowThePoint : MonoBehaviour
    {
        [Header("Обов'язкові поля")]
        [SerializeField] private RectTransform _followUp;
        [SerializeField] private RectTransform _pursuer;
        [SerializeField] private bool _followByX;
        [SerializeField] private bool _followByY;
        /// <summary>
        /// Даний флаг позначає, що той хто переміщається за точкою при досягнені ліміту,
        /// не буде слідувати подальшим рухам по осі, поки точка, за якою відбувається рух, не повернеться на 
        /// значення коли був досягнутий ліміт.
        /// </summary> 
        [SerializeField] private bool _continuousMovement;

        [Header("Не обов'язкові поля")]
        [SerializeField] private RectTransform _minPositionX;
        [SerializeField] private RectTransform _maxPositionX;
        [SerializeField] private RectTransform _minPositionY;
        [SerializeField] private RectTransform _maxPositionY;

        private MinMax _xLimitExceeded = MinMax.None;
        private MinMax _yLimitExceeded = MinMax.None;
        private float _inverseMinX = float.NaN;
        private float _inverseMaxX = float.NaN;
        private float _inverseMinY = float.NaN;
        private float _inverseMaxY = float.NaN;
        private float _followUpMinX = float.NaN;
        private float _followUpMaxX = float.NaN;
        private float _followUpMinY = float.NaN;
        private float _followUpMaxY = float.NaN;
        private Vector3 _oldFollowUpDirection;
        private Vector3 _newFollowUpDirection;
        private Action<MinMax> XLimitExceeded;
        private Action<MinMax> YLimitExceeded;
        private Action<MinMax> XLimitNotExceeded;
        private Action<MinMax> YLimitNotExceeded;

        private void Awake()
        {
            if (!CheckMinAndMax())
            {
                throw new System.Exception("Мінімальне та максимальне значення було вказано некоректно." +
                    " Змініть будь-ласка значення");
            }

            if (_minPositionX != null)
                _inverseMinX = _pursuer.parent.InverseTransformPoint(_minPositionX.position).x;
            if (_maxPositionX != null)
                _inverseMaxX = _pursuer.parent.InverseTransformPoint(_maxPositionX.position).x;
            if (_minPositionY != null)
                _inverseMinY = _pursuer.parent.InverseTransformPoint(_minPositionY.position).y;
            if (_maxPositionY != null)
                _inverseMaxY = _pursuer.parent.InverseTransformPoint(_maxPositionY.position).y;

            _oldFollowUpDirection = _pursuer.parent.InverseTransformDirection(_followUp.position);
            _newFollowUpDirection = _pursuer.parent.InverseTransformDirection(_followUp.position);

            if (_continuousMovement)
            {
                XLimitExceeded += OnXLimitExceeded;
                XLimitNotExceeded += OnXLimitNotExceeded;
                YLimitExceeded += OnYLimitExceeded;
                YLimitNotExceeded += OnYLimitNotExceeded;

                SetFollowUpLimitPoint();
            }
        }
        private void Update()
        {
            _newFollowUpDirection = _pursuer.parent.InverseTransformDirection(_followUp.position);

            if (_followByX && _xLimitExceeded != MinMax.None)
                UpdateXLimitExceeded();
            if (_followByY && _yLimitExceeded != MinMax.None)
                UpdateYLimitExceeded();

            if ((_followByX && _followByY) && (_xLimitExceeded == MinMax.None && _yLimitExceeded == MinMax.None))
                MoveByXY();
            else if (_followByX && _xLimitExceeded == MinMax.None)
                MoveByX();
            else if(_followByY && _yLimitExceeded == MinMax.None)
                MoveByY();

            _pursuer.localPosition = SetRestrictions(_pursuer.localPosition);

            _oldFollowUpDirection = _newFollowUpDirection;
        }

        private bool CheckMinAndMax()
        {
            if (_minPositionX != null && _maxPositionX != null)
            {
                if (_minPositionX.position.x > _maxPositionX.position.x)
                    return false;
            }
            if (_minPositionY != null && _maxPositionY != null)
            {
                if (_minPositionY.position.y > _maxPositionY.position.y)
                    return false;
            }

            return true;
        }
        public void MoveByXY()
        {
            Vector3 direction = _newFollowUpDirection - _oldFollowUpDirection;
            _pursuer.Translate(new Vector3(direction.x, direction.y, 0));
        }
        public void MoveByX()
        {
            Vector3 direction = _newFollowUpDirection - _oldFollowUpDirection;
            _pursuer.Translate(new Vector3(direction.x, 0, 0));
        }
        public void MoveByY()
        {
            Vector3 direction = _newFollowUpDirection - _oldFollowUpDirection;
            _pursuer.Translate(new Vector3(0, direction.y, 0));
        }
        public void MoveToMinX()
        {
            _xLimitExceeded = MinMax.Min;
            float directionX = _inverseMinX - _pursuer.parent.InverseTransformPoint(_pursuer.position).x;
            _pursuer.localPosition = new Vector3(_pursuer.localPosition.x + directionX, 0, 0);
        }
        public void MoveToMaxX()
        {
            _xLimitExceeded = MinMax.Max;
            float directionX = _inverseMaxX - _pursuer.parent.InverseTransformPoint(_pursuer.position).x;
            _pursuer.localPosition = new Vector3(_pursuer.localPosition.x + directionX, 0, 0);
        }
        public void MoveToMinY()
        {
            _yLimitExceeded = MinMax.Min;
            float directionY = _inverseMinY - _pursuer.parent.InverseTransformPoint(_pursuer.position).y;
            _pursuer.localPosition = new Vector3(0, _pursuer.localPosition.y + directionY, 0);
        }
        public void MoveToMaxY()
        {
            _yLimitExceeded = MinMax.Max;
            float directionY = _inverseMaxY - _pursuer.parent.InverseTransformPoint(_pursuer.position).y;
            _pursuer.localPosition = new Vector3(0, _pursuer.localPosition.y + directionY, 0);
        }
        public Vector3 SetRestrictions(Vector3 value)
        {
            float x = value.x;
            float y = value.y;

            if (_followByX && _xLimitExceeded == MinMax.None)
            {
                if (_minPositionX != null && x < _inverseMinX)
                {
                    x = _inverseMinX;
                    XLimitExceeded?.Invoke(MinMax.Min);
                }
                else if (_maxPositionX != null && x > _inverseMaxX)
                {
                    x = _inverseMaxX;
                    XLimitExceeded?.Invoke(MinMax.Max);
                }
            }
            
            if (_followByY && _yLimitExceeded == MinMax.None)
            {
                if (_minPositionY != null && y < _inverseMinY)
                {
                    y = _inverseMinY;
                    YLimitExceeded?.Invoke(MinMax.Min);
                }
                else if (_maxPositionY != null && y > _inverseMaxY)
                {
                    y = _inverseMaxY;
                    YLimitExceeded?.Invoke(MinMax.Max);
                }
            }

            value = new Vector3(x, y, value.z);
            return value;
        }
        /// <returns>Поверне true, якщо всі мінімальні та максимальні значення задані корректно</returns>
        private void SetFollowUpLimitPoint()
        {
            float distanceToMinX = _pursuer.localPosition.x - _inverseMinX;
            float distanceToMaxX = _inverseMaxX - _pursuer.localPosition.x;
            float distanceToMinY = _pursuer.localPosition.y - _inverseMinY;
            float distanceToMaxY = _inverseMaxY - _pursuer.localPosition.y;

            var temp = Vector3.zero;
            if(_followByX)
            {
                temp.x = distanceToMinX;
                temp = _pursuer.parent.TransformPoint(temp);
                distanceToMinX = _pursuer.parent.InverseTransformDirection(temp).x;
                temp.x = distanceToMaxX;
                temp = _pursuer.parent.TransformPoint(temp);
                distanceToMaxX = _pursuer.parent.InverseTransformDirection(temp).x;

                _followUpMinX = _newFollowUpDirection.x - distanceToMinX;
                _followUpMaxX = _newFollowUpDirection.x + distanceToMaxX;
            }
            if(_followByY)
            {
                temp.y = distanceToMinY;
                temp = _pursuer.parent.TransformPoint(temp);
                distanceToMinY = _pursuer.parent.InverseTransformDirection(temp).y;
                temp.y = distanceToMaxY;
                temp = _pursuer.parent.TransformPoint(temp);
                distanceToMaxY = _pursuer.parent.InverseTransformDirection(temp).y;

                _followUpMinY = _newFollowUpDirection.y - distanceToMinY;
                _followUpMaxY = _newFollowUpDirection.y + distanceToMaxY;
            }
        }
        private void UpdateXLimitExceeded()
        {
            if (_newFollowUpDirection.x < _followUpMaxX && _newFollowUpDirection.x > _followUpMinX)
                XLimitNotExceeded?.Invoke(_xLimitExceeded);
        }
        private void UpdateYLimitExceeded()
        {
            if (_newFollowUpDirection.y < _followUpMaxY && _newFollowUpDirection.y > _followUpMinY)
                YLimitNotExceeded?.Invoke(_yLimitExceeded);
        }
        private void OnXLimitExceeded(MinMax value)
        {
            _xLimitExceeded = value;
        }
        private void OnYLimitExceeded(MinMax value)
        {
            _yLimitExceeded = value;
        }
        private void OnXLimitNotExceeded(MinMax value)
        {
            if(value == MinMax.Min)
            {
                _oldFollowUpDirection = new Vector3(_followUpMinX, _oldFollowUpDirection.y, _oldFollowUpDirection.z);
            } else
            {
                _oldFollowUpDirection = new Vector3(_followUpMaxX, _oldFollowUpDirection.y, _oldFollowUpDirection.z);
            }

            _xLimitExceeded = MinMax.None;
        }
        private void OnYLimitNotExceeded(MinMax value)
        {
            if (value == MinMax.Min)
            {
                _oldFollowUpDirection = new Vector3(_oldFollowUpDirection.x, _followUpMinY, _oldFollowUpDirection.z);
            }
            else
            {
                _oldFollowUpDirection = new Vector3(_oldFollowUpDirection.x, _followUpMaxY, _oldFollowUpDirection.z);
            }

            _yLimitExceeded = MinMax.None;
        }

        private enum MinMax
        {
            Min,
            Max,
            None
        }
    }
}