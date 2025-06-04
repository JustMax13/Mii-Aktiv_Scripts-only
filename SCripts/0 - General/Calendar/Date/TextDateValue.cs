using CreateMenu;
using System;
using TMPro;
using UnityEngine;

namespace General.Calendar
{
    public class TextDateValue : FieldIsFilled
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private DateFormat _dateInText;

        private string _defaultTextValue;
        private DateTime _selectedDate;

        public TextMeshProUGUI Tmp { get => _tmp; private set => _tmp = value; }
        public Calendar Calendar { get => _calendar; private set => _calendar = value; }
        public DateTime SelectedDate { get => _selectedDate; private set => _selectedDate = value; }
        public string DefaultTextValue { get => _defaultTextValue; private set => _defaultTextValue = value; }


        private void Awake()
        {
            Calendar.ChangeSelectedDate += SetDate;
            DefaultTextValue = Tmp.text;
        }

        private void SetDate(DateTime date)
        {
            SelectedDate = date;

            switch (_dateInText)
            {
                case DateFormat.Point:
                    {
                        var day = SelectedDate.Day.ToString();
                        var month = SelectedDate.Month.ToString();

                        Tmp.text = day + "." + month;
                        break;
                    }
                case DateFormat.WithMonthName:
                    {
                        var day = SelectedDate.Day.ToString();
                        string month = "";

                        switch (SelectedDate.Month)
                        {
                            case 1:
                                {
                                    month = "січня";
                                    break;
                                }
                            case 2:
                                {
                                    month = "лютого";
                                    break;
                                }
                            case 3:
                                {
                                    month = "березня";
                                    break;
                                }
                            case 4:
                                {
                                    month = "квітня";
                                    break;
                                }
                            case 5:
                                {
                                    month = "травня";
                                    break;
                                }
                            case 6:
                                {
                                    month = "червня";
                                    break;
                                }
                            case 7:
                                {
                                    month = "липня";
                                    break;
                                }
                            case 8:
                                {
                                    month = "серпня";
                                    break;
                                }
                            case 9:
                                {
                                    month = "вересня";
                                    break;
                                }
                            case 10:
                                {
                                    month = "жовтня";
                                    break;
                                }
                            case 11:
                                {
                                    month = "листопада";
                                    break;
                                }
                            case 12:
                                {
                                    month = "грудня";
                                    break;
                                }
                        }

                        Tmp.text = day + " " + month;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void UpdateTaskCompletion()
        {
            if (Tmp.text != _defaultTextValue)
                TaskCompletion = true;
            else
                TaskCompletion = false;
        }

        private enum DateFormat
        {
            Point,
            WithMonthName,
        }
    }
}