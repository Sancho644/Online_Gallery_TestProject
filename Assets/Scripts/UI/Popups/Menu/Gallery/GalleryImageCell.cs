using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.Popups.Menu.Gallery
{
    public class GalleryImageCell : MonoBehaviour
    {
        [SerializeField] private int premiumInterval;
        [SerializeField] private Image image;
        [SerializeField] private Image premiumBadge;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button button;

        public RectTransform Rect => rectTransform;
        public int Index => _index;

        private string _url;
        private bool _loaded;
        private Coroutine _loadRoutine;
        private int _index;
        private Sprite _image;
        
        private IPopupService _popupService;

        private void Awake()
        {
            button.onClick.AddListener(ShowViewImagePopup);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(ShowViewImagePopup);
        }

        public void Setup(string url, int index, IPopupService popupService)
        {
            _url = url;
            _index = index;
            _popupService = popupService;

            SetupPremium();
        }

        public void Load()
        {
            if (_loaded || _loadRoutine != null || !gameObject.activeInHierarchy)
            {
                return;
            }

            _loadRoutine = StartCoroutine(LoadRoutine());
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        private IEnumerator LoadRoutine()
        {
            using var req = UnityWebRequestTexture.GetTexture(_url);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            var tex = DownloadHandlerTexture.GetContent(req);

            _image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            image.sprite = _image;

            _loaded = true;
        }

        private void ShowViewImagePopup()
        {
            if (_image == null)
            {
                return;
            }
            
            _popupService.ShowImagePopup(_image);
        }

        private void SetupPremium()
        {
            var inPremium = _index % premiumInterval == 0;
            premiumBadge.enabled = inPremium;
        }
    }
}