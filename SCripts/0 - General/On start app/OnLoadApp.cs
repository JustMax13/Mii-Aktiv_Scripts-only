using Filters;
using General.Database;
using MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace General
{
    public class OnLoadApp : MonoBehaviour
    {
        [SerializeField] private int _eventCount;
        [SerializeField] private RectTransform _content;
        [SerializeField] private VersionCompatibility _versionCompatibility;
        [SerializeField] private DbUpdate _dbUpdate;
        [SerializeField] private ApplyCategories _applyCategories;

        private int _loadCollectionLength;
        //private static bool _appVersionChecked = false;
        //private static bool _dbStateChecked = false;
        private static bool _isVersionSupported = false;
        private static bool _allowedToLoadEvents = true;

        private static event Action DatabaseInitializationComplete;

        public static event Action LoadEventsIsEnd;
        public static event Action<string> CollectionIsntOver;
        public static event Action CollectionIsOver;

        public int EventCount { get => _eventCount; private set => _eventCount = value; }
        public int LoadCollectionLength { get => _loadCollectionLength; private set => _loadCollectionLength = value; }
        public RectTransform Content { get => _content; private set => _content = value; }

        private void Awake()
        {
            _loadCollectionLength = _eventCount + 1;

            DatabaseInitializationComplete += DatabaseInitializationCompleteAsync;
            _applyCategories.CategoriesApply += LoadEvents;

            WaitDatabaseInitializationAsync();
        }

        private async void WaitDatabaseInitializationAsync()
        {
            await Task.Run(() =>
            {
                while (!DatabaseActions.DatabaseInitializationComplete) { }
            });

            DatabaseInitializationComplete?.Invoke();
        }
        private async void DatabaseInitializationCompleteAsync()
        {
            await CheckVersionCompatibilityAsync();
            await CheckDbOnUpdateAsync();

            if (_allowedToLoadEvents == true)
                LoadEventsAsync();
        }
        private async Task CheckVersionCompatibilityAsync()
        {
            _isVersionSupported = await _versionCompatibility.IsVersionSupportedAsync();
            if (!_isVersionSupported)
            {
                _versionCompatibility.TurnOnUnsupportedVersionCanvas();
                _allowedToLoadEvents = false;
            }

            //_appVersionChecked = true;
        }
        private async Task CheckDbOnUpdateAsync()
        {
            if (!_isVersionSupported)
            {
                //_dbStateChecked = true;
                _allowedToLoadEvents = false;
                return;
            }

            bool dbOnUpdate = await _dbUpdate.TrustDbForUpdates();

            if (dbOnUpdate)
            {
                _dbUpdate.TurnOnCanvas();
                _allowedToLoadEvents = false;
            }

            //_dbStateChecked = true;
        }
        private void LoadEvents()
            => LoadEventsAsync();
        public async void LoadEventsAsync(string startAfterEventWithId = "")
        {
            var collectionOfEventsId = new List<string>();
            if (_applyCategories.AppliedCategories == null)
            {
                if (startAfterEventWithId == "")
                    collectionOfEventsId = await DatabaseActions.GetEvents(LoadCollectionLength);
                else
                    collectionOfEventsId = await DatabaseActions.GetEvents(LoadCollectionLength, startAfterEventWithId);
            }
            else
            {
                if (startAfterEventWithId == "")
                    collectionOfEventsId = await DatabaseActions.GetEventsWithTags(_applyCategories.AppliedCategories, LoadCollectionLength);
                else
                    collectionOfEventsId = await DatabaseActions.GetEventsWithTags(_applyCategories.AppliedCategories, LoadCollectionLength, startAfterEventWithId);
            }

            collectionOfEventsId.Reverse();

            int counter = 0;
            var shortInfoObjects = new List<ShortEventInfo>();
            var tasks = new List<Task>();
            foreach (var someEventId in collectionOfEventsId)
            {
                if (counter == _eventCount || counter == collectionOfEventsId.Count)
                    break;

                var shortInfo = ShortEventInfo.CreateEmptyObject(Content);
                tasks.Add(shortInfo.SetFields(someEventId));
                shortInfoObjects.Add(shortInfo);

                counter++;
            }

            if (counter == _eventCount && collectionOfEventsId.Count > counter)
                CollectionIsntOver?.Invoke(collectionOfEventsId.ElementAt(collectionOfEventsId.Count - 2)); // беремо 2й з кінця
            else
                CollectionIsOver?.Invoke();

            await Task.Run(() =>
            {
                bool allTaskIsCompleted = false;
                while (!allTaskIsCompleted)
                {
                    foreach (var task in tasks)
                    {
                        if (task.IsCompleted)
                        {
                            allTaskIsCompleted = true;
                            continue;
                        }
                        else
                        {
                            allTaskIsCompleted = false;
                            break;
                        }
                    }
                }
            });
            await ShortEventInfo.SetTextures(shortInfoObjects);

            LoadEventsIsEnd?.Invoke();
        }
    }
}