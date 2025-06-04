using General.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace General.EventAction
{
    public class DeleteEvent
    {
        public event Action RealtimeDeleteComplete;
        public event Action CloudDeleteComplete;
        public event Action StorageDeleteComplete;
        public event Action DeleteWithTegsListComplete;

        public async Task FullDeleteAsync(List<string> documentsId)
        {
            var realtimeTasks = new List<Task>();
            var cloudTasks = new List<Task>();
            var storageTasks = new List<Task>();
            var tagsListTasks = new List<Task>();

            foreach (var documentId in documentsId)
                realtimeTasks.Add(RealtimeDeleteAsync(documentId));
            foreach (var documentId in documentsId)
                cloudTasks.Add(CloudDeleteAsync(documentId));
            foreach (var documentId in documentsId)
                storageTasks.Add(StorageDeleteAsync(documentId));
            foreach (var documentId in documentsId)
                tagsListTasks.Add(DeleteWithTagsListAsync(documentId));

            var realtimeTasksComplete = Task.WhenAll(realtimeTasks);
            var cloudTasksComplete = Task.WhenAll(cloudTasks);
            var storageTasksComplete = Task.WhenAll(storageTasks);
            var tagsListTasksComplete = Task.WhenAll(tagsListTasks);

            var allTasksComplete = new List<Task>
            {
                realtimeTasksComplete.ContinueWith((completedTask) => 
                {
                    RealtimeDeleteComplete?.Invoke();
                }),
                cloudTasksComplete.ContinueWith((completedTask) =>
                {
                    CloudDeleteComplete?.Invoke();
                }),
                storageTasksComplete.ContinueWith((completedTask) =>
                {
                    StorageDeleteComplete?.Invoke();
                }),
                tagsListTasksComplete.ContinueWith((completedTask) =>
                {
                    DeleteWithTegsListComplete?.Invoke();
                }),
            };
            await Task.WhenAll(allTasksComplete);
        }

        public async Task CloudDeleteAsync(string documentId)
            => await DatabaseActions.RefToFirestore.Collection(DatabaseActions.FirestorePathToEvent)
                .Document(documentId).DeleteAsync();
        public async Task RealtimeDeleteAsync(string documentId)
        => await DatabaseActions.RefToRealtime.Child(DatabaseActions.RealtimePathToEvent)
            .Child(documentId).RemoveValueAsync();
        public async Task StorageDeleteAsync(string documentId)
        {
            var folderRef = DatabaseActions.RefToStorage
                    .Child(DatabaseActions.PathToEventImageFolder)
                    .Child(documentId);

            var tasks = new List<Task>()
            {
                folderRef.Child("icon.json").DeleteAsync(),
                folderRef.Child("icon.tga").DeleteAsync(),
            };

            await Task.WhenAll(tasks);
        }
        public async Task DeleteWithTagsListAsync(string documentId)
        {
            var snapshot = await DatabaseActions.RefToRealtime
                .Child("TagsList")
                .OrderByChild(documentId)
                .GetValueAsync();

            var taskList = new List<Task>();
            foreach (var tag in snapshot.Children)
            {
                taskList.Add(DatabaseActions.RefToRealtime
                .Child("TagsList")
                .Child(tag.Key)
                .Child(documentId)
                .RemoveValueAsync());
            }

            await Task.WhenAll(taskList);
        }
    }
}