using UnityEngine;
using UnityEngine.EventSystems;

namespace Plugins.ButtonSoundsEditor
{
    [DisallowMultipleComponent]
    public class ButtonClickSound : MonoBehaviour , IPointerEnterHandler, IPointerClickHandler
    {
        public AudioSource AudioSource;
        public AudioClip ClickSound;
        public AudioClip HoverSound;
        
        // public void Awake()
        // {
        //     Button button = GetComponent<Button>();
        //     if (button != null)
        //     {
        //         button.onClick.AddListener(PlayClickSound);
        //     }
        //
        //     EventTrigger eventTrigger = GetComponent<EventTrigger>();
        //     if (eventTrigger != null)
        //     {
        //         EventTrigger.Entry clickEntry = eventTrigger.triggers.SingleOrDefault(_ => _.eventID == EventTriggerType.PointerClick);
        //         if (clickEntry != null)
        //             clickEntry.callback.AddListener(_ => PlayClickSound());
        //     }
        // }

       
        private void PlayClickSound()
        {
            AudioSource.Play();
        }

        public void Initiallize(AudioSource audioSource)
        {
            AudioSource = audioSource;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioSource.PlayOneShot(HoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioSource.PlayOneShot(HoverSound);
        }
    }

}
