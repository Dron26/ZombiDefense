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
