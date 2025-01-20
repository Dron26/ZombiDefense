using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Services;
using UnityEngine;

namespace Interface
{
    public interface ISearchService: IService
    {
        public void AddEntity(Entity entity);
        public void RemoveEntity(Entity entity);
        public void ClearAllEntities();
        public  T GetClosestEntity<T>(Vector3 position, Entity excludeEntity = null)where T : Entity;
        public  List<T> GetEntitiesInRange<T>(Vector3 position, float range) where T : Entity;
        public  T [] GetEntitiesInCircularRange<T>(Vector3 position, float maxRange, float minRange) where T : Entity;
    }
}

