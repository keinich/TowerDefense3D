using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  private static Queue<long> enemyIdsToSpawn;

  public bool running;

  // Start is called before the first frame update
  void Start() {
    enemyIdsToSpawn = new Queue<long>();
    EnemySpawner.Init();

    StartCoroutine(GameLoop());
    InvokeRepeating("SpawnTest", 0f, 3f);
    InvokeRepeating("RemoveTest", 0f, .5f);
  }

  void SpawnTest() {
    EnqueueEnemyidToSpawn(1);
  }

  void RemoveTest() {
     if (EnemySpawner.enemies.Count > 0) {
      EnemySpawner.RemoveEnemy(EnemySpawner.enemies[Random.Range(0,EnemySpawner.enemies.Count)]);
    }
  }

  IEnumerator GameLoop() {
    while (running) {
      if (enemyIdsToSpawn.Count > 0) {
        for (int i = 0; i < enemyIdsToSpawn.Count; i++) {
          EnemySpawner.SpawnEnemy(enemyIdsToSpawn.Dequeue());
        }
      }
      yield return null;
    }
  }

  public static void EnqueueEnemyidToSpawn(long id) {
    enemyIdsToSpawn.Enqueue(id);
  }

}
