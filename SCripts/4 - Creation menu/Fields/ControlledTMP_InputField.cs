using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CreateMenu.Controllers
{
    public class ControlledTMP_InputField : TMP_InputField
    {
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            enabled = false;
        }
    }
}