using Service;
using UnityEngine;

namespace Infrastructure.Factories.FactoryGame
{
    public interface IGameFactory:IService
    {
        GameObject CreateMenu();
        GameObject CreateLevel();
    }
}