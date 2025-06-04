using General;
using General.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewMenu
{
    public class ViewInterfaceField : MonoBehaviour
    {
        [Header("Поля для виводу")]
        [SerializeField] private RawImage _avatar;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _startDate;
        [SerializeField] private TextMeshProUGUI _shortAbout;
        [SerializeField] private TextMeshProUGUI _longAbout;
        [SerializeField] private LinkObject _telegramLink;
        [SerializeField] private Button _telegramButton;
        [SerializeField] private LinkObject _registerFormLink;

        [Header("Дефолтні значення")]
        [SerializeField] private Sprite _defaultAvatar;

        public static EventData EventData { get; set; }
        public static RawImage Avatar { get; set; }
        public static TextMeshProUGUI Name { get; set; }
        public static TextMeshProUGUI StartDate { get; set; }
        public static TextMeshProUGUI ShortInfo { get; set; }
        public static TextMeshProUGUI FullInfo { get; set; }
        public static LinkObject TelegramLink { get; set; }
        public static Button TelegramButton { get; set; }
        public static LinkObject RegisterFormLink { get; set; }

        public static Sprite DefaultAvatar { get; private set; }

        private void Awake()
        {
            Avatar = _avatar;
            Name = _name;
            StartDate = _startDate;
            ShortInfo = _shortAbout;
            FullInfo = _longAbout;
            TelegramLink = _telegramLink;
            TelegramButton = _telegramButton;
            RegisterFormLink = _registerFormLink;

            DefaultAvatar = _defaultAvatar;
        }
    }
}