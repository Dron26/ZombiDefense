using Infrastructure.AssetManagement;
using UnityEngine;

namespace Infrastructure.FactoryGame
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        
        public GameFactory(IAssets assets) => 
            _assets = assets;
        
        public GameObject CreateMainScene() => 
            _assets.Instantiate(AssetPath.MainScene);
        
        public GameObject CreateSceneBattle() => 
            _assets.Instantiate(AssetPath.SceneSwitcher);
    }
}
