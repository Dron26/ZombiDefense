using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Humanoids;
using UI.HUD.LuckySpin;
using UnityEngine;

namespace Observer
{
    public class ObserverLevel : MonoCache
    {
        [SerializeField] private HumanoidFactory _humanoidFactory;
        [SerializeField] private CanvasResultBar _canvasResult;

        private const int WaitTime = 200;

        private void Start() => 
            CheckEndBattle();

        private async void CheckEndBattle()
        {
            // bool isWork = true;
            //
            // while (isWork)
            // {
            //     List<Humanoid> aliveHumanoids = _humanoidFactory.GetAllHumanoids.Where(humanoid => 
            //         humanoid.IsLife()).ToList();
            //
            //     List<Enemy> aliveEnemies = _humanoidFactory.GetAllEnemies.Where(enemy => 
            //         enemy.IsLife()).ToList();
            //      
            //     if (aliveEnemies.Count == 0)
            //     {
            //         ShowResult();
            //         _canvasResult.DrawResult("WIN!");
            //         isWork = false;
            //     }
            //
            //     if (aliveHumanoids.Count == 0)
            //     {
            //         ShowResult();
            //         _canvasResult.DrawResult("DEFEAT!");
            //         isWork = false;
            //     }
            //     
            //     await UniTask.Delay(WaitTime);
            // }
        }

        private void ShowResult()
        {
            _canvasResult.CalculateBonus(_humanoidFactory);
            _canvasResult.gameObject.SetActive(true);
        }
    }
}