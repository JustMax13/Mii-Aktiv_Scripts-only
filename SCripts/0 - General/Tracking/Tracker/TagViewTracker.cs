using Filters.FilteringValues.Category;
using General.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewMenu;

namespace General.Tracking
{
    public class TagViewTracker : MonoBehaviour, ITracker
    {
        [SerializeField] private CategoriesViewTrackingSystem _tagViewTrackingSystem;
        [SerializeField] private string _nameInJson;

        private ITrackingSystem _trackingSystem;
        public string NameInJson { get => _nameInJson; private set => _nameInJson = value; }
        public Dictionary<int, int> Counters { get; private set; }
        public ITracker ThisTracker => this;

        public ITrackingSystem TrackingSystem { get => _trackingSystem; private set => _trackingSystem = value; }

        private void Awake()
        {
            _trackingSystem = _tagViewTrackingSystem;
            ThisTracker.Instantiate(TrackingSystem);

            Counters = new Dictionary<int, int>();

            ViewInterfaceActions.SetValueIsEndWithResult += (eventData) =>
            {
                foreach (var item in eventData.Tags)
                    ThisTracker.AddToCounter(item);
            };
        }
    }
}