using Filters.FilteringValues.Category;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Storage;
using General.EventAction;
using General.Value;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EventData = General.Data.EventData;

namespace General.Database
{
    public class DatabaseActions : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _devDB;

        private static bool _databaseInitializationComplete;

        private static string _pathToDbStateOnUpdates = "Db state on updates";
        private static string _pathToSupportedVersionsRT = "Supported versions";
        private static string _pathToUpdateAppLinkRT = "UpdateAppLink";
        private static string _firestorePathToEvent = "EventData";
        private static string _realtimePathToEvent = "EventData";
        private static string _realtimePathToTagsList = "TagsList";
        private static string _pathToEventImageFolder = "Events";
        private static string _pathInEventImageFolderToIcon = "icon/icon";

        private static DatabaseReference _refToRealtime;
        private static FirebaseFirestore _refToFirestore;
        private static FirebaseStorage _storage;
        private static StorageReference _refToStorage;

        private static DeleteEvent _deleteEvent;

        public static bool DatabaseInitializationComplete { get => _databaseInitializationComplete; set => _databaseInitializationComplete = value; }

        public static string PathToDbStateOnUpdates { get => _pathToDbStateOnUpdates; }
        public static string PathToSupportedVersionsRT { get => _pathToSupportedVersionsRT; }
        public static string PathToUpdateAppLinkRT { get => _pathToUpdateAppLinkRT; }
        public static string FirestorePathToEvent { get => _firestorePathToEvent; }
        public static string RealtimePathToEvent { get => _realtimePathToEvent; }
        public static string RealtimePathToTagsList { get => _realtimePathToTagsList; }
        public static string PathToEventImageFolder { get => _pathToEventImageFolder; }
        public static string PathInEventImageFolderToIcon { get => _pathInEventImageFolderToIcon; }

        public static DatabaseReference RefToRealtime { get => _refToRealtime; }
        public static FirebaseFirestore RefToFirestore { get => _refToFirestore; }
        public static StorageReference RefToStorage { get => _refToStorage; }
        public static FirebaseStorage Storage { get => _storage; }
        private void Awake()
        {
            _refToRealtime = FirebaseDatabase.DefaultInstance.RootReference;
            _refToFirestore = FirebaseFirestore.DefaultInstance;
            _storage = FirebaseStorage.DefaultInstance;

            if (_devDB)
            {
                _refToStorage = _storage.GetReferenceFromUrl("gs://mii-aktive---dev.appspot.com/");
                Debug.Log("Використовується DEV db");
            }
            else
            {
                _refToStorage = _storage.GetReferenceFromUrl("gs://mii-aktiv.appspot.com");
                Debug.Log("Використовується USER db");
            }

            _deleteEvent = new DeleteEvent();
            var deleteTask = DeletePastEventsAsync(10);

            var allInitTask = new List<Task>()
            {
                deleteTask
            };
            var allTaskComplete = Task.WhenAll(allInitTask);

            allTaskComplete.ContinueWith((task) =>
            {
                _databaseInitializationComplete = true;
            });
        }

        public static async Task DeletePastEventsAsync(int countDeletions)
        {
            var snapshot = await _refToRealtime.Child(RealtimePathToEvent)
                .OrderByChild("EventEndTime")
                .LimitToLast(countDeletions)
                .GetValueAsync();

            long currentDateTime = long.Parse(EventData.ToYMDHM(DateTime.Now));

            var eventForDelete = new List<string>();
            foreach (var someEvent in snapshot.Children)
            {
                long value = (long)someEvent.Child("EventEndTime").Value;
                if (value <= currentDateTime)
                    eventForDelete.Add(someEvent.Key);
            }

            if (eventForDelete.Count > 0)
                await _deleteEvent.FullDeleteAsync(eventForDelete);
        }
        public static async Task<byte[]> GetBitesArray(string pathInStorage)
        {
            StorageReference imageRef = _refToStorage.Child(pathInStorage + ActionWithTexture2D.TextureExpansion);

            byte[] imageBytes = null;
            try
            {
                imageBytes = await imageRef.GetBytesAsync(long.MaxValue);
            }
            catch
            {
                Debug.LogWarning("Картинка не завантажилася, не отримано потік байтів");
                return null;
            }

            return imageBytes;
        }
        public static async Task<List<string>> GetEvents(int elementCount, string startAfterEventWithId = "")
        {
            if (elementCount < 0)
                elementCount = 0;

            List<string> collection = new List<string>();

            if (elementCount != 0)
            {
                if (startAfterEventWithId == "")
                {
                    var query = _refToRealtime.Child(_realtimePathToEvent)
                       .LimitToLast(elementCount);

                    DataSnapshot snapshot = await query.GetValueAsync();

                    foreach (var item in snapshot.Children)
                        collection.Add(item.Key);
                }
                else
                {
                    var snapshot = await _refToRealtime.Child(_realtimePathToEvent)
                      .OrderByKey()
                      .EndAt(startAfterEventWithId)
                      .LimitToLast(elementCount + 1)
                      .GetValueAsync();

                    foreach (var item in snapshot.Children)
                    {
                        if (item.Key == snapshot.Children.Last().Key)
                            break;

                        collection.Add(item.Key);
                    }
                }
            }

            if (collection == null || collection.Count == 0)
                Debug.LogWarning("Повернута колекція розміром у 0 елементів, або рівна null. Можливо некоректно вказано стартовий елемент.");

            return collection;
        }
        public static async Task<List<string>> GetEventsWithTags(List<CategoryObject> tags, int elementCount, string startAfterEventWithId = "")
        {
            if (elementCount < 0)
                elementCount = 0;

            List<string> collection = new List<string>();

            if (tags.Count == 0 || tags == null)
            {
                collection = await GetEvents(elementCount, startAfterEventWithId);
                Debug.LogWarning("Фільтри не були введені. Повернуто колекцію без врахування фільтрів");
                return collection;
            }

            if (elementCount != 0)
            {
                var taskArray = new Task<DataSnapshot>[tags.Count];

                if (startAfterEventWithId == "")
                {
                    for (int i = 0; i < tags.Count; i++)
                        taskArray[i] = _refToRealtime.Child(_realtimePathToTagsList)
                            .Child(tags[i].ID.ToString())
                            .LimitToLast(elementCount)
                            .GetValueAsync();
                }
                else
                {
                    for (int i = 0; i < tags.Count; i++)
                        taskArray[i] = _refToRealtime.Child(_realtimePathToTagsList)
                            .Child(tags[i].ID.ToString())
                            .OrderByKey()
                            .EndAt(startAfterEventWithId)
                            .LimitToLast(elementCount + 1)
                            .GetValueAsync();
                }
                await Task.WhenAll(taskArray);

                var allEventsDataSnapshot = new List<DataSnapshot>();
                foreach (var task in taskArray)
                    allEventsDataSnapshot = allEventsDataSnapshot.Concat(task.Result.Children).ToList();

                var allEventId = new List<string>();
                foreach (var child in allEventsDataSnapshot)
                    allEventId.Add(child.Key);
                allEventId = allEventId.Distinct().ToList();
                allEventId.Sort();

                if (startAfterEventWithId != "")
                    allEventId.Remove(startAfterEventWithId);

                int index = elementCount;
                if (index > allEventId.Count)
                    index = 0;
                else
                    index = allEventId.Count - elementCount;
                for (int i = 0; i < elementCount && i < allEventId.Count; i++, index++)
                    collection.Add(allEventId.ElementAt(index));
            }

            if (collection == null)
            {
                Debug.LogWarning("Колекція рівна null. Повертаємо пусту колекцію. Можливо некоректно вказано стартовий елемент.");
                return new List<string>();
            }

            return collection;
        }
        public static async Task<ImageValues> GetImageValues(string pathInStorage)
        {
            StorageReference jsonRef = _refToStorage.Child(pathInStorage + ".json");

            ImageValues imageValues = null;
            try
            {
                Stream stream = await jsonRef.GetStreamAsync();

                await Task.Run(() =>
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonString = reader.ReadToEnd();
                        imageValues = JsonUtility.FromJson<ImageValues>(jsonString);
                    }
                });
            }
            catch
            {
                Debug.LogWarning("Помилка читання файлу з Firebase Storage: ");
                return null;
            }

            return imageValues;
        }
        public static void SaveEvent(EventData eventData, string eventID)
        {
            // Запис до Firestore
            var dataForFireStore = new Dictionary<string, object>()
            {
                {"ShortInfo", eventData.ShortInfo},
                {"FullInfo", eventData.FullInfo},
                {"TelegramLink", eventData.TelegramLink},
                {"RegisterFormLink", eventData.RegisterFormLink},
                {"LinkToAvatarInStorage", eventData.LinkToAvatarInStorage},
            };

            var document = _refToFirestore.Collection(FirestorePathToEvent)
                .Document(eventID);
            document.SetAsync(dataForFireStore);

            // Запис до Realtime
            var dataForRealtime = new Dictionary<string, object>()
            {
                {"Name", eventData.Name},
                {"DateAndTimeOfCreationEvent", eventData.DateAndTimeOfCreationEvent},
                {"EventEndTime", eventData.EventEndTime},
                {"Tags", eventData.Tags},
            };

            for (int i = 0; i < eventData.Tags.Length; i++)
            {
                _refToRealtime.Child(_realtimePathToTagsList)
                .Child(eventData.Tags[i].ToString()).Child(document.Id).SetValueAsync("");
            }

            _refToRealtime.Child(_realtimePathToEvent).Child(document.Id).SetValueAsync(dataForRealtime);
        }
        public static async Task UploadImageAsync(Texture2D texture2D, string pathToSave)
        {
            StorageReference imageRef = _refToStorage.Child(pathToSave + ActionWithTexture2D.TextureExpansion);
            StorageReference jsonRef = _refToStorage.Child(pathToSave + ".json");

            // Save Image
            var imgTask = imageRef.PutBytesAsync(ActionWithTexture2D.CompressTextureToBytes(texture2D));
            await imgTask;
            if (imgTask.IsFaulted || imgTask.IsCanceled)
                Debug.LogError(imgTask.Exception.ToString());
            else
                Debug.Log("Img upload successful!");

            // Save JSON
            var image = new ImageValues(texture2D);
            string jsonString = JsonConvert.SerializeObject(image, Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            var JsonTask = jsonRef.PutStreamAsync(stream);
            await JsonTask;

            if (JsonTask.IsFaulted || JsonTask.IsCanceled)
                Debug.LogError(JsonTask.Exception.ToString());
            else
                Debug.Log("JSON upload successful!");
        }
    }
}