using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public interface IGameObjectsState
    {
        public void ChangeObjectState(int stateValue);
    }
}