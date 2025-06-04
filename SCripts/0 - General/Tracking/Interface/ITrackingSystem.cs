using Firebase.Storage;
using General.Database;
using Google.MiniJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace General.Tracking
{
    public interface ITrackingSystem
    {
        private static StorageReference PathInStorage { get; set; }
        public int ActionForSave { get; }
        public string SystemIDName { get; set; }
        public string JsonPath { get; set; }
        public ITrackingSystem ThisTrackingSystem { get; }
        public List<ITracker> Trackers { get; set; }
        public bool CheckReadinessForSaveToDB(string json)
        {
            JObject jsonData = JObject.Parse(json);
            int newValueSum = 0;
            try { newValueSum = (int)jsonData["NewValueSum"]; }
            catch { return false; }

            if (newValueSum >= ActionForSave)
                return true;
            else
                return false;
        }
        public void CreateEmptyJson() => File.WriteAllText(JsonPath, JsonUtility.ToJson(""));
        public async void Initialization()
        {
            if (PathInStorage == null)
            {
                if (DatabaseActions.RefToStorage == null)
                    await Task.Run(() => { while (DatabaseActions.RefToStorage == null) { } });
                PathInStorage = DatabaseActions.RefToStorage.Child("Tracking");
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            JsonPath = Path.Combine(Application.persistentDataPath, SystemIDName + ".json");
#else
            JsonPath = Path.Combine(Application.dataPath, "Resources/Saves/" + SystemIDName + ".json");
#endif
        }
        protected string TrackersToJson()
        {
            if (Trackers == null)
                return null;

            var jsonStrings = new JObject[Trackers.Count];

            int i = 0;
            foreach (var tracker in Trackers)
            {
                JObject jsonObject = new JObject();
                jsonObject["NameInJson"] = tracker.NameInJson;

                JArray countersArray = new JArray();
                foreach (var counter in tracker.Counters)
                {
                    JObject counterObject = new JObject();
                    counterObject["Key"] = counter.Key;
                    counterObject["Value"] = counter.Value;
                    countersArray.Add(counterObject);
                }
                jsonObject["Counters"] = countersArray;

                jsonStrings[i++] = jsonObject;
            }
            var mergedObject = new JObject();
            foreach (var jObject in jsonStrings)
                mergedObject.Merge(jObject);

            return JsonConvert.SerializeObject(mergedObject, Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        public async void SaveToDB(string systemDataInJson)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(systemDataInJson));

            string deviceID = DeviceID.ID;
            StorageReference jsonRef = PathInStorage.Child(deviceID).Child(SystemIDName + ".json");

            var JsonTask = jsonRef.PutStreamAsync(stream);
            await JsonTask;

            if (JsonTask.IsFaulted || JsonTask.IsCanceled)
                Debug.LogError(JsonTask.Exception.ToString());
            else
                Debug.Log("TrackerSystem in JSON upload successful!");
        }
        public string UpdateJson()
        {
            if (!File.Exists(JsonPath))
                CreateEmptyJson();

            if (Trackers == null || Trackers.Count == 0)
                return File.ReadAllText(JsonPath);

            string jsonTrackers = TrackersToJson();

            JObject systemInfo = new JObject();
            systemInfo["SystemName"] = SystemIDName;
            systemInfo["NewValueSum"] = 0; // змінна імені цього параметру потребує також заміни назви у методах CheckReadinessForSaveToDB та UpdateNewValueSum
            JObject trackersInfo = JObject.Parse(jsonTrackers);

            JObject mergedJson = new JObject();
            mergedJson.Merge(systemInfo);
            mergedJson.Merge(trackersInfo);

            string resultJson = "";
            string oldJson = File.ReadAllText(JsonPath);
            JObject oldJsonObj = JObject.Parse(oldJson);
            JObject resultObj = new JObject();

            int newValueSum;
            try { newValueSum = (int)oldJsonObj["NewValueSum"]; }
            catch { newValueSum = 0; }

            foreach (var property in mergedJson.Properties())
            {
                string propertyName = property.Name;

                if (property.Value.Type == JTokenType.Array)
                {
                    JArray resultArray = oldJsonObj[propertyName] as JArray;

                    if (resultArray == null)
                    {
                        resultArray = new JArray(property.Value.OrderBy(item => (int)item["Key"]));

                        foreach (var pair in resultArray)
                            newValueSum += (int)pair["Value"];
                    } 
                    else
                    {
                        foreach (JObject jObject in property.Value)
                        {
                            JObject foundItem = resultArray.Children<JObject>().FirstOrDefault(item => (int)item["Key"] == (int)jObject["Key"]);
                            
                            if (foundItem != null) 
                                foundItem["Value"] = (int)foundItem["Value"] + (int)jObject["Value"];
                            else
                                resultArray.Add(jObject);

                            newValueSum += (int)jObject["Value"];
                        }

                        resultArray = new JArray(resultArray.OrderBy(item => (int)item["Key"]));
                    }

                    resultObj[propertyName] = resultArray;
                }
                else
                    resultObj[propertyName] = property.Value;
            }

            resultObj["NewValueSum"] = newValueSum;
            resultJson = resultObj.ToString();

            ResetTrackers();
            WriteData(resultJson, JsonPath);

            return resultJson;
        }
        public void UpdateNewValueSum()
        {
            string json = File.ReadAllText(JsonPath);
            JObject data = JObject.Parse(json);

            while ((int)data["NewValueSum"] >= ActionForSave)
                data["NewValueSum"] = (int)data["NewValueSum"] - ActionForSave;

            WriteData(data.ToString(), JsonPath);
        }
        private void WriteData(string json, string path)
        {
            File.WriteAllText(path, string.Empty);
            File.WriteAllText(path, json);
        }

        private void ResetTrackers()
        {
            foreach (var tracker in Trackers)
                tracker.Reset();
        }
        public void CallOnAwake()
        {
            string json = null;

            if (File.Exists(JsonPath))
                json = File.ReadAllText(JsonPath);
            if (json != null && json != "{}" && ThisTrackingSystem.CheckReadinessForSaveToDB(json))
            {
                ThisTrackingSystem.UpdateNewValueSum();
                ThisTrackingSystem.SaveToDB(json);
            }
        }
        public void CallOnApplicationQuit()
        {
            ThisTrackingSystem.UpdateJson();
        }
    }
}