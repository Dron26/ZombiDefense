using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterSkinnedMeshesGroup: MonoCache
    {  
        [SerializeField] private  List<GameObject> _ordinarySoldier;  
        [SerializeField] private  List<GameObject> _sergeant;  
        [SerializeField] private  List<GameObject> _grenader;  
        [SerializeField] private  List<GameObject> _scout;  
        [SerializeField] private  List<GameObject> _sniper;

        
        
        
        private  List<List<GameObject>> _characterSkinnedMeshes;
private bool _isShowed = false;
private int _selectedIndex;
        public void Awake()
        {
            _characterSkinnedMeshes=new List<List<GameObject>>();
            FillCharacters();
        }
       
        private void FillCharacters()
        {
            _characterSkinnedMeshes.Add(_ordinarySoldier);
            _characterSkinnedMeshes.Add(_sergeant);
            _characterSkinnedMeshes.Add(_grenader);
            _characterSkinnedMeshes.Add(_scout);
            _characterSkinnedMeshes.Add(_sniper);
            
            
            foreach (List<GameObject> obj in _characterSkinnedMeshes)
            {
                foreach (GameObject obj2 in obj)
                {
                    obj2.SetActive(false);
                }
            }
        }


        public void ShowCharacter(int index)
        {
            if (_selectedIndex!=index)
            {
                foreach (GameObject obj in _characterSkinnedMeshes[_selectedIndex])
                {
                    obj.SetActive(false);
                }
            }
            
            foreach (GameObject obj in _characterSkinnedMeshes[index])
            {
                obj.SetActive(true);
            }
            
            _selectedIndex=index;
        }
    }
}