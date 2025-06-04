using Filters;
using Filters.FilteringValues.Category;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CreateMenu
{
    public class ChoosesCategory : MonoBehaviour
    {
        [SerializeField] private CategoryObject _selectCategoryPrefab;

        [SerializeField] private RectTransform _contentForSpawn;
        [SerializeField] private ApplyCategories _applyCategories;

        public List<CategoryObject> CategoryObjects { get; private set; }
        private void Awake()
        {
            CategoryObjects = new List<CategoryObject>();
            _applyCategories.GetApplyCategories += InstantiateCategory;
        }
        public void InstantiateCategory(List <CategoryObject> categories)
        {
            CategoryObjects = new List<CategoryObject>();

            for (int i = 0; i < categories.Count; i++)
                CategoryObjects.Add(Instantiate(_selectCategoryPrefab, _contentForSpawn));

            for (int i = 0; i < categories.Count; i++)
                CategoryObjects.ElementAt(i).Name.text = categories.ElementAt(i).Name.text;
        }
    }
}