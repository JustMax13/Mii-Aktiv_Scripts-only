using Firebase.Firestore;
using Firebase.Storage;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using Unity.VisualScripting;
using Tests;
using General.Database;
using System.Threading.Tasks;

namespace General.Data
{
    [FirestoreData]
    public class EventData
    {
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string LinkToAvatarInStorage { get; set; }
        [FirestoreProperty] public string ShortInfo { get; set; }
        [FirestoreProperty] public string FullInfo { get; set; }
        [FirestoreProperty] public string TelegramLink { get; set; }
        [FirestoreProperty] public string RegisterFormLink { get; set; }
        /// <summary>
        /// Зберігати дані в форматі YMDHMS:
        /// Y - year,
        /// M - month,
        /// D - day,
        /// H - hour,
        /// M - minute,
        /// S - second.
        /// </summary>
        [FirestoreProperty] public long DateAndTimeOfCreationEvent { get; set; }
        /// <summary>
        /// Зберігати дані в форматі YMDHM:
        /// Y - year,
        /// M - month,
        /// D - day,
        /// H - hour,
        /// M - minute.
        /// </summary>
        [FirestoreProperty] public long EventEndTime { get; set; }
        [FirestoreProperty] public int[] Tags { get; set; }

        public Texture2D Icon { get; set; }

        public EventData()
        {
            string YMDHMS = ToYMDHMS(DateTime.Now);
            DateAndTimeOfCreationEvent = long.Parse(YMDHMS);
        }

        public static async Task<EventData> CreateByDatabaseId(string eventId)
        {
            var firestoreSnapshot = await DatabaseActions.RefToFirestore.Collection(DatabaseActions.FirestorePathToEvent)
                .Document(eventId).GetSnapshotAsync();
            var realtimeSnapshot = await DatabaseActions.RefToRealtime.Child(DatabaseActions.RealtimePathToEvent)
                .Child(eventId).GetValueAsync();

            if (!firestoreSnapshot.Exists || !realtimeSnapshot.Exists)
            {
                Debug.LogWarning("Неправильно вказано eventId. Повернуто пустий eventData");
                return new EventData();
            }

            var eventData = new EventData();

            eventData.Name = realtimeSnapshot.Child("Name").Value.ToString();
            eventData.DateAndTimeOfCreationEvent = (long)realtimeSnapshot.Child("DateAndTimeOfCreationEvent").Value;
            eventData.EventEndTime = (long)realtimeSnapshot.Child("EventEndTime").Value;
            var tagsSnapshot = realtimeSnapshot.Child("Tags");
            eventData.Tags = new int[tagsSnapshot.ChildrenCount];
            int i = 0;
            foreach (var tag in tagsSnapshot.Children)
                eventData.Tags[i++] = tag.Value.ConvertTo<int>(); // тут почему-то обрывается

            var firestoreEventData = firestoreSnapshot.ConvertTo<EventData>();
            eventData.LinkToAvatarInStorage = firestoreEventData.LinkToAvatarInStorage;
            eventData.ShortInfo = firestoreEventData.ShortInfo;
            eventData.FullInfo = firestoreEventData.FullInfo;
            eventData.TelegramLink = firestoreEventData.TelegramLink;
            eventData.RegisterFormLink = firestoreEventData.RegisterFormLink;

            return eventData;
        }
        public static string ToYMDHM(DateTime dateTime)
        {
            string yearMonthDay = dateTime.Year.ToString();

            int month = DateTime.Now.Month;
            if (month < 10)
                yearMonthDay += "0" + month;
            else
                yearMonthDay += month;

            int day = DateTime.Now.Day;
            if (day < 10)
                yearMonthDay += "0" + day;
            else
                yearMonthDay += day;

            string hourMinute = "";

            int hour = DateTime.Now.Hour;
            if (hour < 10)
                hourMinute += "0" + hour;
            else
                hourMinute += hour;

            int minute = DateTime.Now.Minute;
            if (minute < 10)
                hourMinute += "0" + minute;
            else
                hourMinute += minute;

            string YMDHM = yearMonthDay + hourMinute;

            return YMDHM;
        }
        public static string ToYMDHMS(DateTime dateTime)
        {
            string YMDHM = ToYMDHM(dateTime);
            string YMDHMS = "";

            int second = DateTime.Now.Second;
            if (second < 10)
                YMDHMS = YMDHM + "0" + second;
            else
                YMDHMS = YMDHM + second;

            return YMDHMS;
        }
        public static DateTime FromYMDHM(string YMDHM)
        {
            // 0123456789 10 11
            // YYYYMMDDHH M  M
            string year = YMDHM.Substring(0, 4);
            string month = YMDHM.Substring(4, 2);
            string day = YMDHM.Substring(6, 2);
            string hour = YMDHM.Substring(8, 2);
            string minute = YMDHM.Substring(10, 2);

            int intYear = int.Parse(year);
            int intMonth = int.Parse(month);
            int intDay = int.Parse(day);
            int intHour = int.Parse(hour);
            int intMinute = int.Parse(minute);

            return new DateTime(intYear, intMonth, intDay, intHour, intMinute, 0);
        }
        public static DateTime FromYMDHMS(string YMDHMS)
        {
            // 0123456789 10 11 12 13
            // YYYYMMDDHH M  M  S  S
            DateTime DateTime_YMDHM = FromYMDHM(YMDHMS);

            string second = YMDHMS.Substring(12, 2);
            int intSecond = int.Parse(second);

            return new DateTime(DateTime_YMDHM.Year, DateTime_YMDHM.Month, DateTime_YMDHM.Day,
                DateTime_YMDHM.Hour, DateTime_YMDHM.Minute, DateTime_YMDHM.Second);
        }
    }
}