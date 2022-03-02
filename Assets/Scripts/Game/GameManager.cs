using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class GameManager : MonoBehaviour {

  public static Vector3[] nodePositions;

  public delegate void ResultCallback(int test);

  private static Queue<Enemy> enemiesToRemove;
  private static Queue<long> enemyIdsToSpawn;
 
  public Transform nodeParent;
  public bool running;

  // Start is called before the first frame update
  void Start() {
    enemyIdsToSpawn = new Queue<long>();
    enemiesToRemove = new Queue<Enemy> ();
    EnemySpawner.Init();

    nodePositions = new Vector3[nodeParent.childCount];

    for (int i = 0; i < nodePositions.Length; i++) {
      nodePositions[i] = nodeParent.GetChild(i).position;
    }

    StartCoroutine(GameLoop());
    InvokeRepeating("SpawnTest", 0f, 3f);
  }

  void SpawnTest() {
    EnqueueEnemyidToSpawn(1);
  }

  IEnumerator GameLoop() {
    while (running) {
      if (enemyIdsToSpawn.Count > 0) {
        for (int i = 0; i < enemyIdsToSpawn.Count; i++) {
          EnemySpawner.SpawnEnemy(enemyIdsToSpawn.Dequeue());
        }
      }

      NativeArray<int> nodeIndices = new NativeArray<int>(EnemySpawner.enemies.Count, Allocator.TempJob);
      NativeArray<float> enemySpeeds = new NativeArray<float>(EnemySpawner.enemies.Count, Allocator.TempJob);
      NativeArray<Vector3> nodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
      TransformAccessArray enemyAccess = new TransformAccessArray(EnemySpawner.enemyTransforms.ToArray(), 2);

      for (int i = 0; i < EnemySpawner.enemies.Count; i++) {
        enemySpeeds[i] = EnemySpawner.enemies[i].speed;
        nodeIndices[i] = EnemySpawner.enemies[i].nodeIndex;
      }

      MoveEnemiesJob moveJob = new MoveEnemiesJob {
        nodePositions = nodesToUse,
        enemySpeeds = enemySpeeds,
        nodeIndices = nodeIndices,
        deltaTime = Time.deltaTime
      };

      JobHandle moveJobHandle = moveJob.Schedule(enemyAccess);
      moveJobHandle.Complete();

      for (int i = 0; i < EnemySpawner.enemies.Count; i++) {
        EnemySpawner.enemies[i].nodeIndex = nodeIndices[i];

        if (EnemySpawner.enemies[i].nodeIndex == nodePositions.Length) {
          EnqueueEnemyToRemove(EnemySpawner.enemies[i]);
        }
      }

      nodesToUse.Dispose();
      enemySpeeds.Dispose();
      nodeIndices.Dispose();
      enemyAccess.Dispose();

      if (enemiesToRemove.Count > 0) {
        for (int i = 0; i < enemiesToRemove.Count; i++) {
          EnemySpawner.RemoveEnemy(enemiesToRemove.Dequeue());
        }
      }
      yield return null;
    }
  }

  public static void EnqueueEnemyidToSpawn(long id) {
    enemyIdsToSpawn.Enqueue(id);
  }

  public static void EnqueueEnemyToRemove(Enemy enemyToRemove) {
    enemiesToRemove.Enqueue(enemyToRemove);
  }

}

public struct MoveEnemiesJob : IJobParallelForTransform {

  [NativeDisableParallelForRestriction]
  public NativeArray<Vector3> nodePositions;

  [NativeDisableParallelForRestriction]
  public NativeArray<float> enemySpeeds;

  [NativeDisableParallelForRestriction]
  public NativeArray<int> nodeIndices;

  public float deltaTime;

  public void Execute(int index, TransformAccess transform) {

    if (nodeIndices[index] >= nodePositions.Length) {
      return;
    }
    Vector3 positionToMoveTo = nodePositions[nodeIndices[index]];

    transform.position = Vector3.MoveTowards(
      transform.position, positionToMoveTo, enemySpeeds[index] * deltaTime
    );

    if (transform.position == positionToMoveTo) {
      nodeIndices[index]++;
    }
  }
}
