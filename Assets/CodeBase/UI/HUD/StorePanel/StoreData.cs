using System.Collections.Generic;

namespace UI.HUD.StorePanel
{
    public class StoreData
    {
        private List<int> _indexAvailableHumanoid;
         
        public void Initialize(  List<int> indexAvailableHumanoid)
        {
            _indexAvailableHumanoid = indexAvailableHumanoid;
        }
    }
}