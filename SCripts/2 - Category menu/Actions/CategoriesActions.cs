using System.Collections.Generic;
using UnityEngine;

namespace Filters.FilteringValues.Category
{
    public class CategoriesActions : MonoBehaviour
    {
        [SerializeField] private CategoriesContent _categoriesContent;
        private List<CategoryObject> _categories;

        private void Awake()
        {
            if (_categoriesContent.AllCategories != null && _categoriesContent.AllCategories.Count != 0)
                _categories = _categoriesContent.AllCategories;
            else
                _categoriesContent.AwakeReturnAllCategories += SetCategories;
        }

        private void SetCategories(List<CategoryObject> categories) => _categories = categories;
        public List<CategoryObject> GetChooseCategories()
        {
            var chooseCategory = new List<CategoryObject>();

            foreach (var category in _categories)
                if (category.Toggle.isOn)
                    chooseCategory.Add(category);

            return chooseCategory;
        }
        public void SelectCategories()
        {
            foreach (var category in _categories)
                category.Toggle.isOn = true;
        }
        public void DeselectCategories()
        {
            foreach (var category in _categories)
                category.Toggle.isOn = false;
        }
    }
}