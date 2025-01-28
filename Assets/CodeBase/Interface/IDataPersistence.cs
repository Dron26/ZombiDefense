using Data;
using Services;

namespace Interface
{
    public interface IDataPersistence: IService
    {
        void Save();
        GameData LoadData();
        void ClearData();
        void ResetProgress();
        void SetFirstStart();
        void OnGameStart();
        void OnGameEnd();
    }
}