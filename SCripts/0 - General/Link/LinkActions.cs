using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class LinkActions : MonoBehaviour
    {
        public static void OpenLink(string link) => Application.OpenURL(link);
        public static void OpenLink(LinkObject linkObject) => Application.OpenURL(linkObject.Link);
    }
}