using Data;

namespace Service.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        void SaveData();
        GameData LoadData();
    }
}