using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Characters
{
    public class AttachmentSetter:MonoCache
    {
        [SerializeField] private List<GameObject> _elements;
        [SerializeField] private List<CharacterType> _types;
        
        public void SetAttachments(CharacterType type)
        {
            if (_types.Contains(type))
            {
                _elements[_types.IndexOf(type)].SetActive(true);
            }
        }

        protected void OnDestroy()
        {
            foreach (GameObject obj in _elements)
            {
                obj.SetActive(false);
            }
        }
    }
}