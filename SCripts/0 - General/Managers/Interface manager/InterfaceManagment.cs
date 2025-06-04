using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using General.Value;
using System;
using System.Linq;

namespace General.Manager
{
    public class InterfaceManagment : MonoBehaviour
    {
        public static Action SomeInterfaceStateChanged;
        public static void ChangeStateOfAllInterfaces(bool changeTo)
        {
            foreach (var someInterface in AllCanvasValue.AllInterfaces)
                someInterface.gameObject.SetActive(changeTo);
        }
        public static void ChangeStateOfInterfaces(ChangeStateInterfacesValue changeStateInterfacesValue)
        {
            if (changeStateInterfacesValue.TurnOffInterface == null || changeStateInterfacesValue.TurnOnInterface == null)
                throw new System.Exception("TurnOffInterface or / and TurnOnInterface is null");

            foreach (var someInterface in changeStateInterfacesValue.TurnOffInterface)
                someInterface.gameObject.SetActive(false);
            foreach (var someInterface in changeStateInterfacesValue.TurnOnInterface)
                someInterface.gameObject.SetActive(true);

            SomeInterfaceStateChanged?.Invoke();
        }
    }
}