using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Buttons
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private AbstractPopup popup;

        private void Awake()
        {
            button.onClick.AddListener(ClosePopup);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(ClosePopup);
        }

        private void ClosePopup()
        {
            if (popup != null)
            {
                popup.ClosePopup();
            }
        }
    }
}