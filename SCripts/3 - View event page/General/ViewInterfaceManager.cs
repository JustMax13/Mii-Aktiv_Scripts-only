using General;
using General.Data;
using General.Move;
using System;
using UnityEngine;

namespace ViewMenu
{
    public class ViewInterfaceManager : MonoBehaviour
    {
        [SerializeField] private InitialTransform _contentInitTransform;
        [SerializeField] private PreferSizeTextField[] _autoSizeTextFields;
        /// <summary>
        /// Передавати значення ТІЛЬКИ починаючи з верхнього елементу у content, закінчуючи ніжнім 
        /// </summary>
        [SerializeField] private MoveAfterPoint[] _moveAfterPoints;
        [SerializeField] private ChangeContentBottom _changeContentBottom;

        private void Awake()
        {
            ViewInterfaceActions.SetValueIsEndWithResult += SetValueIsEnd;
        }
        private void SetValueIsEnd(EventData eventData)
        {
            foreach (var item in _autoSizeTextFields)
                item.UpdateSize();
            foreach (var item in _moveAfterPoints)
                item.Move();

            _changeContentBottom.Change();
            _contentInitTransform.SetInitLocalPosition();
        }
    }
}