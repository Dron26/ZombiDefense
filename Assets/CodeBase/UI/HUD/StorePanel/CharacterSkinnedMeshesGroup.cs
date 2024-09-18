using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterSkinnedMeshesGroup : MonoCache
    {
        private List<List<GameObject>> _characterSkinnedMeshes;
        private bool _isShowed = false;
        private int _selectedIndex = -1;
        private CharacterStore _characterStore;

        public void Initialize(CharacterStore characterStore)
        {
            _characterSkinnedMeshes = new List<List<GameObject>>();
            _characterStore = characterStore;
            characterStore.OnUpdateSelectedCharacter += OnStoreActive;
            _characterSkinnedMeshes = characterStore.CharacterSkinnedMeshes;
        }

        private void OnStoreActive()
        {
            ShowCharacter(_characterStore.SelectedCharacterSlot.Type);
        }


        private void ShowCharacter( CharacterType type)
        {
            int index = (int)type;
            if (_selectedIndex != -1)
            {
                HideCharacter();
            }

            foreach (GameObject obj in _characterSkinnedMeshes[index])
            {
                obj.SetActive(true);
            }

            _selectedIndex = index;
        }

        private void HideCharacter()
        {
            foreach (GameObject obj in _characterSkinnedMeshes[_selectedIndex])
            {
                obj.SetActive(false);
            }
        }
    }
}