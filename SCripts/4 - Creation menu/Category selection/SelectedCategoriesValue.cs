using Filters.FilteringValues.Category;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Filters.FilteringValues.Category.Categories;

namespace CreateMenu
{
    public class SelectedCategoriesValue : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetValue(CategoryObject category)
        {
            var categoryName = new CategoryValue();
            try { Filters.FilteringValues.Category.Categories.AllCategories.TryGetValue(category.ID, out categoryName); }
            catch{ throw new System.Exception("ID категорії вказано не вірно"); }

            _text.text = categoryName.Name;
        }
    }
}