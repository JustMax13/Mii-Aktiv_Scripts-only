using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class LinkObject : MonoBehaviour, IOpenLink
    {
        [SerializeField] private string _link;

        public string Link { get => _link; set => _link = value; }

        public void OpenLink() => LinkActions.OpenLink(_link);
    }
}