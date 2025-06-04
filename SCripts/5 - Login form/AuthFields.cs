using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General.Database
{
    public class AuthFields : MonoBehaviour
    {
        [Header("Вхід в акаунт")]
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TextMeshProUGUI _loginStatus;

        public static TMP_InputField Email { get; set; }
        public static TMP_InputField Password { get; set; }
        public static TextMeshProUGUI LoginStatus { get; set; }

        private void Awake()
        {
            Email = _email;
            Password = _password;
            LoginStatus = _loginStatus;
        }
    }
}