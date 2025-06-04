using General.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class UpdateAppLink : MonoBehaviour, IOpenLink
    {
        private string _updateAppLink;

        private void Start()
        {
            SetLink();
        }
        private async void SetLink()
        {
            var data = await DatabaseActions.RefToRealtime.Child(DatabaseActions.PathToUpdateAppLinkRT).Child("0").GetValueAsync();
            _updateAppLink = data.Value.ToString();
        }
        public void OpenLink() => LinkActions.OpenLink(_updateAppLink);
    }

}