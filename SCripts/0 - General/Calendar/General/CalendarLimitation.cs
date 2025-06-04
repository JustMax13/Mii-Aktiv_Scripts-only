using System;
using UnityEngine;

namespace General.Calendar
{
    public class CalendarLimitation : MonoBehaviour
    {
        [SerializeField] private Calendar _calendar;

        [SerializeField] private bool _startFromToday;
        [SerializeField] private bool _includesToday;

        [SerializeField] private int _numberDaysToChoose;

        private void Awake()
        {
            _calendar.CalendarUpdate += UpdateLimit;
        }

        private void UpdateLimit()
        {
            foreach (var day in _calendar.Days)
                day.Button.interactable = true;
            
            if (_startFromToday)
            {
                DateTime limitStartDate = DateTime.Now.AddDays(_numberDaysToChoose);

                if (_includesToday)
                {
                    foreach (var day in _calendar.Days)
                    {
                        if (!day.VisualTurnOn)
                            continue;

                        if (day.Value <= DateTime.Now || day.Value > limitStartDate)
                            day.Button.interactable = false;
                    }
                }
                else
                {
                    foreach (var day in _calendar.Days)
                    {
                        if (!day.VisualTurnOn)
                            continue;

                        if (day.Value < DateTime.Now || day.Value > limitStartDate)
                            day.Button.interactable = false;
                    }
                }
            }
            else
            { // колись можливо реалізувати обмеження календаря, не починаючи з сьогоднішнього дня
                throw new NotImplementedException();
            }
        }
    }
}