using System.Collections;
using System.Collections.Generic;
using UI.Popups.Menu.TabBar;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Menu.Gallery
{
    public class ImagesGallery : MonoBehaviour
    {
        private const float TabletAspectRatio = 0.6f;
        private const int PhoneColumns = 2;
        private const int TabletColumns = 3;

        [Header("Refs")] 
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private GalleryImageCell itemPrefab;
        [SerializeField] private ScreenResolutionDetector resolutionDetector;
        [SerializeField] private GalleryTabBar galleryTabBar;

        [Header("Settings")] 
        [SerializeField] private int preloadRows = 2;

        private readonly List<GalleryImageCell> _items = new();
        private List<string> _links;

        private void Awake()
        {
            scrollRect.onValueChanged.AddListener(OnScroll);
            resolutionDetector.OnScreenResolutionChange += UpdateGridColumns;
        }

        private IEnumerator Start()
        {
            var provider = new GalleryLinksProvider();

            yield return provider.GetLinks(list => { _links = list; });

            if (_links == null)
            {
                yield return null;
            }

            BuildGrid();

            yield return new WaitForEndOfFrame();

            galleryTabBar.SelectDefaultFilter();
            
            CheckVisible();
        }

        private void OnDestroy()
        {
            scrollRect.onValueChanged.RemoveListener(OnScroll);
            resolutionDetector.OnScreenResolutionChange -= UpdateGridColumns;
        }

        private void BuildGrid()
        {
            for (var i = 0; i < _links.Count; i++)
            {
                var item = Instantiate(itemPrefab, content);
                var cellIndex = i + 1;
                item.Setup(_links[i], cellIndex);

                _items.Add(item);
            }
        }

        private void UpdateGridColumns()
        {
            var ratio = (float)Screen.width / Screen.height;
            var isTablet = TabletAspectRatio < ratio;
            var columns = isTablet ? TabletColumns : PhoneColumns;

            grid.constraintCount = columns;

            UpdateCellSize(columns);
        }

        private void UpdateCellSize(int columns)
        {
            var width = content.rect.width;
            var padding = grid.padding.left + grid.padding.right;
            var totalSpacing = grid.spacing.x * (columns - 1);
            var cellWidth = (width - padding - totalSpacing) / columns;

            grid.cellSize = new Vector2(cellWidth, cellWidth);
        }
        
        public void ApplyFilter(GalleryFilterType filter)
        {
            foreach (var item in _items)
            {
                var index = item.Index;

                var visible = filter switch
                {
                    GalleryFilterType.All => true,
                    GalleryFilterType.Odd => index % 2 == 1,
                    GalleryFilterType.Even => index % 2 == 0,
                    _ => true
                };

                item.SetVisible(visible);
            }
        }

        private void OnScroll(Vector2 _)
        {
            CheckVisible();
        }

        private void CheckVisible()
        {
            foreach (var item in _items)
            {
                if (IsVisible(item.Rect))
                {
                    item.Load();
                }
            }
        }

        private bool IsVisible(RectTransform item)
        {
            var viewport = scrollRect.viewport;

            var itemBounds =
                RectTransformUtility.CalculateRelativeRectTransformBounds(
                    viewport,
                    item);

            var viewBounds =
                new Bounds(
                    viewport.rect.center,
                    viewport.rect.size);

            return itemBounds.Intersects(viewBounds);
        }
    }
}