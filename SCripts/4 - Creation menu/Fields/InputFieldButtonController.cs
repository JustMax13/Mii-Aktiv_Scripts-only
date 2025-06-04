using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CreateMenu.Controllers
{
    public class InputFieldButtonController : Button
    {
        public ControlledTMP_InputField _TMP_Input { get; set; }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (_TMP_Input != null && _TMP_Input.enabled != true)
            {
                _TMP_Input.enabled = true;
                _TMP_Input.Select();
                _TMP_Input.ActivateInputField();
            } else if(_TMP_Input == null)
                Debug.LogWarning("Забули передати поле: _TMP_Input = null. Зробіть це через скрипт SetInputFieldForButton.");
        }
    }
}