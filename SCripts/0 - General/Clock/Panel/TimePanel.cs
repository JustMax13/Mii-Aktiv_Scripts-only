using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace General.ChooseTime
{
    public class TimePanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _tmp;

        public RectTransform RectTransform { get => _rectTransform; }
        public TextMeshProUGUI Tmp { get => _tmp; }
        public int Value { get; set; }
        public List<TimePanel> TimePanelParentList { get; set; }
    }
}