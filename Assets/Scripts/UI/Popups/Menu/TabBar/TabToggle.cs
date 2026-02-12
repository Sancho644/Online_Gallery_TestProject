using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Menu.TabBar
{
    public class TabToggle : MonoBehaviour
    {
        [SerializeField] private GalleryFilterType filterType;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image checkMark;
        
        public Toggle Toggle => toggle;
        public GalleryFilterType FilterType => filterType;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(isOn => {checkMark.enabled = isOn;});
        }

        private void Start()
        {
            checkMark.enabled = toggle.isOn;
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(isOn => {checkMark.enabled = isOn;});
        }
    }
}