using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Value
{
    public class ChangeStateInterfacesValue : MonoBehaviour
    {
        [Header("Якщо нічого не вести - нічого не вимкнеться")]
        [SerializeField] private RectTransform[] _turnOffInterface;
        [Header("Якщо нічого не вести - нічого не увімкнеться")]
        [SerializeField] private RectTransform[] _turnOnInterface;

        public RectTransform[] TurnOnInterface { get => _turnOnInterface; set => _turnOnInterface = value; }
        public RectTransform[] TurnOffInterface { get => _turnOffInterface; set => _turnOffInterface = value; }

    }
}