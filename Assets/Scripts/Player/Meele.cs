using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public interface IDamageable
{
    void Attack(float Amount);
}

public class Meele : MonoBehaviour
{
    [SerializeField] public float WindUpTime = 0.3f;
    [SerializeField] public float AttackCooldown = 1f;

    [SerializeField] public Transform AttackPoint;
    [SerializeField] public float AttackRange = 0.5f;
    public LayerMask EnemyLayer;
    [SerializeField] public float Damage = 40f;

    private Animator animator;
    private bool canAttack = true;

    private bool AttackKey;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (canAttack && ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame)))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.Play("Sword_Attack", 0, 0f);
        
        //Wait to checl for hitting enemy after windup in animation
        yield return new WaitForSeconds(WindUpTime);

        //Check for enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, AttackRange, EnemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            if(enemy.TryGetComponent(out IDamageable target))
            {
                target.Attack(Damage);
            }
        }

        //Wait for the remaining cooldown
        yield return new WaitForSeconds(AttackCooldown - WindUpTime);
        animator.Play("Sword_Idle", 0, 0f);
        canAttack = true;
    }
}
