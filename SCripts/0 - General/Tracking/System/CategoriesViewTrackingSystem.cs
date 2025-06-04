using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace General.Tracking
{
    public class CategoriesViewTrackingSystem : MonoBehaviour, ITrackingSystem
    {
        public int ActionForSave => 200;
        string ITrackingSystem.SystemIDName { get; set; } = "TagViewTrackingSystem";
        string ITrackingSystem.JsonPath { get; set; }
        List<ITracker> ITrackingSystem.Trackers { get; set; } = new List<ITracker>();
        public ITrackingSystem ThisTrackingSystem { get; private set; }

        private void Awake()
        {
            ThisTrackingSystem = this;
            ThisTrackingSystem.Initialization();

            ThisTrackingSystem.CallOnAwake();
        }
        private void OnApplicationQuit()
        {
            ThisTrackingSystem.CallOnApplicationQuit();
        }
    }
}