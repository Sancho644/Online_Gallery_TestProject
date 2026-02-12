using System.Collections.Generic;
using System.Linq;
using UI.Popups.Menu.Gallery;
using UnityEngine;

namespace UI.Popups.Menu.TabBar
{
    public class GalleryTabBar : MonoBehaviour
    {
        [SerializeField] private GalleryFilterType defaultFilter;
        [SerializeField] private List<TabToggle> toggles;
        [SerializeField] private ImagesGallery gallery;

        private void Awake()
        {
            foreach (var toggle in toggles)
            {
                toggle.Toggle.onValueChanged.AddListener((_) =>
                    OnTabSelected(toggle.FilterType));
            }
        }

        private void OnDestroy()
        {
            foreach (var toggle in toggles)
            {
                toggle.Toggle.onValueChanged.RemoveAllListeners();
            }
        }

        public void SelectDefaultFilter()
        {
            var toggleWithFilter = toggles.First(x => x.FilterType == defaultFilter);
            toggleWithFilter.Toggle.isOn = true;
        }

        private void OnTabSelected(GalleryFilterType filter)
        {
            gallery.ApplyFilter(filter);
        }
    }
}