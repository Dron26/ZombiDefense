using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIMove : MonoCache
    {
        [SerializeField]private RectTransform[] _windows;
        [SerializeField]private RectTransform[] _windowsSecond;
       
        private List<RectTransform[]> _windiws;
        private readonly float _duration = 1f;

        private Vector2[] _startPositions;
        private Vector2[] _endPositions;
        private Tweener _tweener;
        private List<Tweener> _tweeners = new List<Tweener>();
        private List<Task> _tasks = new List<Task>();
        private bool _isFinished;
        public bool IsFinished => _isFinished;

        private void Awake()
        {
            _windiws = new List<RectTransform[]>();
            _windiws.Add(_windows);
            _windiws.Add(_windowsSecond);
            
        }

        public  void SlideIn(int id)
        {
            _startPositions = new Vector2[_windiws[id].Length];
            _endPositions = new Vector2[_windiws[id].Length];

            for (int i = 0; i < _windiws[id].Length; i++)
            {
                var window = _windiws[id][i];

                _endPositions[i] = window.anchoredPosition;
                float direction = Random.Range(0, 2) == 0 ? -1 : 1;

                if (direction == -1)
                {
                    _startPositions[i] = new Vector2(_endPositions[i].x - window.rect.width, _endPositions[i].y);
                }
                else
                {
                    _startPositions[i] = new Vector2(_endPositions[i].x, _endPositions[i].y - window.rect.height);
                }

                window.anchoredPosition = _startPositions[i];

                window.gameObject.SetActive(true);
                _tweener = window.DOAnchorPos(_endPositions[i], _duration).SetEase(Ease.OutCubic);
            }
        }

        private void OnDestroy()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
        }
    }
}