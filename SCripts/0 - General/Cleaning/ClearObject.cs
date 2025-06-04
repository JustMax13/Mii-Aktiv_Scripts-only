using UnityEngine;

namespace General
{
    public class ClearObject : MonoBehaviour
    {
        public static void ClearChildOn(RectTransform content)
        {
            while (content.childCount > 0)
            {
                Transform child = content.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }
        public static void ClearChildWithout(NeverClear neverClearObjectChild)
        {
            var gameObj = neverClearObjectChild.gameObject;

            int pass = 0;
            while (gameObj.transform.childCount > neverClearObjectChild.NeverClearChilds.Count)
            {
                Transform child = gameObj.transform.GetChild(pass);

                bool letsContinue = false;
                foreach (var element in neverClearObjectChild.NeverClearChilds)
                {
                    if (child.gameObject == element.gameObject)
                    {
                        pass++;
                        letsContinue = true;
                        break;
                    }
                }
                if (letsContinue)
                    continue;

                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }
    }
}