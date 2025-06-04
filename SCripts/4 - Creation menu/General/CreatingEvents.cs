using General;
using General.Data;
using General.Database;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class CreatingEvents : MonoBehaviour
    {
        public async void CreateEvent()
        {
            var eventData = WriteToEventData();

            int charPartLength = 5;
            string secondKeyPart = RandomString.Generate(charPartLength);
            string eventID = eventData.DateAndTimeOfCreationEvent + secondKeyPart;

            if (CreateInterfaceValues.Avatar.sprite.texture == null)
                throw new Exception("Іконка до події пуста, неможливо створити івент");
            if (CreateInterfaceValues.DefaultAvatar.texture != eventData.Icon)
                eventData.LinkToAvatarInStorage = CreateIconPath(eventID);
            else
                eventData.LinkToAvatarInStorage = "";

            var canCreate = await DatabaseAuth.CheckCurrentUserOnDisable();
            if (!canCreate)
            {
                Debug.Log("Додати реалізацію, коли при створені події акаунт відключили");
                return;
            }

            if (eventData.LinkToAvatarInStorage != "")
                await DatabaseActions.UploadImageAsync(eventData.Icon, eventData.LinkToAvatarInStorage);

            DatabaseActions.SaveEvent(eventData, eventID);
            ClearCreateInterface.Clear();
        }
        private EventData WriteToEventData()
        {
            var eventData = new EventData();

            eventData.Name = CreateInterfaceValues.Name.text;
            eventData.ShortInfo = CreateInterfaceValues.ShortInfo.text;
            eventData.FullInfo = CreateInterfaceValues.FullInfo.text;
            eventData.TelegramLink = CreateInterfaceValues.TelegramLink.text;
            eventData.RegisterFormLink = CreateInterfaceValues.RegisterFormLink.text;
            eventData.Tags = CreateInterfaceValues.ChooseTagsID;
            eventData.Icon = CreateInterfaceValues.Avatar.sprite.texture;
            {
                string eventEndTime = CreateInterfaceValues.TextDateValue.SelectedDate.Year.ToString();

                var month = CreateInterfaceValues.TextDateValue.SelectedDate.Month.ToString();
                if (CreateInterfaceValues.TextDateValue.SelectedDate.Month < 10)
                    month = "0" + month;
                eventEndTime += month;

                var day = CreateInterfaceValues.TextDateValue.SelectedDate.Day.ToString();
                if (CreateInterfaceValues.TextDateValue.SelectedDate.Day < 10)
                    day = "0" + day;
                eventEndTime += day;

                var hour = CreateInterfaceValues.TextTimeValue.SelectedTime.Hour.ToString();
                if (CreateInterfaceValues.TextTimeValue.SelectedTime.Hour < 10)
                    hour = "0" + hour;
                eventEndTime += hour;

                var minute = CreateInterfaceValues.TextTimeValue.SelectedTime.Minute.ToString();
                if (CreateInterfaceValues.TextTimeValue.SelectedTime.Minute < 10)
                    minute = "0" + minute;
                eventEndTime += minute;

                eventData.EventEndTime = long.Parse(eventEndTime);
            }

            return eventData;
        }

        /// <returns> Повертає шлях до збереженого аватару</returns>
        private string CreateIconPath(string eventID)
        {
            string pathToSaveImages = DatabaseActions.PathToEventImageFolder + "/" + eventID;
            string iconPath = pathToSaveImages + "/icon";

            return iconPath;
        }
    }
}