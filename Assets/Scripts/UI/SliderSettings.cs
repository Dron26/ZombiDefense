using Infrastructure.BaseMonoCache.Code.MonoCache;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderSettings:MonoCache
    {
        [HideInInspector] public GameObject Fill;

        private void Start()
        {
            Fill=GetComponentInChildren<RectMask2D>().gameObject;
        }
    }
}