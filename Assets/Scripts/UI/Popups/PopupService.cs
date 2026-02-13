using UI.Popups.ImagePopup;
using UnityEngine;

namespace UI.Popups
{
    public class PopupService : MonoBehaviour, IPopupService
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private ViewImagePopup imagePopup;

        public void ShowImagePopup(Sprite sprite)
        {
            var popup =
                Instantiate(imagePopup, root);

            popup.Setup(sprite);
        }
    }
}