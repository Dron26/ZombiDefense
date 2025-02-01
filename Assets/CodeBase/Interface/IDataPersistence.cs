using Data;
using Services;

namespace Interface
{
    public interface IDataPersistence:IService
    {
        void Save(GameData data);
        GameData Load();
        void Reset();
    }
}