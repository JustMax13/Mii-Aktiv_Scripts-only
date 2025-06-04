using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public interface ICheckExecution
    {
        public bool TaskCompletion { get; }

        public Action TaskExecutionCanceled { get; }
        public Action TaskDone { get; }

        public void UpdateTaskCompletion();
    }
}