using Data;

namespace Service.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        void SaveData();
        void ClearData();//new game
        DataBase LoadData();
    }
}