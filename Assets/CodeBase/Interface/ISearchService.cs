using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Service;
using UnityEngine;

namespace Interface
{
    public interface ISearchService: IService
    {
        void AddEntity(Entity entity);
        void RemoveEntity(Entity entity);
        T GetClosestEntity<T>(Vector3 position, Entity excludeEntity = null)where T : Entity;
        List<T> GetEntitiesInRange<T>(Vector3 position, float range) where T : Entity;
        T [] GetEntitiesInCircularRange<T>(Vector3 position, float maxRange, float minRange) where T : Entity;
    }
}

