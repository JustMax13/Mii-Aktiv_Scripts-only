using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class TextState : MonoBehaviour, IGameObjectsState
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string[] _textState;
        public void ChangeObjectState(int stateValue) => _text.text = _textState[stateValue];
    }
}