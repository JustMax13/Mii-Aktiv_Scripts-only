using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class CanvasStarter : MonoBehaviour, IStarter
    {
        // Скрипт по суті для того, щоб усі елементи Canvas пройшли Awake
        [SerializeField] private Canvas[] canvases;
        [SerializeField] private int targetUpdateCount;

        private float[] planeDistances;

        public bool IsStart { get; set; } = false;

        public void Launch()
        {
            planeDistances = new float[canvases.Length];

            for (int i = 0; i < canvases.Length; i++)
            {
                planeDistances[i] = canvases[i].planeDistance;
                canvases[i].planeDistance = -1;
                canvases[i].gameObject.SetActive(true);
            }

            StartCoroutine(ExecuteAfterUpdates());

            IsStart = true;
        }
        private IEnumerator ExecuteAfterUpdates()
        {
            int updatesCount = 0;

            while (updatesCount < targetUpdateCount)
            {
                yield return null;
                updatesCount++;
            }

            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].planeDistance = planeDistances[i];
                canvases[i].gameObject.SetActive(false);
            }
        }
    }
}