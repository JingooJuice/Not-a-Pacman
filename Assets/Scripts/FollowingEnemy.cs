using UnityEngine;
using UnityEngine.AI;

public class FollowingEnemy : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent enemy;
    
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        enemy.destination = player.position;
    }
}
