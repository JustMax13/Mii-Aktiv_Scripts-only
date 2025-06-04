using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public class AllObjectsWithState : MonoBehaviour, IGameObjectsState
    {
        [SerializeField] private ImageState[] _imageState;
        [SerializeField] private TextState[] _textState;

        public void ChangeObjectState(int stateValue)
        {
            foreach (var item in _imageState)
                item.ChangeObjectState(stateValue);
            foreach (var item in _textState)
                item.ChangeObjectState(stateValue);
        }
    }
}