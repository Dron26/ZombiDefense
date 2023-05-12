using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.Location;
using UnityEditor;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    public class StoreOnPlay:MonoCache
    {
        private Panel _storePanel;
        private Button _showButton;
        private Button _closeButton;
        private WorkPoint _selectedWorkPoint;
        private Image _dimImage;
        private  List<Humanoid> _characters = new();
       private List<WorkPoint> _points ;
       private PlayerCharacterInitializer _characterInitializer;
       
        private void Initialize(PlayerCharacterInitializer initializer)
        {
            _characterInitializer=initializer;
            initializer.OnClickWorkpoint += SetPointInfo;
            _storePanel=GetComponentInChildren<Panel>();
            _storePanel.Initialize( _characterInitializer);
            _dimImage=_storePanel.GetComponentInChildren<Image>();
            InitializeButton();
            ClosePanel();
        }


        private void InitializeButton()
        {
            _closeButton=_dimImage.GetComponent<Button>();
            _closeButton.onClick.AddListener(ClosePanel);
        }

        private void SetPointInfo(WorkPoint workPoint)
        {
            print(workPoint.transform.position);
        }

        private void ShowPanel()
        { 
            _storePanel.gameObject.SetActive(true);
            _storePanel.ShowAvaibleCharacters();
        }

        private void ClosePanel()
        {
            _storePanel.gameObject.SetActive(false);
        }

        public List<Humanoid>  GetAvaibleCharacters()
        {
            return _characters;
        }

       

    }
}