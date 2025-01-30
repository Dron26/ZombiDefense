using Services;
using UnityEngine;

namespace Infrastructure
{
    public interface IGameFactory:IService
    {
        GameObject CreateMenu();
        GameObject CreateLevel();
    }
}