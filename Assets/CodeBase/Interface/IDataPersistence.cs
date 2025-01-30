using Data;
using Services;

namespace Interface
{
    public interface IDataPersistence: IService
    {
        void Save();
        GameData LoadData();
        void OnGameStart();
        void OnGameEnd();
        void Reset();
    }
}