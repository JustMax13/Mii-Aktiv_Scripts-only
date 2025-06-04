using UnityEngine;

namespace General.Move
{
    public class InitialTransform : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector3 _position;
        private Vector3 _localPosition;

        private bool _valueIsInit = false;
        private void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }
        private void Update()
        {
            if(!_valueIsInit)
            {
                _position = _rectTransform.position;
                _localPosition = _rectTransform.localPosition;

                _valueIsInit = true;
            }
        }
        public void SetInitPosition()
            => _rectTransform.position = _position;
        public void SetInitLocalPosition()
            => _rectTransform.localPosition = _localPosition;
    }
}