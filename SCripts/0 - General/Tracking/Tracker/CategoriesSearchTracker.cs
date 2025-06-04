using Filters;
using Filters.FilteringValues.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewMenu;

namespace General.Tracking
{
    public class CategoriesSearchTracker : MonoBehaviour, ITracker
    {
        [SerializeField] private TagSearchTrackingSystem _tagSearchTrackingSystem;
        [SerializeField] private string _nameInJson;
        [SerializeField] private ApplyCategories applyFilter;

        private ITrackingSystem _trackingSystem;
        public string NameInJson { get => _nameInJson; private set => _nameInJson = value; }
        public Dictionary<int, int> Counters { get; private set; }
        public ITracker ThisTracker => this;

        public ITrackingSystem TrackingSystem { get => _trackingSystem; private set => _trackingSystem = value; }

        private void Awake()
        {
            _trackingSystem = _tagSearchTrackingSystem;
            ThisTracker.Instantiate(TrackingSystem);

            Counters = new Dictionary<int, int>();

            applyFilter.GetApplyCategories += AddInfo;
        }

        public void AddInfo(List<CategoryObject> tags)
        {
            foreach (var tag in tags)
                ThisTracker.AddToCounter(tag.ID);
        }
    }
}