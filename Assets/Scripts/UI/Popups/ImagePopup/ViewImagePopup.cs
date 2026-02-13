using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.ImagePopup
{
    public class ViewImagePopup : AbstractPopup
    {
        [SerializeField] private Image image;

        public void Setup(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}