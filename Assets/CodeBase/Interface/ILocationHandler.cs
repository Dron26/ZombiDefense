using System.Collections.Generic;
using Services;
using Services.SaveLoad;
using UI.Locations;

namespace Interface
{
    public interface ILocationHandler:IService
    {
        void LocationCompleted();
        List<int> GetCompletedLocationId();
        void SetLocationsDatas(List<LocationData> locationDatas);
        void SetSelectedLocationId(int id);
        int GetSelectedLocationId();
        void Reset();
        bool IsExitFromLocation { get; set; }
    }
}