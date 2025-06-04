using System.Collections.Generic;
using UnityEngine;

namespace General.Value
{
    public class AllCanvasValue : MonoBehaviour
    {
        [Header("Усі наявні інтерфейси")]
        [SerializeField] private List<RectTransform> _allInterfaces;

        [Header("Основні інтерфейси")]
        [SerializeField] private RectTransform _mainInterface;
        [SerializeField] private RectTransform _viewInterface;
        [SerializeField] private RectTransform _filterInterface;
        [SerializeField] private RectTransform _createInterface;
        [SerializeField] private RectTransform _profileInterface;
        [SerializeField] private RectTransform _floorInterface;

        public static RectTransform MainInterface { get; private set; }
        public static RectTransform ViewInterface { get; private set; }
        public static RectTransform FilterInterface { get; private set; }
        public static RectTransform CreateInterface { get; private set; }
        public static RectTransform ProfileInterface { get; private set; }
        public static RectTransform FloorInterface { get; private set; }

        public static List<RectTransform> AllInterfaces { get; private set; }

        private void Awake()
        {
            MainInterface = _mainInterface;
            ViewInterface = _viewInterface;
            FilterInterface = _filterInterface;
            CreateInterface = _createInterface;
            ProfileInterface = _profileInterface;
            FloorInterface = _floorInterface;

            if (MainInterface == null || ViewInterface == null || FilterInterface == null 
                || CreateInterface == null || ProfileInterface == null || FloorInterface == null )
                throw new System.Exception($"Забули на {gameObject} поставити один, або декілька з головних інтерфейсів.");

            AllInterfaces = _allInterfaces;
        }
    }
}