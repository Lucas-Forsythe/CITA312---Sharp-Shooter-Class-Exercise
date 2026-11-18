using System.Collections;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;
    float spawnDelay = KillCount.spawnDelay;
    bool gameActive = true;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        while (gameActive)
        {
            Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
            );
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    public void StopSpawning()
    {
        gameActive = false;
    }
}