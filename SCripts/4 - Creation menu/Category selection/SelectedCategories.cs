using Filters;
using Filters.FilteringValues.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreateMenu
{
    public class SelectedCategories : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabSelectedCategories;
        [SerializeField] private RectTransform _contentForSpawn;
        [SerializeField] private ApplyCategories _applyCategories;

        private void Awake()
        {
            _applyCategories.GetApplyCategories += UpdateCategories;
        }

        private void UpdateCategories(List<CategoryObject> categories)
        {
            foreach (var item in categories)
            {
                var category = Instantiate(_prefabSelectedCategories, _contentForSpawn).GetComponent<SelectedCategoriesValue>();
                category.SetValue(item);
            }
        }
    }
}