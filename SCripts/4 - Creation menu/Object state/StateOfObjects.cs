using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public class StateOfObjects : MonoBehaviour
    {
        [SerializeField] private AllFieldsIsFilled _allFieldsIsFilled;

        public void UpdateImageState()
        {
            foreach (var item in _allFieldsIsFilled.FieldsIsFilled)
            {
                if (item.TaskCompletion == false)
                {
                    try
                    {
                        var imageState = item.gameObject.GetComponent<ImageState>();
                        imageState.ChangeObjectState(1);
                    }
                    catch
                    {
                        Debug.LogWarning("На " + item.gameObject + " не вдалося знайти ImageState, або вказаний стан не існує");
                    }
                }
                else
                {
                    try
                    {
                        var imageState = item.gameObject.GetComponent<ImageState>();
                        imageState.ChangeObjectState(0);
                    }
                    catch
                    {
                        Debug.LogWarning("На " + item.gameObject + " не вдалося знайти ImageState, або вказаний стан не існує");
                    }
                }
            }
        }
        public void UpdateTextState()
        {
            foreach (var item in _allFieldsIsFilled.FieldsIsFilled)
            {
                if (item.TaskCompletion == false)
                {
                    try
                    {
                        var textState = item.gameObject.GetComponentInChildren<TextState>();
                        textState.ChangeObjectState(1);
                    }
                    catch
                    {
                        Debug.LogWarning("На " + item.gameObject + " не вдалося знайти TextState, або вказаний стан не існує");
                    }
                }
                else
                {
                    try
                    {
                        var textState = item.gameObject.GetComponentInChildren<TextState>();
                        textState.ChangeObjectState(0);
                    }
                    catch
                    {
                        Debug.LogWarning("На " + item.gameObject + " не вдалося знайти TextState, або вказаний стан не існує");
                    }
                }
            }
        }
    }
}