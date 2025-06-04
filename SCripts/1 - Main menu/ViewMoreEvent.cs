using Firebase.Firestore;
using General;
using General.Database;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ViewMoreEvent : MonoBehaviour
    {
        [SerializeField] private Button _viewMoreButton;
        [SerializeField] private Image _viewMoreImage;
        [SerializeField] private RectTransform _content;
        [SerializeField] private OnLoadApp _onLoadApp;

        private string _lastDownloadEventsId;

        private void Awake()
        {
            OnLoadApp.CollectionIsntOver += CollectionIsntOver;
            OnLoadApp.CollectionIsOver += CollectionIsOver;

            SetActive(false);
        }
        public void CollectionIsntOver(string eventId)
        {
            _lastDownloadEventsId = eventId;
            SetViewMoreButton();
        }
        public void CollectionIsOver()
        {
            gameObject.transform.SetAsLastSibling();
            SetActive(false);
        }
        private void SetViewMoreButton()
        {
            gameObject.transform.SetAsLastSibling();
            SetActive(true);
        }
        
        public void ViewMore()
        {
            SetActive(false);
            _onLoadApp.LoadEventsAsync(_lastDownloadEventsId);
        }
        public void SetActive(bool value)
        {
            _viewMoreButton.enabled = value;
            _viewMoreImage.enabled = value;
        }
    }
}