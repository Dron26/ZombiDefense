using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemySearchTargetState : EnemyState
    {
        private EnemyMovementState _movementState;
        private EnemyAttackState _attackState;
        private Character _targetCharacter;
        private Enemy _enemy;
        private NavMeshAgent agent;
        private Character[] _humanoidTransforms;
        private bool _isSearhing;

        private void Awake()
        {
            _movementState = GetComponent<EnemyMovementState>();
            _attackState = GetComponent<EnemyAttackState>();
            agent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
        }

        public override void OnTakeGranadeDamage()
        {
            agent.speed = 0;
            StateMachine.EnterBehavior<EnemyStunningState>();
            _isSearhing = false;
        }

        protected override void UpdateCustom()
        {
            if (_isSearhing == false&&_enemy.IsLife()) Search();
        }

        private void Search()
        {
            agent.speed = 0;
            _isSearhing = true;
            _humanoidTransforms = saveLoadService.GetActiveCharacters()
                .Where(character=> character.IsLife == true)
                .Where(character=> character.TryGetComponent(out Humanoid humanoid) == true)
                .ToArray();
            
            
            
            
            int closestIndex = GetClosestIndex(transform.position, _humanoidTransforms);

            if (closestIndex != -1)
            {
                _targetCharacter = _humanoidTransforms[closestIndex];
                _movementState.InitCharacter(_targetCharacter);
                _attackState.InitCharacter(_targetCharacter);

                EnemyMovementState _enemyMovement = GetComponent<EnemyMovementState>();
                _enemyMovement.SetTarget(false);
                agent.speed = 1;
                StateMachine.EnterBehavior<EnemyMovementState>();
            }

            _isSearhing = false;
        }

        private int GetClosestIndex(Vector3 soldierPosition, Character[] humanoidTransforms)
        {
            NativeArray<EnemyPositionData> enemyPositionDataArray =
                new NativeArray<EnemyPositionData>(humanoidTransforms.Length, Allocator.TempJob);

            for (int i = 0; i < humanoidTransforms.Length; i++)
            {
                enemyPositionDataArray[i] = new EnemyPositionData
                {
                    soldierPosition = soldierPosition,
                    enemyPosition = humanoidTransforms[i].transform.position
                };
            }

            // Create a job and set the size of the result array.
            var job = new GetClosestEnemyJob
            {
                enemyPositionDataArray = enemyPositionDataArray
            };

            job.SetResultArraySize(humanoidTransforms.Length);

            // Schedule the job and wait for it to complete.
            var jobHandle = job.Schedule(humanoidTransforms.Length, 10);
            jobHandle.Complete();

            // Find the index of the closest enemy from the results of the job.
            int closestEnemyIndex = -1;
            float closestEnemyDistance = float.MaxValue;
            for (int i = 0; i < humanoidTransforms.Length; i++)
            {
                float distance = job.resultArray[i].distance;
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemyIndex = job.resultArray[i].enemyIndex;
                }
            }

            enemyPositionDataArray.Dispose();
            job.resultArray.Dispose();

            return closestEnemyIndex;
        }

        private struct EnemyPositionData
        {
            public Vector3 soldierPosition;
            public Vector3 enemyPosition;
        }

        private struct GetClosestEnemyJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EnemyPositionData> enemyPositionDataArray;
            public NativeArray<EnemyDistanceData> resultArray;

            public struct EnemyDistanceData
            {
                public int enemyIndex;
                public float distance;
            }

            public void Execute(int index)
            {
                EnemyPositionData enemyPositionData = enemyPositionDataArray[index];
                float distance = math.distance(enemyPositionData.soldierPosition, enemyPositionData.enemyPosition);

                resultArray[index] = new EnemyDistanceData
                {
                    enemyIndex = index,
                    distance = distance
                };
            }

            public void SetResultArraySize(int size)
            {
                resultArray = new NativeArray<EnemyDistanceData>(size, Allocator.TempJob);
            }
        }

        public override void Disable()
        {}
    }
}