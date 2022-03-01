using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Create EnemySpawnData")]
public class EnemySpawnData : ScriptableObject {
  public GameObject enemyPrefab;
  public long enemyId;
}
