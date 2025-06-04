using System;
using TMPro;
using UnityEngine;

namespace CreateMenu
{
    public class InputFieldIsFilled : FieldIsFilled
    {
        [SerializeField] private int _countCharForPass;
        [SerializeField] private TMP_InputField _TMP_InputField;

        public int CountCharForPass { get => _countCharForPass; set => _countCharForPass = value; }
        public TMP_InputField TMP_InputField { get => _TMP_InputField; set => _TMP_InputField = value; }

        public override void UpdateTaskCompletion()
        {
            if (_TMP_InputField.text.Length >= _countCharForPass)
            {
                if (TaskCompletion != true)
                {
                    TaskCompletion = true;
                    TaskDone?.Invoke();
                }
                    
            }
            else
            {
                if (TaskCompletion != false)
                {
                    TaskCompletion = false;
                    TaskExecutionCanceled?.Invoke();
                }
            }
        }
    }
}