using CreateMenu;
using System;
using TMPro;
using UnityEngine;

namespace General.ChooseTime
{
    public class TextTimeValue : FieldIsFilled
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private CentralTimePanels _centralTimePanels;

        private TimePanel _hourCentralPanel;
        private TimePanel _minuteCentralPanel;
        private string _defaultTextValue;
        private DateTime _selectedTime;

        public TextMeshProUGUI Tmp { get => _tmp; private set => _tmp = value; }
        public string DefaultTextValue { get => _defaultTextValue; set => _defaultTextValue = value; }
        public DateTime SelectedTime { get => _selectedTime; set => _selectedTime = value; }

        private void Awake()
        {
            DefaultTextValue = _tmp.text;
            _centralTimePanels.OnApplyCenterPanels += SetTime;
        }

        private void SetTime(TimePanel hour, TimePanel minute)
        {
            _hourCentralPanel = hour;
            _minuteCentralPanel = minute;

            SelectedTime = new DateTime(1, 1, 1, _hourCentralPanel.Value, _minuteCentralPanel.Value, 0);

            if (_minuteCentralPanel.Value < 10)
                _tmp.text = _hourCentralPanel.Value + ":0" + _minuteCentralPanel.Value;
            else
                _tmp.text = _hourCentralPanel.Value + ":" + _minuteCentralPanel.Value;
        }
        public override void UpdateTaskCompletion()
        {
            if (_tmp.text != _defaultTextValue)
                TaskCompletion = true;
            else
                TaskCompletion = false;
        }
    }
}