using UnityEngine;
using UnityEngine.AI;

public class GuardEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent enemy;
    public LayerMask whatIsGround, whatIsPlayer;

    //States
    public float sightRange;
    public bool playerInSightRange;

    //Guard state
    private Vector3 guardPosition;
    private bool isGuarding = true;
    private bool hasReturnPosition = false;
    private Vector3 returnPosition;

    private void Awake()
    {
        player = GameObject.Find("Pacman").transform;
        enemy = GetComponent<NavMeshAgent>();
        guardPosition = transform.position;
    }

    void Update()
    {
        //Check for sight
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (playerInSightRange)
        {
            ChasePlayer();
            isGuarding = false;
            hasReturnPosition = false;
        }
        else if (!isGuarding)
        {
            // Stop moving when player is out of range
            if (!hasReturnPosition)
            {
                returnPosition = transform.position;
                hasReturnPosition = true;
                enemy.SetDestination(returnPosition);
            }

            if (hasReturnPosition && enemy.remainingDistance <= enemy.stoppingDistance)
            {
                isGuarding = true;
            }
        }
        else
        {
            if (enemy.isActiveAndEnabled && enemy.isOnNavMesh)
            {
                enemy.SetDestination(transform.position);
            }
        }
    }

    private void ChasePlayer()
    {
        if (player != null && enemy.isActiveAndEnabled && enemy.isOnNavMesh)
        {
            enemy.SetDestination(player.position);
        }
    }

    // Visual radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}