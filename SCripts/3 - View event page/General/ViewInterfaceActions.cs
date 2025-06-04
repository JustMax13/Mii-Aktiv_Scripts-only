using Firebase.Firestore;
using General.Data;
using General.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ViewMenu
{
    public class ViewInterfaceActions : MonoBehaviour
    {
        private static string _defaultText = "Інформація відсутня";
        public static Action<EventData> SetValueIsEndWithResult;
        static ViewInterfaceActions()
        {
            SetValueIsEndWithResult = null;
        }
        public static void SetValueToViewInterface(EventData eventInfo)
        {
            ViewInterfaceField.EventData = eventInfo;

            try 
            {
                if (eventInfo.Name == null || eventInfo.Name == "")
                    ViewInterfaceField.Name.text = _defaultText;
                else
                    ViewInterfaceField.Name.text = eventInfo.Name;
            }
            catch { ViewInterfaceField.Name.text = _defaultText; }
            try
            {
                DateTime startDate = EventData.FromYMDHM(eventInfo.EventEndTime.ToString());

                string month = "";
                switch (startDate.Month)
                {
                    case 1:
                        {
                            month = "січ";
                            break;
                        }
                    case 2:
                        {
                            month = "лют";
                            break;
                        }
                    case 3:
                        {
                            month = "бер";
                            break;
                        }
                    case 4:
                        {
                            month = "квіт";
                            break;
                        }
                    case 5:
                        {
                            month = "трав";
                            break;
                        }
                    case 6:
                        {
                            month = "черв";
                            break;
                        }
                    case 7:
                        {
                            month = "лип";
                            break;
                        }
                    case 8:
                        {
                            month = "серп";
                            break;
                        }
                    case 9:
                        {
                            month = "вер";
                            break;
                        }
                    case 10:
                        {
                            month = "жовт";
                            break;
                        }
                    case 11:
                        {
                            month = "лист";
                            break;
                        }
                    case 12:
                        {
                            month = "груд";
                            break;
                        }
                }

                string hour = "";
                if (startDate.Hour < 10)
                    hour = "0" + startDate.Hour;
                else
                    hour = startDate.Hour.ToString();
                string minute = "";
                if (startDate.Minute < 10)
                    minute = "0" + startDate.Minute;
                else
                    minute = startDate.Minute.ToString();

                ViewInterfaceField.StartDate.text = startDate.Day + " " + month + ", " + hour + ":" + minute;
            }
            catch { ViewInterfaceField.StartDate.text = _defaultText; }
            try 
            {
                if (eventInfo.ShortInfo == null || eventInfo.ShortInfo == "")
                    ViewInterfaceField.ShortInfo.text = _defaultText;
                else
                    ViewInterfaceField.ShortInfo.text = eventInfo.ShortInfo;
            }
            catch { ViewInterfaceField.ShortInfo.text = _defaultText; }
            try  
            {
                if (eventInfo.FullInfo == null || eventInfo.FullInfo == "")
                    ViewInterfaceField.FullInfo.text = _defaultText;
                else
                    ViewInterfaceField.FullInfo.text = eventInfo.FullInfo;
            }
            catch { ViewInterfaceField.FullInfo.text = _defaultText; }
            try 
            {
                if (eventInfo.TelegramLink == null || eventInfo.TelegramLink == "")
                    ViewInterfaceField.TelegramButton.interactable = false;
                else
                {
                    ViewInterfaceField.TelegramButton.interactable = true;
                    ViewInterfaceField.TelegramLink.Link = eventInfo.TelegramLink;
                }
            }
            catch { ViewInterfaceField.TelegramButton.interactable = false; }
            try 
            {
                if (eventInfo.RegisterFormLink == null || eventInfo.RegisterFormLink == "")
                    ViewInterfaceField.RegisterFormLink.Link = _defaultText;
                else
                    ViewInterfaceField.RegisterFormLink.Link = eventInfo.RegisterFormLink;
            }
            catch { ViewInterfaceField.RegisterFormLink.Link = null; }
            try 
            {
                if (eventInfo.Icon != null)
                    ViewInterfaceField.Avatar.texture = eventInfo.Icon;
            }
            catch { Debug.LogWarning("Аватарка не була завантажена"); }

            SetValueIsEndWithResult?.Invoke(eventInfo);
        }
    }
}