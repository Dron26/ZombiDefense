using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using UI.HUD.LuckySpin;
using UnityEngine;

namespace Observer
{
    public class ObserverLevel : MonoCache
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private CanvasResultBar _canvasResult;

        private const int WaitTime = 200;

        private void Start() => 
            CheckEndBattle();

        private async void CheckEndBattle()
        {
            bool isWork = true;
            
            while (isWork)
            {
                List<Humanoid> aliveHumanoids = _factory.GetAllHumanoids.Where(humanoid => 
                    humanoid.IsLife()).ToList();

                List<Enemy> aliveEnemies = _factory.GetAllEnemies.Where(enemy => 
                    enemy.IsLife()).ToList();
                 
                if (aliveEnemies.Count == 0)
                {
                    ShowResult();
                    _canvasResult.DrawResult("WIN!");
                    isWork = false;
                }

                if (aliveHumanoids.Count == 0)
                {
                    ShowResult();
                    _canvasResult.DrawResult("DEFEAT!");
                    isWork = false;
                }
                
                await UniTask.Delay(WaitTime);
            }
        }

        private void ShowResult()
        {
            _canvasResult.CalculateBonus(_factory);
            _canvasResult.gameObject.SetActive(true);
        }
    }
}