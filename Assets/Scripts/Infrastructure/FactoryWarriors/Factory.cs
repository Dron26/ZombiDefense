using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.GeneralFactory;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors
{ 
    enum Level
    {
        Soldier = 1,
        Archer = 2,
        Knight = 3,
        King = 4,
        
        CyberSoldier = 5,
        CyberArcher = 6,
        CyberKnight = 7,
        CyberKing = 8,
        
        CrazyTractor = 9,
        CyberZombie = 10,
        GunGrandmother = 11,
        Virus = 12
    }
    
    public class Factory : MonoCache, IServiceFactory
    {
        private const float PositionY = 0f;
        private const int MinPositionX = 10;
        private const int MaxPositionX = 25;
        private const int PositionZ = 30;
        
        private static readonly List<Humanoid> _humanoids = new();
        private static readonly List<Enemy> _enemies = new();
        protected SaveLoad _saveLoad;
        
        public void Awake()
        {
            _saveLoad=FindObjectOfType<SaveLoad>();
        }
        
        protected void InitEnemy(Enemy enemy, int capacity)
        {
            if (enemy != null && capacity > 0)
            {
                for (int i = 0; i < capacity; i++)
                {
                    enemy = Instantiate(enemy, InitRandomPosition(MinPositionX, MaxPositionX,
                            PositionZ, -PositionZ),
                        Quaternion.identity);
                    enemy.GetComponent<State>().InitFactory(this);
                    _enemies.Add(enemy);
                }
            }
        }   

        protected void InitHumanoid(Humanoid humanoid, int capacity)
        {
            if (humanoid != null && capacity > 0)
            {
                for (int i = 0; i < capacity; i++)
                {
                    humanoid = Instantiate(humanoid,
                        InitRandomPosition(-MinPositionX, -MaxPositionX,
                            PositionZ, -PositionZ), Quaternion.identity);
                    humanoid.GetComponent<State>().InitFactory(this);
                    _humanoids.Add(humanoid);
                }
            }
        }

        private Vector3 InitRandomPosition(int minPositionX, int maxPositionX, int minPositionZ, int maxPositionZ)
        {
            return new Vector3(
                Random.Range(minPositionX, maxPositionX),
                PositionY,
                Random.Range(minPositionZ, maxPositionZ));
        }

        public List<Humanoid> GetAllHumanoids =>
            _humanoids;

        public List<Enemy> GetAllEnemies => 
            _enemies;
        
        
        
        
    }
}