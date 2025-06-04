using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CreateMenu
{
    public class AllFieldsIsFilled : MonoBehaviour, ICheckExecution
    {
        [SerializeField] private FieldIsFilled[] _fieldsIsFilled;
        [SerializeField] private UnityEvent _checkResultIsTrue;
        [SerializeField] private UnityEvent _checkResultIsFalse;
        private bool _taskCompletion;

        public bool TaskCompletion
        {
            get => _taskCompletion;
            private set
            {
                if (value == true)
                {
                    if (_taskCompletion != value)
                    {
                        _taskCompletion = value;
                        TaskDone?.Invoke();
                    }
                }
                else
                {
                    if (_taskCompletion != value)
                    {
                        _taskCompletion = value;
                        TaskExecutionCanceled?.Invoke();
                    }
                }
            }
        }
        public FieldIsFilled[] FieldsIsFilled { get => _fieldsIsFilled; set => _fieldsIsFilled = value; }

        public Action TaskExecutionCanceled { get; private set; }

        public Action TaskDone { get; private set; }

        public void UpdateCompletionInFields()
        {
            foreach (var field in _fieldsIsFilled)
                field.UpdateTaskCompletion();
        }
        public void UpdateTaskCompletion()
        {
            UpdateCompletionInFields();

            bool allFieldIsFilled = true;
            foreach (var fieldIsFilled in _fieldsIsFilled)
                if (fieldIsFilled.TaskCompletion != true)
                {
                    allFieldIsFilled = false;
                    break;
                }

            TaskCompletion = allFieldIsFilled;

            if (TaskCompletion == true)
            {
                TaskDone?.Invoke();
                _checkResultIsTrue?.Invoke();
            }   
            else
            {
                TaskExecutionCanceled?.Invoke();
                _checkResultIsFalse?.Invoke();
            }

            Debug.Log(TaskCompletion);
        }
    }
}