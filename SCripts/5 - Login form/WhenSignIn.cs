using General.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class WhenSignIn : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _turnOn;
        [SerializeField] private RectTransform[] _turnOff;

        private void Awake()
        {
            DatabaseAuth.SignInSuccessful += TurnOnAndOff;
        }

        private void TurnOnAndOff()
        {
            foreach (var turnOn in _turnOn)
                turnOn.gameObject.SetActive(true);

            foreach (var turnOff in _turnOff)
                turnOff.gameObject.SetActive(false);
        }
    }
}