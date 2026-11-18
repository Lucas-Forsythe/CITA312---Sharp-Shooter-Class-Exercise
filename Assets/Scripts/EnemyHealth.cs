using System.Threading;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = 3;

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        Debug.Log("Enemy HP: " + hitPoints);

        if (hitPoints <= 0)
        {
            KillCount.killCount++;
            if (KillCount.spawnDelay > 1f)
            {
                KillCount.spawnDelay = KillCount.spawnDelay - 0.5f;
            }
            Explode();
            gameObject.SetActive(false);
        }
    }
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] int explosionDamage = 1;
    [SerializeField] LayerMask enemyLayer;
    void Explode()
    {
        Instantiate(
        explosionVFX,
        transform.position,
        Quaternion.identity
        );
        Collider[] nearbyEnemies =
        Physics.OverlapSphere(
        transform.position,
        explosionRadius,
        enemyLayer
        );
        foreach (Collider nearby in nearbyEnemies)
        {
            EnemyHealth enemy =
            nearby.GetComponent<EnemyHealth>();
            if (enemy != null && enemy.gameObject != gameObject)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }
}