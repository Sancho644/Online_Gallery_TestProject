using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Popups.Menu.BannersCarousel
{
    public class InfiniteCarousel : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("Refs")] 
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private ScreenResolutionDetector resolutionDetector;
        [SerializeField] private List<RectTransform> banners;
        [SerializeField] private List<Dot> dots;

        [Header("Settings")] 
        [SerializeField] private float autoScrollDelay = 5f;
        [SerializeField] private float scrollDuration = 0.35f;
        [SerializeField] private float swipeThreshold = 50f;

        private float _bannerWidth;
        private bool _isAnimating;
        private Coroutine _autoScrollRoutine;
        private Vector2 _dragStartPos;
        private int _currentIndex;

        private void Awake()
        {
            resolutionDetector.OnScreenResolutionChange += RefreshBannerWidth;
        }

        private void Start()
        {
            if (banners == null || banners.Count == 0)
                return;

            _bannerWidth = banners[0].rect.width;
            _currentIndex = 0;

            ArrangeCentered();
            RefreshDots();

            _autoScrollRoutine = StartCoroutine(AutoScroll());
        }

        private void OnDestroy()
        {
            resolutionDetector.OnScreenResolutionChange -= RefreshBannerWidth;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isAnimating)
            {
                return;
            }

            _dragStartPos = eventData.position;

            StopAutoScroll();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isAnimating)
            {
                return;
            }

            var delta = eventData.position.x - _dragStartPos.x;

            if (Mathf.Abs(delta) < swipeThreshold)
            {
                SnapBack();
            }
            else if (delta < 0)
            {
                ScrollNext();
            }
            else
            {
                ScrollPrev();
            }

            _autoScrollRoutine = StartCoroutine(AutoScroll());
        }

        private void ArrangeCentered()
        {
            var centerIndex = banners.Count / 2;

            for (var i = 0; i < banners.Count; i++)
            {
                var x = (i - centerIndex) * _bannerWidth;

                banners[i].anchoredPosition = new Vector2(x, 0);
            }

            content.anchoredPosition = Vector2.zero;
        }

        private void RefreshDots()
        {
            if (dots == null || dots.Count == 0)
            {
                return;
            }

            for (var i = 0; i < dots.Count; i++)
            {
                dots[i].SetActive(i == _currentIndex);
            }
        }

        private void RefreshBannerWidth()
        {
            if (banners == null || banners.Count == 0)
            {
                return;
            }

            _bannerWidth = banners[0].rect.width;
        }

        private void StopAutoScroll()
        {
            if (_autoScrollRoutine != null)
            {
                StopCoroutine(_autoScrollRoutine);
            }
        }

        private IEnumerator AutoScroll()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoScrollDelay);

                if (!_isAnimating)
                {
                    ScrollNext();
                }
            }
        }

        private void ScrollNext()
        {
            _isAnimating = true;

            KillVelocity();

            scrollRect.enabled = false;

            var endPos = content.anchoredPosition + Vector2.left * _bannerWidth;

            content
                .DOAnchorPos(endPos, scrollDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    var first = banners[0];
                    banners.RemoveAt(0);
                    banners.Add(first);

                    RebuildPositions();

                    content.anchoredPosition = Vector2.zero;

                    _currentIndex++;
                    if (_currentIndex >= dots.Count)
                    {
                        _currentIndex = 0;
                    }

                    RefreshDots();

                    scrollRect.enabled = true;
                    _isAnimating = false;
                });
        }

        private void ScrollPrev()
        {
            _isAnimating = true;

            KillVelocity();

            scrollRect.enabled = false;

            var endPos = content.anchoredPosition + Vector2.right * _bannerWidth;

            content
                .DOAnchorPos(endPos, scrollDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    var last = banners[^1];
                    banners.RemoveAt(banners.Count - 1);
                    banners.Insert(0, last);

                    RebuildPositions();

                    content.anchoredPosition = Vector2.zero;

                    _currentIndex--;
                    if (_currentIndex < 0)
                    {
                        _currentIndex = dots.Count - 1;
                    }

                    RefreshDots();

                    scrollRect.enabled = true;
                    _isAnimating = false;
                });
        }

        private void RebuildPositions()
        {
            var centerIndex = banners.Count / 2;

            for (var i = 0; i < banners.Count; i++)
            {
                var x = (i - centerIndex) * _bannerWidth;

                banners[i].anchoredPosition = new Vector2(x, 0);
            }
        }

        private void SnapBack()
        {
            _isAnimating = true;

            KillVelocity();

            scrollRect.enabled = false;

            content
                .DOAnchorPos(Vector2.zero, scrollDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    scrollRect.enabled = true;

                    _isAnimating = false;
                });
        }

        private void KillVelocity()
        {
            scrollRect.velocity = Vector2.zero;
            scrollRect.StopMovement();
        }
    }
}