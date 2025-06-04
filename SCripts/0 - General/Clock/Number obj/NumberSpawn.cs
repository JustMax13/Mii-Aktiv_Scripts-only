using System;
using System.Collections.Generic;
using UnityEngine;

namespace General.ChooseTime
{
    public class NumberSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _numberPrefab;
        [SerializeField] private GameObject _emptyPrefab;

        [SerializeField] private RectTransform _content;

        [SerializeField] private bool _startFromZero;
        [SerializeField] private int _amountOfNumbers;
        [SerializeField] private int _amountOfEmptyOnStart;
        [SerializeField] private int _amountOfEmptyOnEnd;

        public Action<List<TimePanel>> SpawnEnd;

        private void Start()
        {
            Spawn(true);
        }
        public void Spawn(bool clearContent)
        {
            if (clearContent)
                ClearObject.ClearChildOn(_content);

            var allElements = new List<TimePanel>();

            for (int i = 0; i < _amountOfEmptyOnStart; i++)
                allElements.Add(Instantiate(_emptyPrefab, _content).GetComponent<TimePanel>());

            {
                int i;
                if (_startFromZero)
                    i = -1;
                else
                    i = 0;

                for (; i < _amountOfNumbers; i++)
                {
                    GameObject gameObject = Instantiate(_numberPrefab, _content);
                    var timePanel = gameObject.GetComponent<TimePanel>();
                    if (i + 1 < 10)
                        timePanel.Tmp.text = "0" + (i + 1).ToString();
                    else
                        timePanel.Tmp.text = (i + 1).ToString();

                    timePanel.Value = int.Parse(timePanel.Tmp.text);
                    allElements.Add(timePanel);
                }
            }

            for (int i = 0; i < _amountOfEmptyOnEnd; i++)
                allElements.Add(Instantiate(_emptyPrefab, _content).GetComponent<TimePanel>());

            SpawnEnd?.Invoke(allElements);
        }
    }
}