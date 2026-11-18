using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        agent.SetDestination(target.position);
    }
    void Update()
    {
        agent.SetDestination(target.position);
    }
}