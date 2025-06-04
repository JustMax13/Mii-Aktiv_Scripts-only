using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class RandomString : MonoBehaviour
    {
        private static string _characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static System.Random _random;

        static RandomString()
        {
            _random = new System.Random();
        }
        public static string Generate(int stringLength)
        {
            string result = "";

            for (int i = 0; i < stringLength; i++)
            {
                int index = _random.Next(_characters.Length);
                result += _characters[index];
            }

            return result;
        }
    }
}