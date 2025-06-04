using General.Data;
using MainMenu;
using System.Collections.Generic;
using UnityEngine;
using ViewMenu;

namespace General.Tracking
{
    public class TagJoinTracker : MonoBehaviour, ITracker
    {
        [SerializeField] private CategoriesJoinTrackingSystem _tagJoinTrackingSystem;
        [SerializeField] private string _nameInJson;

        private ITrackingSystem _trackingSystem;
        public string NameInJson { get => _nameInJson; private set => _nameInJson = value; }
        public Dictionary<int, int> Counters { get; private set; }
        public ITracker ThisTracker => this;

        public ITrackingSystem TrackingSystem { get => _trackingSystem; private set => _trackingSystem = value; }

        private void Awake()
        {
            _trackingSystem = _tagJoinTrackingSystem;
            ThisTracker.Instantiate(TrackingSystem);

            Counters = new Dictionary<int, int>();
        }
        public void AddInfo()
        {
            foreach (var item in ViewInterfaceField.EventData.Tags)
                ThisTracker.AddToCounter(item);
        }
    }
}