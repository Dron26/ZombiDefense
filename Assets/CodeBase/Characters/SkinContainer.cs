using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Characters
{
    public class SkinContainer:MonoCache
    {
        public List<GameObject> _skins = new List<GameObject>();

        public void SetSkin(CharacterType type)
        {
            _skins[(int)type].gameObject.SetActive(true);
        }
    }
}