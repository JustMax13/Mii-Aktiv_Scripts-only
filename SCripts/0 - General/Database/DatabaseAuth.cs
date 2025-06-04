using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace General.Database
{
    public class DatabaseAuth : MonoBehaviour
    {
        private FirebaseAuth _auth;

        public static FirebaseAuth Auth { get; private set; }

        public static Action SignInSuccessful;
        public static Action SignOutSuccessful;

        private void Awake()
        {
            _auth = FirebaseAuth.DefaultInstance;

            Auth = _auth;

            if (Auth.CurrentUser != null)
                _ = CheckCurrentUserOnDisable();
        }

        public static async Task<bool> CheckCurrentUserOnDisable()
        {
            try 
            { 
                await Auth.CurrentUser.ReloadAsync(); 
                return true; 
            }
            catch (Exception) 
            {
                Auth.SignOut();
                return false;
            }
        }

        private void HandleSignInError(FirebaseException ex)
        {
            switch (ex.ErrorCode)
            {
                case (int)AuthError.MissingEmail:
                    AuthFields.LoginStatus.text = "Не вказано електронну пошту";
                    break;

                case (int)AuthError.MissingPassword:
                    AuthFields.LoginStatus.text = "Не вказано пароль";
                    break;

                case (int)AuthError.InvalidEmail:
                    AuthFields.LoginStatus.text = "Некоректна адреса електронної пошти";
                    break;

                case (int)AuthError.WrongPassword:
                    AuthFields.LoginStatus.text = "Неправильний пароль";
                    break;

                case (int)AuthError.UserNotFound:
                    AuthFields.LoginStatus.text = "Ця електронна пошта не зареєстрована";
                    break;

                case (int)AuthError.NetworkRequestFailed:
                    AuthFields.LoginStatus.text = "Проблема підключення до сервера. Перевірте інтернет зв'язок та спробуйте ще раз";
                    break;

                case (int)AuthError.TooManyRequests:
                    AuthFields.LoginStatus.text = "Багато спроб входу за короткий час. Спробуйте пізніше";
                    break;

                case (int)AuthError.OperationNotAllowed:
                    AuthFields.LoginStatus.text = "Данна операція поки що недоступна. Спробуйте пізніше";
                    break;

                case (int)AuthError.UserDisabled:
                    AuthFields.LoginStatus.text = "Данний обліковий запис заблоковано адміністратором";
                    break;

                default:
                    AuthFields.LoginStatus.text = "Непередбачена помилка: " + ex.Message;
                    break;
            }
        }
        public async void SignIn()
        {
            try
            {
                AuthResult authResult = await _auth.SignInWithEmailAndPasswordAsync(AuthFields.Email.text, AuthFields.Password.text);
                AuthFields.LoginStatus.color = Color.blue;
                AuthFields.LoginStatus.text = "Успішно ввійшли в акаунт: " + authResult.User.Email;

                SignInSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                AuthFields.LoginStatus.color = Color.red;
                if (ex.InnerException is FirebaseException firebaseException)
                    HandleSignInError(firebaseException);
                else
                    AuthFields.LoginStatus.text = "Помилка: " + ex.Message;
            }

        }
        public void SignOut()
        {
            _auth.SignOut();
            AuthFields.LoginStatus.color = Color.black;
            AuthFields.LoginStatus.text = "Ви вийшли з акаунту";

            SignOutSuccessful?.Invoke();
        }
    }
}