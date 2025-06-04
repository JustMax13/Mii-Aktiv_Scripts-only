using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevTools
{
    public class TextObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;

        public TextMeshProUGUI TMP { get => _tmp; private set => _tmp = value; }
    }
}