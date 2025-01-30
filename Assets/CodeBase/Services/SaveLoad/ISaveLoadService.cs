using Data;

namespace Services.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        public void Save();

        public void Load();

        public void Reset();
        public void Initialize(ISerializationService jsonSerializationService);
    }
}