using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class NeverClear : MonoBehaviour
    {
        [SerializeField] private List<Transform> _neverClearChilds;

        public List<Transform> NeverClearChilds { get => _neverClearChilds; set => _neverClearChilds = value; }
    }
}