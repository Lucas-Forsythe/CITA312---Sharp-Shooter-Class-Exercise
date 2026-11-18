using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] int enemiesToWin = 10;
    [SerializeField] GameObject winPanel;
    [SerializeField] EnemySpawner spawner;
    public void Update()
    {
        if (KillCount.killCount >= enemiesToWin)
        {
            spawner.StopSpawning();
            winPanel.SetActive(true);
        }
    }
}