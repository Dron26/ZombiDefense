using Infrastructure.AssetManagement;
using Services;
using UnityEngine;

namespace Infrastructure.Factories.FactoryGame
{
    public interface IGameFactory
    {
        GameObject CreateMenu();
        GameObject CreateLevel();
    }
}