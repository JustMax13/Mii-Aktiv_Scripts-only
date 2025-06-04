using System.Collections.Generic;
using UnityEngine;

namespace General.Tracking
{
    public class CategoriesJoinTrackingSystem : MonoBehaviour, ITrackingSystem
    {
        public int ActionForSave => 50;
        string ITrackingSystem.SystemIDName { get; set; } = "TagJoinTrackingSystem";
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
