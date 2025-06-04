using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Filters.FilteringValues.Category
{
    public class CategoriesContent : MonoBehaviour
    {
        [SerializeField] private GameObject _categoryPrefab;
        [SerializeField] private RectTransform _contentRect;

        private List<CategoryObject> _allCategoriesObjects;
        public Action<List<CategoryObject>> AwakeReturnAllCategories;

        public List<CategoryObject> AllCategories { get => _allCategoriesObjects; set => _allCategoriesObjects = value; }

        private void Awake()
        {
            _allCategoriesObjects = new List<CategoryObject>();

            for (int i = 0; i < Categories.AllCategories.Count; i++)
            {
                GameObject categoryGO = Instantiate(_categoryPrefab, _contentRect);

                CategoryObject category = categoryGO.GetComponent<CategoryObject>();

                category.ID = Categories.AllCategories.ElementAt(i).Key;
                CategoryValue categoryValue = Categories.AllCategories.ElementAt(i).Value;
                category.Name.text = categoryValue.Name;
                category.Image.sprite = Resources.Load<Sprite>(categoryValue.PathToIcon);

                _allCategoriesObjects.Add(category);
            }

            AwakeReturnAllCategories?.Invoke(_allCategoriesObjects);
        }
    }
}