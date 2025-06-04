using UnityEngine;

namespace CreateMenu
{
    public class MinChildCount : FieldIsFilled
    {
        [SerializeField] private RectTransform _childParent;
        [SerializeField] private int _minChildNumber;

        public override void UpdateTaskCompletion()
        {
            if (_childParent.childCount >= _minChildNumber)
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