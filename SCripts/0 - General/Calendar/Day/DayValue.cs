using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General.Calendar
{
    public class DayValue : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _selectedDaySprite;
        [SerializeField] private Sprite _currentDaySprite;

        private DateTime _value;
        private bool _visualTurnOn;
        public Button Button { get => _button; set => _button = value; }
        public TextMeshProUGUI Tmp { get => _tmp; set => _tmp = value; }
        public Image Image { get => _image; set => _image = value; }
        public DateTime Value { get => _value; set => _value = value; }
        public bool VisualTurnOn { get => _visualTurnOn; private set => _visualTurnOn = value; }

        public void TurnOffVisual()
        {
            _tmp.enabled = false;
            _image.enabled = false;
            VisualTurnOn = false;
        }
        public void TurnOnVisual()
        {
            _tmp.enabled = true;
            _image.enabled = true;
            VisualTurnOn = true;
        }
        public void SetDefaultSprite() => Image.sprite = _defaultSprite;
        public void SetSelectDaySprite() => Image.sprite = _selectedDaySprite;
        public void SetCurrentDaySprite() => Image.sprite = _currentDaySprite;
    }
}
