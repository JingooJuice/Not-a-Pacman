using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyObject;
        public Vector3 startPosition;
        public Quaternion startRotation;
        public NavMeshAgent navMeshAgent;
    }

    public List<EnemyData> enemies = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeEnemies();
    }

    public void InitializeEnemies()
    {
        enemies.Clear();

        GameObject[] enemyObjects = FindGameObjectsByLayer(LayerMask.NameToLayer("Enemies"));

        foreach (GameObject enemy in enemyObjects)
        {
            EnemyData data = new()
            {
                enemyObject = enemy,
                startPosition = enemy.transform.position,
                startRotation = enemy.transform.rotation,
                navMeshAgent = enemy.GetComponent<NavMeshAgent>()
            };

            enemies.Add(data);
            Debug.Log($"Enemy registered: {enemy.name} at position {data.startPosition}");
        }
    }

    public void RespawnAllEnemies()
    {
        foreach (EnemyData enemyData in enemies)
        {
            if (enemyData.enemyObject != null)
            {
                RespawnEnemy(enemyData);
            }
        }
        Debug.Log("All enemies respawned");
    }

    public void RespawnEnemy(EnemyData enemyData)
    {
        GameObject enemy = enemyData.enemyObject;

        if (enemyData.navMeshAgent != null)
        {
            enemyData.navMeshAgent.enabled = false;
        }

        enemy.transform.SetPositionAndRotation(enemyData.startPosition, enemyData.startRotation);
        enemy.SetActive(true);

        if (enemyData.navMeshAgent != null)
        {
            enemyData.navMeshAgent.enabled = true;
            enemyData.navMeshAgent.ResetPath();
        }

        Debug.Log($"Enemy respawned: {enemy.name}");
    }

    public void RespawnEnemy(GameObject enemyObject)
    {
        EnemyData enemyData = enemies.Find(data => data.enemyObject == enemyObject);
        if (enemyData != null)
        {
            RespawnEnemy(enemyData);
        }
        else
        {
            Debug.LogWarning($"Enemy {enemyObject.name} not found in EnemyManager");
        }
    }

    private GameObject[] FindGameObjectsByLayer(int layer)
    {
        var gameObjects = new List<GameObject>();
        var allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var go in allGameObjects)
        {
            if (go.layer == layer)
            {
                gameObjects.Add(go);
            }
        }

        return gameObjects.ToArray();
    }

    public void AddEnemy(GameObject enemy)
    {
        EnemyData data = new()
        {
            enemyObject = enemy,
            startPosition = enemy.transform.position,
            startRotation = enemy.transform.rotation,
            navMeshAgent = enemy.GetComponent<NavMeshAgent>()
        };

        enemies.Add(data);
        Debug.Log($"Enemy manually added: {enemy.name}");
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }
}