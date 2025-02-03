using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Interface;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure
{
    public class EntitySearchService : MonoCache,ISearchService
    {
        private List<Entity> _allEntities = new List<Entity>();
        private SceneObjectManager _sceneObjectManager;
        private WaveSpawner _waveSpawner;
        private SceneObjectManager sceneObjectManager;
    
        public void AddEntity(Entity entity)
        {
            if (!_allEntities.Contains(entity))
            {
                entity.OnEntityDeath += RemoveEntity;
                _allEntities.Add(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            _allEntities.Remove(entity);
        }

        public void ClearAllEntities()
        {
            _allEntities.Clear();
        }
        public T GetClosestEntity<T>(Vector3 position, Entity excludeEntity = null) where T : Entity
        {
            if (_allEntities.Count == 0)
                return null;

            T closestEntity = null;
            float closestSqrDistance = float.MaxValue;

            foreach (var entity in _allEntities)
            {
                if (entity == excludeEntity || !(entity is T))
                    continue;

                float sqrDistance  = (position - entity.transform.position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestEntity = entity as T;
                }
            }

            return closestEntity;
        }
    
        public List<T> GetEntitiesInRange<T>(Vector3 position, float range) where T : Entity
        {
            List<T> entitiesInRange = new List<T>();

            foreach (var entity in _allEntities)
            {
                if (entity is T) 
                {
                    float sqrDistance = (position - entity.transform.position).sqrMagnitude;
                    if (sqrDistance <= range * range) 
                    {
                        entitiesInRange.Add((T)entity); 
                    }
                }
            }

            return entitiesInRange;
        }
    
        public T[][] GetEntitiesInNextRange<T>(Vector3 position, List<float> ranges) where T : Entity
        {
            List<T> smallRangeEntities = new List<T>();
            List<T> mediumRangeEntities = new List<T>();
            List<T> largeRangeEntities = new List<T>();

            foreach (var entity in _allEntities)
            {
                if (entity is T) 
                {
                    float sqrDistance = (position - entity.transform.position).sqrMagnitude;
                
                    for (int i = 0; i < ranges.Count - 1; i++) 
                    {
                        float sqrRange = sqrDistance; 

                        if (sqrRange <= ranges[i] * ranges[i]) 
                        {
                            smallRangeEntities.Add((T)entity);
                        }
                        else if (sqrRange <= ranges[i + 1] * ranges[i + 1]) 
                        {
                            mediumRangeEntities.Add((T)entity);
                        }
                        else
                        {
                            largeRangeEntities.Add((T)entity);
                        }
                    }
                }
            }

            return new T[][] { smallRangeEntities.ToArray(), mediumRangeEntities.ToArray(), largeRangeEntities.ToArray() };
        }
    
        public T [] GetEntitiesInCircularRange<T>(Vector3 position, float maxRange, float minRange) where T : Entity
        {
            List<T> entitiesInRange = new List<T>();

            foreach (var entity in _allEntities)
            {
                if (entity is T) 
                {
                    float sqrDistance = (position - entity.transform.position).sqrMagnitude;
                    if (sqrDistance <= maxRange * maxRange && sqrDistance >= minRange * minRange) 
                    {
                        entitiesInRange.Add((T)entity); 
                    }
                }
            }

            return entitiesInRange.ToArray(); 
        }
    }
}