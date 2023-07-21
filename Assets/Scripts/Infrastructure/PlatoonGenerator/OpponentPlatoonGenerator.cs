using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnemiesUI.AbstractEntity;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.Unit;
using UnityEngine;

namespace Infrastructure.PlatoonGenerator
{
    public class OpponentPlatoonGenerator : MonoCache
    {
        private List<Enemy> _enemies = new();
        private List<int> _enemyLevels = new();
        private List<Enemy> _platoon = new();
        List<Type> types = new List<Type>();
        private List<GameObject> _slots = new();
        private GameObject _unit;
        private List<HumanoidUI> _playerPlatoon;

        public void Initialize(UIUnitEnemy enemyUnit, List<HumanoidUI> playerPlatoon)
        {
            _playerPlatoon = playerPlatoon;
            _unit = enemyUnit.gameObject;
            GeneratePlatoon();
        }


        private void GeneratePlatoon()
        {
            foreach (var unit in _playerPlatoon)
            {
                int levelFirst = unit.GetLevel();

                bool isFunded = false;

                var abstractClassType = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Alien))).ToList();

                foreach (var abstractClass in abstractClassType)
                {
                    Alien characterAlien = Activator.CreateInstance(abstractClass) as Alien;
                    
                    int i = characterAlien.GetLevel();
                    if (characterAlien.GetLevel() == levelFirst)
                    {
                        СreateEnemyUnit(characterAlien);
                        isFunded = true;
                        break;
                    }
                }

                if (isFunded == false)
                {
                    var abstractClassTypeTwo = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Pig))).ToList();

                    foreach (var abstractClassPig in abstractClassTypeTwo)
                    {
                        Pig characterPig = Activator.CreateInstance(abstractClassPig) as Pig;

                        if (characterPig.GetLevel() == levelFirst)
                        {
                            СreateEnemyUnit(characterPig);
                            break;
                        }
                    }
                }
            }
        }


        private void СreateEnemyUnit(Enemy character)
        {
            GameObject newCharacterObject = Instantiate(_unit, Vector3.zero, Quaternion.identity);
            Type characterType = character.GetType();

            if (newCharacterObject.TryGetComponent(characterType, out Component component))
            {
                Destroy(component);
            }
            else
            {
                newCharacterObject.AddComponent(characterType);
            }

            string name = characterType.Name;
            newCharacterObject.GetComponent<UIUnitEnemy>().Initialize(name);
            
            _platoon.Add(newCharacterObject.GetComponent<Enemy>());
        }
        public List<Enemy> GetPlatoon()
        {
            List<Enemy> newPatoon = new List<Enemy>();

            foreach (Enemy unit in _platoon)
            {
                newPatoon.Add(unit);
            }

            _platoon.Clear();

            return newPatoon;
        }
    }
}