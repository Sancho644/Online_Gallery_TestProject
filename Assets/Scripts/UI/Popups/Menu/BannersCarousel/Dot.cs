using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Menu.BannersCarousel
{
    public class Dot : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;

        public void SetActive(bool active)
        {
            image.sprite = active
                ? image.sprite = activeSprite
                : image.sprite = inactiveSprite;
        }
    }
}