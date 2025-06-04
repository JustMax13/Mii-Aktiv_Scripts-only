using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace General
{
    public class StarterManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _startActions;

        private void Awake()
        {
            _startActions?.Invoke();
        }
    }
}