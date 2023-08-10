
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Infrastructure
{
    public  class ClosestIndexCharacter
    {
        public int GetClosestIndex(Vector3 soldierPosition, Transform[] _enemyTransforms)
        {
            NativeArray<EnemyPositionData> enemyPositionDataArray = new NativeArray<EnemyPositionData>(_enemyTransforms.Length, Allocator.TempJob);

            for (int i = 0; i < _enemyTransforms.Length; i++)
            {
                enemyPositionDataArray[i] = new EnemyPositionData
                {
                    soldierPosition = soldierPosition,
                    enemyPosition = _enemyTransforms[i].position
                };
            }

            // Create a job and set the size of the result array.
            var job = new GetClosestEnemyJob
            {
                enemyPositionDataArray = enemyPositionDataArray
            };
            
            job.SetResultArraySize(_enemyTransforms.Length);

            // Schedule the job and wait for it to complete.
            var jobHandle = job.Schedule(_enemyTransforms.Length, 10);
            jobHandle.Complete();

            // Find the index of the closest enemy from the results of the job.
            int closestEnemyIndex = -1;
            float closestEnemyDistance = float.MaxValue;
            for (int i = 0; i < _enemyTransforms.Length; i++)
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
    }
}