using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class ClearCreateInterface : MonoBehaviour
    {
        public static void Clear()
        {
            CreateInterfaceValues.Avatar.sprite = CreateInterfaceValues.DefaultAvatar;

            CreateInterfaceValues.Name.text = null;
            CreateInterfaceValues.ShortInfo.text = null;
            CreateInterfaceValues.FullInfo.text = null;
            CreateInterfaceValues.TelegramLink.text = null;
            CreateInterfaceValues.RegisterFormLink.text = null;

            CreateInterfaceValues.TextDateValue.Tmp.text = CreateInterfaceValues.TextDateValue.DefaultTextValue;
            CreateInterfaceValues.TextDateValue.Calendar.ClearSelectDay();

            CreateInterfaceValues.TextTimeValue.Tmp.text = CreateInterfaceValues.TextTimeValue.DefaultTextValue;

            // Категорії та інтерфейс вибору часу очищуються при натисканні на кнопку Create, якщо усі поля заповнені
        }
    }
}