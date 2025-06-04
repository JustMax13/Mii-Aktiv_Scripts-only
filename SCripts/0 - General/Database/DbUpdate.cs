using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace General.Database
{
    public class DbUpdate : MonoBehaviour
    {
        [SerializeField] private RectTransform _DbOnUpdateCanvas;

        public async Task<bool> TrustDbForUpdates()
        {
            bool result = true;

            var dataTask = DatabaseActions.RefToRealtime.Child(DatabaseActions.PathToDbStateOnUpdates).GetValueAsync();
            await dataTask;

            if (dataTask.IsFaulted)
                Debug.LogError("Failed: " + dataTask.Exception);
            else if (dataTask.IsCompleted)
            {
                DataSnapshot snapshot = dataTask.Result;

                result = (bool)snapshot.Value;
            }

            return result;
        }
        public void TurnOnCanvas() => _DbOnUpdateCanvas.gameObject.SetActive(true);
    }
}