using Filters.FilteringValues.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class CategorySelection : MonoBehaviour
    {
        [SerializeField] private int _maxNumberSelectedCategories;
        [SerializeField] private CategoriesContent _categoriesContent;

        private int _currentNumberSelectedCategories;
        private List<CategoryObject> _categories;

        public Action onCounterUpdate;

        private void Awake()
        {
            if (_categoriesContent.AllCategories == null || _categoriesContent.AllCategories.Count == 0)
                _categoriesContent.AwakeReturnAllCategories += SetCategories;
            else
                SetCategories(_categoriesContent.AllCategories);
        }

        private void SetTogglesStatus(bool isOn)
        {
            foreach (var item in _categories)
                if (!item.Toggle.isOn)
                    item.Toggle.interactable = isOn;
        }
        private void SetVisualStatus(VisualAlphaStatus visualAlphaStatus)
        {
            foreach (var categoryObject in _categories)
            {
                if (!categoryObject.Toggle.isOn)
                {
                    var imageColor = categoryObject.Image.color;
                    categoryObject.Image.color = new Color(imageColor.r, imageColor.g, imageColor.b, (float)visualAlphaStatus / 255f);
                } 
            }
        }
        private void CompareCurrentAndMax(bool togglesOn)
        {
            if(togglesOn)
            {
                if(_currentNumberSelectedCategories == _maxNumberSelectedCategories)
                {
                    SetTogglesStatus(false);
                    SetVisualStatus(VisualAlphaStatus.LittleTransparent);
                }  
            }
            else
            {
                if (_currentNumberSelectedCategories + 1 == _maxNumberSelectedCategories)
                {
                    SetTogglesStatus(true);
                    SetVisualStatus(VisualAlphaStatus.Opaque);
                }
            }
        }
        private void ToggleValueChanged(bool value)
        {
            UpdateCounter();
            CompareCurrentAndMax(value);
        }
        private void SetCategories(List<CategoryObject> categories)
        {
            _categories = categories;

            foreach (var category in _categories)
                category.Toggle.onValueChanged.AddListener(ToggleValueChanged);
        }
        private void UpdateCounter()
        {
            int newCurrent = 0;

            foreach (var item in _categories)
            {
                if (item.Toggle.isOn)
                    newCurrent++;
            }

            _currentNumberSelectedCategories = newCurrent;

            onCounterUpdate?.Invoke();
        }

        private enum VisualAlphaStatus
        {
            Opaque = 255,
            LittleTransparent = 170,
            LittleOpaque = 85,
            Transparent = 0,
        }
    }
}