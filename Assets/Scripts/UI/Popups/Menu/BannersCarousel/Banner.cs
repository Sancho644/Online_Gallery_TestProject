using UnityEngine;

namespace UI.Popups.Menu.BannersCarousel
{
    [ExecuteAlways]
    public class Banner : MonoBehaviour
    {
        [SerializeField] private RectTransform bannerRect;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private ScreenResolutionDetector resolutionDetector;

        private void Awake()
        {
            if (resolutionDetector != null)
            {
                resolutionDetector.OnScreenResolutionChange += SetBannerSize;
            }
        }

        private void Start()
        {
            SetBannerSize();
        }

        private void OnDestroy()
        {
            if (resolutionDetector != null)
            {
                resolutionDetector.OnScreenResolutionChange -= SetBannerSize;
            }
        }

        private void SetBannerSize()
        {
            if (viewport == null)
            {
                return;
            }

            var width = viewport.rect.width;
            var height = viewport.rect.height;

            bannerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            bannerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void OnRectTransformDimensionsChange()
        {
            SetBannerSize();
        }
    }
}