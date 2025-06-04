using Filters.FilteringValues.Category;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Filters
{
    public class ApplyCategories : MonoBehaviour
    {
        [SerializeField] private CategoriesActions _categoriesActions;
        
        public Action<List<CategoryObject>> GetApplyCategories;
        public Action CategoriesApply;

        public List<CategoryObject> AppliedCategories { get; set; }

        public void Apply()
        {
            AppliedCategories = _categoriesActions.GetChooseCategories();

            GetApplyCategories?.Invoke(AppliedCategories);
            CategoriesApply?.Invoke();
        }
    }
}