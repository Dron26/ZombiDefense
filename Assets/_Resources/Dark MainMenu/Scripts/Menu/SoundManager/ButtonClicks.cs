using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Resources.Dark_MainMenu.Scripts.Menu.SoundManager
{
    public class ButtonClicks : MonoCache  , IPointerEnterHandler, IPointerClickHandler
    {
        public AudioSource clickSource;

        public AudioClip HoverSound;
        public AudioClip ClickSound;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            clickSource.PlayOneShot(HoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickSource.PlayOneShot(ClickSound);
        }
    }
}

