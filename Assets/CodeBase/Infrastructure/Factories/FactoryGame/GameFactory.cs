
    using Infrastructure;
    using Infrastructure.AssetManagement;
    using UnityEngine;

    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        
        public GameFactory(IAssets assets) => 
            _assets = assets;
        
        public GameObject CreateMenu() => 
            _assets.Instantiate(AssetPaths.Menu);

        public GameObject CreateLevel() => 
            _assets.Instantiate(AssetPaths.Level);
    }
