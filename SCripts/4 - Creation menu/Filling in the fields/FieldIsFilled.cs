using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public abstract class FieldIsFilled : MonoBehaviour, ICheckExecution
    {
        private bool _taskCompletion;
        public bool TaskCompletion
        {
            get => _taskCompletion;
            protected set
            {
                if (value == true)
                {
                    if(_taskCompletion!= value)
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

        public Action TaskExecutionCanceled { get; private set; }
        public Action TaskDone { get; private set; }

        public abstract void UpdateTaskCompletion();
    }
}