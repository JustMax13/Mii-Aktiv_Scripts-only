using General.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class StateDependentText : MonoBehaviour
    {
        [Header("Неактивний колір")]
        [SerializeField] private Color _nonActiveColor;

        [Header("Активний колір")]
        [SerializeField] private Color _activeColor;

        [Header("Перевіряється при зміні станів інтерфейсів")]
        [SerializeField] private GameObject[] _gameObjects;

        [Header("TMP для оновлення тексту")]
        [SerializeField] private TextMeshProUGUI _tmp;

        private void Awake()
        {
            InterfaceManagment.SomeInterfaceStateChanged += CheckObjectState;
        }

        private void CheckObjectState()
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.activeInHierarchy == true && _tmp.color != _activeColor)
                {
                    _tmp.color = _activeColor;
                    break;
                }
                else if (gameObject.activeInHierarchy == false && _tmp.color != _nonActiveColor)
                {
                    _tmp.color = _nonActiveColor;
                }
            } 
        }
    }
}