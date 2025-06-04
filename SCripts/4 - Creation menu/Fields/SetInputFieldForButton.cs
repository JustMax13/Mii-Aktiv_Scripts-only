using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CreateMenu.Controllers
{
    public class SetInputFieldForButton : MonoBehaviour
    {
        [SerializeField] private ControlledTMP_InputField _TMP_Input;
        [SerializeField] private InputFieldButtonController _inputFieldButtonController;

        private void Awake()
        {
            _inputFieldButtonController._TMP_Input = _TMP_Input;
        }
    }
}