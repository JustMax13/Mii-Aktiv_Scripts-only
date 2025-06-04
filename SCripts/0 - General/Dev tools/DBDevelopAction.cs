using Firebase.Firestore;
using General.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FastDevelop
{
    public class DBDevelopAction : MonoBehaviour
    {
        private void Start()
        {
            DeleteCollectionWithoutFirstByDate(1);
            // тут викликаються усі методи
        }
        // Видалити усю колекцію подій

        // Видалити усю колекцию, окрім N первых елементов
        private /*async*/ void DeleteCollectionWithoutFirstByDate(int value)
        {
            //// КАРТИНКИ НЕ УДАЛЯЮТСЯ
            //QuerySnapshot querySnapshots = await DatabaseActions.GetEventsName();

            //int counter = querySnapshots.Count - 1;

            //foreach (var document in querySnapshots)
            //{
            //    if (counter < value)
            //    {
            //        counter--;
            //        continue;
            //    }
            //    else
            //        counter--;

            //    await document.Reference.DeleteAsync();
            //}

            //Debug.Log("Delete complete!");
            Debug.LogWarning("Даний метод поки що не працює");
        }
        // Створити N пустих подій

    }

}