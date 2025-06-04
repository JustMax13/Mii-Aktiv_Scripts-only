using General.Database;
using General.Value;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace General
{
    public class DependenceOfInterfacesOnAccountLogin : MonoBehaviour
    {
        [Header("Об'єкт для змін")]
        [SerializeField] private ChangeStateInterfacesValue _changeStateInterfacesValue;

        [Header("Коли користувач НЕ в акаунті")]
        [SerializeField] private RectTransform[] _turnOffInterface;
        [SerializeField] private RectTransform[] _turnOnInterface;

        [Header("Коли користувач в акаунті")]
        [SerializeField] private RectTransform[] _turnOffInterfaceInAccount;
        [SerializeField] private RectTransform[] _turnOnInterfaceInAccount;

        private void Awake()
        {
            WaitDatabaseAuthRef();
        }

        private async void WaitDatabaseAuthRef()
        {
            await Task.Run(() =>
            {
                while (DatabaseAuth.Auth == null) { }
            });

            DatabaseAuth.Auth.StateChanged += CheckAuthState;
        }
        private void CheckAuthState(object sender, EventArgs e)
        {
            if(DatabaseAuth.Auth.CurrentUser != null)
            {
                _changeStateInterfacesValue.TurnOffInterface = _turnOffInterfaceInAccount;
                _changeStateInterfacesValue.TurnOnInterface = _turnOnInterfaceInAccount;
            }
            else
            {
                _changeStateInterfacesValue.TurnOffInterface = _turnOffInterface;
                _changeStateInterfacesValue.TurnOnInterface = _turnOnInterface;
            }
        }
    }
}