using Firebase.Database;
using General.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace General
{
    public class VersionCompatibility : MonoBehaviour
    {
        [SerializeField] private RectTransform _unsupportedVersionCanvas;

        public async Task<bool> IsVersionSupportedAsync()
        {
            bool result = false;
            var dataTask = DatabaseActions.RefToRealtime.Child(DatabaseActions.PathToSupportedVersionsRT).GetValueAsync();
            await dataTask;

            Debug.Log("Версія додатку: " + Application.version);
            if (dataTask.IsFaulted)
                Debug.LogError("Failed to fetch versions: " + dataTask.Exception);
            else if (dataTask.IsCompleted)
            {
                DataSnapshot snapshot = dataTask.Result;

                foreach (DataSnapshot versionSnapshot in snapshot.Children)
                {
                    string supportedVersion = versionSnapshot.Value.ToString();
                    Debug.Log("Підтримуєма версія з БД: " + supportedVersion);
                    if(supportedVersion == Application.version)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
        public void TurnOnUnsupportedVersionCanvas() => _unsupportedVersionCanvas.gameObject.SetActive(true);
    }
}