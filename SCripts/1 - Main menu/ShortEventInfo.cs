using Firebase.Firestore;
using General;
using General.Data;
using General.Database;
using General.Manager;
using General.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewMenu;

namespace MainMenu
{
    public class ShortEventInfo : MonoBehaviour
    {
        [SerializeField] private RawImage _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _shortAbout;

        private EventData _eventData;
        private static GameObject _prefabShortEventInfo;

        public EventData EventData { get => _eventData; }
        public RawImage Icon { get => _icon; }
        public TextMeshProUGUI Name { get => _name; }
        public TextMeshProUGUI ShortAbout { get => _shortAbout; }

        public static ShortEventInfo CreateEmptyObject(RectTransform content)
        {
            if (_prefabShortEventInfo == null)
            {
                _prefabShortEventInfo = Resources.Load<GameObject>("Prefab/1 - Main menu/ViewField/Event preview");
                if (_prefabShortEventInfo == null)
                    throw new Exception("Event preview був переміщений, або видалений.");
            }

            GameObject shortInfoObj = Instantiate(_prefabShortEventInfo, content);
            var shortInfo = shortInfoObj.GetComponent<ShortEventInfo>();

            var viewChangeStateValue = shortInfo.GetComponentInChildren<ChangeStateInterfacesValue>();
            var viewButton = viewChangeStateValue.GetComponent<Button>();

            viewChangeStateValue.TurnOnInterface = new RectTransform[] { AllCanvasValue.ViewInterface };

            viewButton.onClick.AddListener(() => InterfaceManagment.ChangeStateOfAllInterfaces(false));
            viewButton.onClick.AddListener(() => InterfaceManagment.ChangeStateOfInterfaces(viewChangeStateValue));
            viewButton.onClick.AddListener(() => ViewInterfaceActions.SetValueToViewInterface(shortInfo.EventData));

            return shortInfo;
        }
        public async Task SetFields(string eventId)
        {
            try
            {
                _eventData = await EventData.CreateByDatabaseId(eventId);
                _name.text = _eventData.Name;
                _shortAbout.text = _eventData.ShortInfo;
            }
            catch
            {
                Debug.LogWarning("Destroy object: " + gameObject + ". Something's  wrong.");
                Destroy(gameObject);
            }
        }
        public static Task SetTextures(List<ShortEventInfo> shortEventInfos, CancellationToken cancellationToken = default)
        {
            var byteArrays = new List<Task<byte[]>>();
            var imageValues = new List<Task<ImageValues>>();

            try
            {
                foreach (var shortEventInfo in shortEventInfos)
                {
                    if (!PathIsEmpty(shortEventInfo))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        byteArrays.Add(DatabaseActions.GetBitesArray(shortEventInfo.EventData.LinkToAvatarInStorage));
                    }
                    else
                        byteArrays.Add(null);
                }
                foreach (var shortEventInfo in shortEventInfos)
                {
                    if (!PathIsEmpty(shortEventInfo))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        imageValues.Add(DatabaseActions.GetImageValues(shortEventInfo.EventData.LinkToAvatarInStorage));
                    }
                    else
                        imageValues.Add(null);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Задача відмінена при завантажені текстури для shortEventInfo.");
                return Task.CompletedTask;
            }

            if (byteArrays == null)
            {
                Debug.LogWarning("Масив байтів пустий і не завантажено жодного фото");
                return Task.CompletedTask;
            }

            for (int i = 0; i < byteArrays.Count; i++)
                SetTexture(shortEventInfos.ElementAt(i), byteArrays[i], imageValues[i], cancellationToken);
            return Task.CompletedTask;
        }

        private static async void SetTexture(ShortEventInfo shortEventInfo, Task<byte[]> taskByteArray, Task<ImageValues> taskImageValue, CancellationToken cancellationToken = default)
        {
            if (taskByteArray == null || taskImageValue == null)
                return;

            var byteArray = await taskByteArray;
            var imageValue = await taskImageValue;

            try
            {
                shortEventInfo.EventData.Icon = await ActionWithTexture2D.CreateTexture(byteArray, imageValue, cancellationToken);
                shortEventInfo.Icon.texture = shortEventInfo.EventData.Icon;
            }
            catch
            {
                Debug.LogWarning("shortEventInfo был удален до установки фотографии.");
            }
        }
        private static bool PathIsEmpty(ShortEventInfo shortEventInfo)
        {
            if (shortEventInfo == null || shortEventInfo.EventData == null)
            {
                Debug.Log("shortEventInfo or EventData = null");
                return true;
            }
            string path = shortEventInfo.EventData.LinkToAvatarInStorage;
            if (path == "" || path == null)
                return true;

            return false;
        }
    }
}