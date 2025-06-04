using System;
using TMPro;
using UnityEngine;

namespace General.Calendar
{
    public class Calendar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _monthName;
        [SerializeField] private DayValue[] _days;

        private DateTime _selectedMonth;
        private DateTime _selectedDate;
        private DayValue _selectedDay;

        public Action<DateTime> ChangeSelectedDate;
        public Action CalendarUpdate;

        public DateTime SelectedMonth { get => _selectedMonth; private set => _selectedMonth = value; }
        public DateTime SelectedDate { get => _selectedDate; private set => _selectedDate = value; }
        public DayValue[] Days { get => _days; private set => _days = value; }

        private void Awake()
        {
            if (Days.Length < 31) // 31 - максимальна кількість днів у місяці
                throw new System.Exception(gameObject + " у скрипті Calendar має кількість днів меншу за 31");

            SelectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            _selectedDay = null;
        }

        public void OnDayClick(DayValue day)
        {
            if (_selectedDay != null)
            {
                int selectedDay = int.Parse(_selectedDay.Tmp.text);
                if (selectedDay == DateTime.Now.Day)
                    _selectedDay.SetCurrentDaySprite();
                else
                    _selectedDay.SetDefaultSprite();
            }

            SelectedDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, int.Parse(day.Tmp.text));
            day.SetSelectDaySprite();
            _selectedDay = day;

            ChangeSelectedDate?.Invoke(SelectedDate);
        }
        public void ClearSelectDay()
        {
            if (_selectedDay != null)
            {
                SelectedDate = new DateTime();

                _selectedDay.SetDefaultSprite();
                _selectedDay = null;
            }
        }
        public void ShowCurrentMonth()
        {
            SelectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (SelectedDate != null)
            {
                if (SelectedDate.Month == SelectedMonth.Month && SelectedDate.Year == SelectedMonth.Year)
                    SetViewCalendar(DateTime.Now.Day, SelectedDate.Day);
                else
                    SetViewCalendar(DateTime.Now.Day);
            }
            else
                SetViewCalendar(DateTime.Now.Day);
        }
        public void ShowNextMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(1);

            if (SelectedDate != null)
            {
                if (SelectedDate.Month == SelectedMonth.Month && SelectedDate.Year == SelectedMonth.Year)
                {
                    if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                        SetViewCalendar(DateTime.Now.Day, SelectedDate.Day);
                    else
                        SetViewCalendar(0, SelectedDate.Day);
                }
                else
                {
                    if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                        SetViewCalendar(DateTime.Now.Day);
                    else
                        SetViewCalendar();
                }
            }
            else
            {
                if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                    SetViewCalendar(DateTime.Now.Day);
                else
                    SetViewCalendar();
            }
        }
        public void ShowLastMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(-1);

            if (SelectedDate != null)
            {
                if (SelectedDate.Month == SelectedMonth.Month && SelectedDate.Year == SelectedMonth.Year)
                {
                    if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                        SetViewCalendar(DateTime.Now.Day, SelectedDate.Day);
                    else
                        SetViewCalendar(0, SelectedDate.Day);
                }
                else
                {
                    if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                        SetViewCalendar(DateTime.Now.Day);
                    else
                        SetViewCalendar();
                }
            }
            else
            {
                if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                    SetViewCalendar(DateTime.Now.Day);
                else
                    SetViewCalendar();
            }
        }
        public void ShowSelectedMonth()
        {
            if (_selectedDay == null)
            {
                ShowCurrentMonth();
                return;
            }

            SelectedMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);

            if (SelectedMonth.Month == DateTime.Now.Month && SelectedMonth.Year == DateTime.Now.Year)
                SetViewCalendar(DateTime.Now.Day, SelectedDate.Day);
            else
                SetViewCalendar(0, SelectedDate.Day);
        }
        public void SetViewCalendar(int currentDate = 0, int selectedDate = 0)
        {
            if (SelectedMonth.DayOfWeek == 0)
                SetDays(DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month),
                    6,
                    currentDate, selectedDate);
            else
                SetDays(DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month),
                    (int)SelectedMonth.DayOfWeek - 1,
                    currentDate, selectedDate);

            SetMonthAndYear(SelectedMonth.Month, SelectedMonth.Year);

            CalendarUpdate?.Invoke();
        }
        private void SetDays(int numberDaysInMonth, int skipInLineup, int currentDate = 0, int selectedDate = 0)
        {
            //                              ----------------   Check start ----------------
            if (numberDaysInMonth < 28)
            {
                numberDaysInMonth = 28;
                Debug.LogWarning("currentDate вказано не корректно, numberDaysInMonth = " + numberDaysInMonth);
            }
            else if (numberDaysInMonth > 31)
            {
                numberDaysInMonth = 31;
                Debug.LogWarning("currentDate вказано не корректно, numberDaysInMonth = " + numberDaysInMonth);
            }
            else if (numberDaysInMonth > Days.Length)
                throw new System.Exception("numberDaysInMonth > _days.Length");

            if (skipInLineup > Days.Length - numberDaysInMonth)
            {
                skipInLineup = Days.Length - numberDaysInMonth;
                Debug.LogWarning("кількість пропусків ( skipInLineup ) днів більша за > _days.Length - numberDaysInMonth." +
                    " skipInLineup = " + skipInLineup);
            }
            else if (skipInLineup < 0)
                throw new Exception("skipInLineup < 0");

            if (currentDate > numberDaysInMonth)
            {
                currentDate = 0;
                Debug.LogWarning("currentDate вказана не корректно, currentDate = " + currentDate);
            }
            else if (currentDate < 0)
                throw new Exception("currentDate < 0");

            if (selectedDate > numberDaysInMonth)
            {
                selectedDate = 0;
                Debug.LogWarning("selectedDate вказана не корректно, selectedDate = " + currentDate);
            }
            else if (selectedDate < 0)
                throw new Exception("selectedDate < 0");

            //                              ----------------   Set start ----------------
            foreach (var day in Days)
                day.TurnOnVisual();

            for (int i = 0, passesLeft = skipInLineup, day = 1; i < Days.Length; i++)
            {
                if (passesLeft > 0)
                {
                    Days[i].TurnOffVisual();
                    passesLeft--;
                    continue;
                }

                if (day <= numberDaysInMonth)
                {
                    Days[i].Tmp.text = day.ToString();
                    Days[i].Value = new DateTime(_selectedMonth.Year, _selectedMonth.Month, day++);
                    Days[i].SetDefaultSprite();
                }
                else
                {
                    while (i < Days.Length)
                        Days[i++].TurnOffVisual();

                    break;
                }
            }

            if (currentDate != 0)
                Days[skipInLineup + currentDate - 1].SetCurrentDaySprite(); //  - 1 тому що індекс
            if (selectedDate != 0)
                Days[skipInLineup + selectedDate - 1].SetSelectDaySprite();
        }
        private void SetMonthAndYear(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new Exception("Неправильно введено місяц");

            switch (month)
            {
                case 1:
                    {
                        _monthName.text = "Січень";
                        break;
                    }
                case 2:
                    {
                        _monthName.text = "Лютий";
                        break;
                    }
                case 3:
                    {
                        _monthName.text = "Березень";
                        break;
                    }
                case 4:
                    {
                        _monthName.text = "Квітень";
                        break;
                    }
                case 5:
                    {
                        _monthName.text = "Травень";
                        break;
                    }
                case 6:
                    {
                        _monthName.text = "Червень";
                        break;
                    }
                case 7:
                    {
                        _monthName.text = "Липень";
                        break;
                    }
                case 8:
                    {
                        _monthName.text = "Серпень";
                        break;
                    }
                case 9:
                    {
                        _monthName.text = "Вересень";
                        break;
                    }
                case 10:
                    {
                        _monthName.text = "Жовтень";
                        break;
                    }
                case 11:
                    {
                        _monthName.text = "Листопад";
                        break;
                    }
                case 12:
                    {
                        _monthName.text = "Грудень";
                        break;
                    }
            }

            _monthName.text += ", " + year;
        }
    }
}