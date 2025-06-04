using CreateMenu.Controllers;
using Filters;
using Filters.FilteringValues.Category;
using General.Calendar;
using General.ChooseTime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreateMenu
{
    public class CreateInterfaceValues : MonoBehaviour
    {
        [Header("Поля для заповнення")]
        [SerializeField] private Image _avatar;
        [SerializeField] private ControlledTMP_InputField _name;
        [SerializeField] private ControlledTMP_InputField _shortAbout;
        [SerializeField] private ControlledTMP_InputField _longAbout;
        [SerializeField] private ControlledTMP_InputField _telegramLink;
        [SerializeField] private ControlledTMP_InputField _registerFormLink;
        [SerializeField] private ApplyCategories _applyCategories;
        [SerializeField] private TextDateValue _textDateValue;
        [SerializeField] private TextTimeValue _textTimeValue;

        [Header("Дефолтні значення")]
        [SerializeField] private Sprite _defaultAvatar;

        public static Image Avatar { get; private set; }
        public static TMP_InputField Name { get; private set; }
        public static TMP_InputField ShortInfo { get; private set; }
        public static TMP_InputField FullInfo { get; private set; }
        public static TMP_InputField TelegramLink { get; private set; }
        public static TMP_InputField RegisterFormLink { get; private set; }
        public static ApplyCategories ApplyCategories { get; private set; }
        public static TextDateValue TextDateValue { get; private set; }
        public static TextTimeValue TextTimeValue { get; private set; }


        public static Sprite DefaultAvatar { get; private set; }

        public static int[] ChooseTagsID { get; private set; }

        private void Awake()
        {
            Avatar = _avatar;
            Name = _name;
            ShortInfo = _shortAbout;
            FullInfo = _longAbout;
            TelegramLink = _telegramLink;
            RegisterFormLink = _registerFormLink;
            ApplyCategories = _applyCategories;
            TextDateValue = _textDateValue;
            TextTimeValue = _textTimeValue;
            DefaultAvatar = _defaultAvatar;

            ApplyCategories.GetApplyCategories += SetApplyTags;
        }

        private void SetApplyTags(List<CategoryObject> applyTags)
        {
            ChooseTagsID = new int[applyTags.Count];

            for (int i = 0; i < applyTags.Count; i++)
                ChooseTagsID[i] = applyTags[i].ID;
        }
    }
}