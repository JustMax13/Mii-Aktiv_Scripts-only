using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Filters.FilteringValues.Category
{
    public class CategoryObject : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _image;

        public int ID { get; set; }
        public Toggle Toggle { get => _toggle; }
        public TextMeshProUGUI Name { get => _name; set => _name = value; }
        public Image Image { get => _image; }
    }
}