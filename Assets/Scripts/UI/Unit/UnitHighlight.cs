using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Unit
{
    public class UnitHighlight : MonoCache
    {
        private Image _highlightImage;
        public float fadeDuration = 0.2f;

        private float _originalAlpha;
        private float targetAlpha;

        private bool _isHighlightedPrev = false;
        private bool _isHighlighted = false;

        private void Start()
        {
            _highlightImage = GetComponentInChildren<Image>();
            _originalAlpha = _highlightImage.color.a;
            targetAlpha = _originalAlpha * 0.5f;
            _highlightImage.color = new Color(_highlightImage.color.r, _highlightImage.color.g, _highlightImage.color.b, 0f);
        }

        public async void SetHighlighted(bool value)
        {
            if (value != _isHighlighted)
            {
                _isHighlighted = value;
                _isHighlightedPrev = _isHighlighted;
                StopAllCoroutines();
                await FadeHighlight();
            }
        }

        private async Task FadeHighlight()
        {
            float startAlpha = _isHighlighted ? 0f : 1f;
            float endAlpha = _isHighlighted ? 1f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                if (_highlightImage != null && _highlightImage.IsActive())
                {
                    _highlightImage.color = new Color(_highlightImage.color.r, _highlightImage.color.g, _highlightImage.color.b, alpha);
                }
                if (_isHighlightedPrev != _isHighlighted) break;
                await Task.Yield();
            }

            if (_highlightImage != null && _highlightImage.IsActive())
            {
                _highlightImage.color = new Color(_highlightImage.color.r, _highlightImage.color.g, _highlightImage.color.b, endAlpha);
            }
            
        }
    }
}