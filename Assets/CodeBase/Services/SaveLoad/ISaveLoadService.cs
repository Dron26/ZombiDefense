using System.Collections.Generic;
using Data;
using Interface;

namespace Services.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        public GameData GetGameData();
        public void Save();
        public GameData Load();
        public void Reset();
        public void UpdateLocationProgressData(List <LocationProgressData> locationProgressData);
        public void Initialize(ISerializationService jsonSerializationService);
        public void ChangeFirstStart();
    }
}