using UnityEngine;

namespace UI.Popups
{
    public abstract class AbstractPopup : MonoBehaviour
    {
        public void ClosePopup()
        {
            Destroy(gameObject);
        }
    }
}