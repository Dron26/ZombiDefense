using Data;

namespace Services.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        public GameData GetGameData();
        public void Save();
        public GameData Load();
        public void Reset();
        public void Initialize(ISerializationService jsonSerializationService);
        public void ChangeFirstStart();
    }
}