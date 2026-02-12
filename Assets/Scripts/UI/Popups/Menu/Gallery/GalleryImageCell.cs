using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.Popups.Menu.Gallery
{
    public class GalleryImageCell : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform rectTransform;

        public RectTransform Rect => rectTransform;
        public int Index => _index;

        private string _url;
        private bool _loaded;
        private Coroutine _loadRoutine;
        private int _index;

        public void Setup(string url, int index)
        {
            _url = url;
            _index = index;
        }

        public void Load()
        {
            if (_loaded || _loadRoutine != null)
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

            image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

            _loaded = true;
        }
    }
}