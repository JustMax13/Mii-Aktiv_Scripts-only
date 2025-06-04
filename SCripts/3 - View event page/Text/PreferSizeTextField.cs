using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class PreferSizeTextField : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private RectTransform _textFieldRectTransform;

        public Action TextAreaResizeDone;

        public TMP_Text TextField { get => _textField; private set => _textField = value; }
        public RectTransform TextFieldRectTransform { get => _textFieldRectTransform; private set => _textFieldRectTransform = value; }

        public void UpdateSize()
        {
            TextFieldRectTransform.sizeDelta = new Vector2(TextFieldRectTransform.sizeDelta.x, TextField.preferredHeight);
            TextAreaResizeDone?.Invoke();
        }
    }
}