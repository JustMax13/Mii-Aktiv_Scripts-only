using System;
using UnityEngine;

namespace General.ChooseTime
{
    public class CentralTimePanels : MonoBehaviour
    {
        [SerializeField] private CentralNumberPanel _hour;
        [SerializeField] private CentralNumberPanel _minute;

        /// <summary>
        /// T1 - hour, T2 - minute
        /// </summary>
        public Action<TimePanel, TimePanel> OnApplyCenterPanels;

        public void ApplyCentralPanels()
            => OnApplyCenterPanels?.Invoke(_hour.PanelInCenter, _minute.PanelInCenter);
    }
}