using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewMenu
{
    public class ClearViewInterface : MonoBehaviour
    {
        public void Clear()
        {
            ViewInterfaceField.Avatar.texture = ViewInterfaceField.DefaultAvatar.texture;

            ViewInterfaceField.Name.text = null;
            ViewInterfaceField.StartDate.text = null;
            ViewInterfaceField.ShortInfo.text = null;
            ViewInterfaceField.FullInfo.text = null;
            ViewInterfaceField.TelegramLink.Link = null;
            ViewInterfaceField.RegisterFormLink.Link = null;
        }
    }
}
