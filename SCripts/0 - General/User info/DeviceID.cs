using Google.MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace General
{
    public class DeviceID : MonoBehaviour
    {
        public static string ID { get; set; }
        private static string _path;

        public static event Action CheckIDEnd;

        private void Awake()
        {
            SetPath();
            if (File.Exists(_path))
            {
                ID = GetIDFromFile();

                if (ID == "" || ID == null)
                {
                    ID = CreateNewID();
                    SetToJson(ID);
                }
            }
            else
            {
                var deviceIDStruct = new DeviceIDStruct();
                deviceIDStruct.ID = CreateNewID();
                ID = deviceIDStruct.ID;

                string jsonID = JsonUtility.ToJson(deviceIDStruct);
                File.WriteAllText(_path, jsonID);
            }

            CheckIDEnd?.Invoke();
        }

        private static void SetPath()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _path = Path.Combine(Application.persistentDataPath, "DeviceID.json");
#else
            _path = Path.Combine(Application.dataPath, "Resources/Saves/DeviceID.json");
#endif
        }
        private static string GetIDFromFile()
        {
            string json = File.ReadAllText(_path);
            return JsonUtility.FromJson<DeviceIDStruct>(json).ID;
        }
        private static string CreateNewID() => Guid.NewGuid().ToString();
        private static void SetToJson(string id)
        {
            var deviceIDStruct = new DeviceIDStruct();
            deviceIDStruct.ID = id;

            File.WriteAllText(_path, JsonUtility.ToJson(deviceIDStruct));
        }

        public struct DeviceIDStruct
        {
            public string ID;
        }
    }
}