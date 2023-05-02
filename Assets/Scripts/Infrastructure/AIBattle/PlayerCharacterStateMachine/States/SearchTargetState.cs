using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Weapon;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class SearchTargetState : State
    {
        private MovementState _movementState;
        private AttackState _attackState;

        private Enemy _targetEnemy;

        // Add a field to hold the array of enemy transforms.
        private Transform[] _enemyTransforms;

        private void Start()
        {
            _movementState = GetComponent<MovementState>();
            _attackState = GetComponent<AttackState>();
            // Get an array of enemy transforms.
            _enemyTransforms = HumanoidFactory.GetAllEnemies
                .Select(enemy => enemy.transform)
                .ToArray();
        }

        protected override void UpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;

            Search();
        }

        private void Search()
        {
            if (TryGetComponent(out Humanoid _))
            {
                // Call the method to get the index of the closest enemy.
                int closestEnemyIndex = GetClosestEnemyIndex(transform.position);

                // If the closest enemy index is valid, use that enemy.
                if (closestEnemyIndex != -1)
                {
                    _targetEnemy = HumanoidFactory.GetAllEnemies[closestEnemyIndex];
                    _attackState.InitEnemy(_targetEnemy);
                    PlayerCharactersStateMachine.EnterBehavior<AttackState>();
                }
            }
        }

        private int GetClosestEnemyIndex(Vector3 soldierPosition)
        {
            // Create a NativeArray of EnemyPositionData and fill it with the data we need.
            NativeArray<EnemyPositionData> enemyPositionDataArray = new NativeArray<EnemyPositionData>(_enemyTransforms.Length, Allocator.TempJob);
        
            for (int i = 0; i < _enemyTransforms.Length; i++)
            {
                enemyPositionDataArray[i] = new EnemyPositionData
                {
                    soldierPosition = soldierPosition,
                    enemyPosition = _enemyTransforms[i].position
                };
            }

            // Create a JobHandle and schedule the job.
            JobHandle jobHandle = new GetClosestEnemyJob
            {
                enemyPositionDataArray = enemyPositionDataArray
            }.Schedule(_enemyTransforms.Length, 10);

            // Wait for the job to complete.
            jobHandle.Complete();

            // Find the index of the closest enemy from the results of the job.
            int closestEnemyIndex = -1;
            float closestEnemyDistance = float.MaxValue;
            for (int i = 0; i < _enemyTransforms.Length; i++)
            {
                float distance = math.distance(soldierPosition, enemyPositionDataArray[i].enemyPosition);
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemyIndex = i;
                }
            }

            // Dispose of the NativeArray.
            enemyPositionDataArray.Dispose();

            return closestEnemyIndex;
        }

        // Define a struct to hold the data needed by the job.
        private struct EnemyPositionData
        {
            public Vector3 soldierPosition;
            public Vector3 enemyPosition;
        }

        // Define the job that will be run in parallel by the Job System.
        private struct GetClosestEnemyJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EnemyPositionData> enemyPositionDataArray;
            // Add a field to hold the array of results.
            private NativeArray<EnemyDistanceData> resultArray;

            // Define a struct to hold the results of the job.
            private struct EnemyDistanceData
            {
                public int enemyIndex;
                public float distance;
            }
        
            public void Execute(int index)
            {
                EnemyPositionData enemyPositionData = enemyPositionDataArray[index];
                float distance = math.distance(enemyPositionData.soldierPosition, enemyPositionData.enemyPosition);

                SetResultArraySize(enemyPositionDataArray.Length);
            
                // Store the result in the corresponding element of the result array.
                resultArray[index] = new EnemyDistanceData
                {
                    enemyIndex = index,
                    distance = distance
                };
            }
        
        
            // Add a method to set the size of the result array.
            public void SetResultArraySize(int size)
            {
                resultArray = new NativeArray<EnemyDistanceData>(size, Allocator.TempJob);
            }
        }
    }
}