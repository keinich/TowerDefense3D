using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

  public static List<Enemy> enemies;
  public static List<Transform> enemyTransforms;
  public static Dictionary<long, GameObject> enemyPrefabsById;
  public static Dictionary<long, Queue<Enemy>> enemyObjectPools;

  private static bool isInitialized;

  // Start is called before the first frame update
  public static void Init() {
    if (isInitialized) {
      return;
    }

    enemyPrefabsById = new Dictionary<long, GameObject>();
    enemyObjectPools = new Dictionary<long, Queue<Enemy>>();
    enemies = new List<Enemy>();
    enemyTransforms = new List<Transform>();

    EnemySpawnData[] enemySpawnDatas = Resources.LoadAll<EnemySpawnData>("Enemies");
    Debug.Log(enemySpawnDatas[0].name);

    foreach (EnemySpawnData enemySpawnData in enemySpawnDatas) {
      enemyPrefabsById.Add(enemySpawnData.enemyId, enemySpawnData.enemyPrefab);
      enemyObjectPools.Add(enemySpawnData.enemyId, new Queue<Enemy>());
    }

    isInitialized = true;
  }

  public static Enemy SpawnEnemy(long enemyId) {
    if (!enemyPrefabsById.ContainsKey(enemyId)) {
      return null;
    }

    Enemy enemy;
    Queue<Enemy> enemyQueue = enemyObjectPools[enemyId];
    if (enemyQueue.Count > 0) {
      enemy = enemyQueue.Dequeue();
      enemy.gameObject.SetActive(true);
    } else {
      GameObject newEnemyObject = Instantiate(enemyPrefabsById[enemyId], GameManager.nodePositions[0], Quaternion.identity);
      enemy = newEnemyObject.GetComponent<Enemy>();
      enemy.id = enemyId;
    }
    enemy.Init();
    enemyTransforms.Add(enemy.transform);
    enemies.Add(enemy);
    return enemy;
  }

  public static void RemoveEnemy(Enemy enemyToRemove) {
    enemyObjectPools[enemyToRemove.id].Enqueue(enemyToRemove);
    enemyToRemove.gameObject.SetActive(false);
    enemies.Remove(enemyToRemove);
    enemyTransforms.Remove(enemyToRemove.transform);
  }

}
