using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterGroup : MonoCache
    {
        public List<GameObject> _characters;
        public List<RuntimeAnimatorController> _characterControllers;
        private bool _isShowed = false;
        private int _selectedIndex=-1;
        private CharacterStore _characterStore;
        private Animator _animator;
        private PlayerCharacterAnimController animController;
        
        public void Initialize(CharacterStore characterStore)
        {
            _characterStore = characterStore;
            characterStore.OnUpdateSelectedCharacter += OnStoreActive;
            _animator=GetComponent<Animator>();
            animController=GetComponent<PlayerCharacterAnimController>();
        }

        private void OnStoreActive()
        {
            ShowCharacter(_characterStore.SelectedCharacterSlot.Type);
        }


        private void ShowCharacter(CharacterType type)
        {
            int index = (int)type;

            if (index != -1&&_selectedIndex!=index)
            {
                _characters[index].SetActive(true);
                _animator.runtimeAnimatorController = _characterControllers[index];
                animController.OnShoot(true);
            }
            
            
            
            if (_selectedIndex!=-1)
            {
                _characters[_selectedIndex].SetActive(false);
            }
            
            _selectedIndex = index;
        }
    }
}