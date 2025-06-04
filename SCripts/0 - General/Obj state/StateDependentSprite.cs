using General.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class StateDependentSprite : MonoBehaviour
    {
        [Header("Стандартний Sprite")]
        [SerializeField] private Sprite _standartSprite;

        [Header("Sprite коли об'єкт активний")]
        [SerializeField] private Sprite _changedSprite;

        [Header("Перевіряється при зміні станів інтерфейсів")]
        [SerializeField] private GameObject[] _gameObjects;

        [Header("Об'єкт для оновлення Sprite")]
        [SerializeField] private Image _image;

        private void Awake()
        {
            InterfaceManagment.SomeInterfaceStateChanged += CheckObjectState;
        }

        private void CheckObjectState()
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.activeInHierarchy == true && _image.sprite != _changedSprite)
                {
                    _image.sprite = _changedSprite;
                    break;
                }   
                else if (gameObject.activeInHierarchy == false && _image.sprite != _standartSprite)
                {
                    _image.sprite = _standartSprite;
                }
            }
        }
    }
}