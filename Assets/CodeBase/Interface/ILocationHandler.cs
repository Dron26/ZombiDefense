using System.Collections.Generic;
using Services;

namespace Interface
{
    public interface ILocationHandler : IService
    {
        void LocationCompleted(); 
        List<int> GetCompletedLocationId();  
        void SetLocationsDatas(List<LocationProgressData> locationProgressData);  
        void SetSelectedLocationId(int id);  
        int GetSelectedLocationId();  
        void Reset();  
        bool IsExitFromLocation { get; set; }  
        int SelectedLocationId { get; set; }  
        void SetMaxEnemyOnScene(int count);  
        void SetSelectedPointId(int id);  
        LocationProgressData GetCurrentLocationData();  
        int GetCurrentReward();  
        void IncreaseWaveLevel();  
    }
}