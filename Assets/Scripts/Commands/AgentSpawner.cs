using UnityEngine;
using UnityEngine.AI;

public class AgentSpawner : MonoBehaviour
{
    public static AgentSpawner Instance { get; private set; }
    [Tooltip("Prefab musi zawiera� NavMeshAgent oraz NPCPatrol")]
    public GameObject agentPrefab;
    public Transform spawnCenter; // je�li null -> u�yje pozycji tego obiektu
    public float spawnRadius = 5f;
    public int maxAgents = 50;

    [Tooltip("Koszt spawnu jednego agenta")]
    public int spawnCost = 0;

    [Tooltip("Referencja do komponentu zarz�dzaj�cego pieni�dzmi (przypisz AddMoney)")]
    public AddMoney moneyManager;

    private int currentCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnAgent()
    {
        if (agentPrefab == null)
        {
            Debug.LogError("[AgentSpawner] Brak przypisanego prefab'a agentPrefab.");
            return;
        }

        if (currentCount >= maxAgents)
        {
            Debug.Log("[AgentSpawner] Osi�gni�to maksymaln� liczb� agent�w.");
            return;
        }

        if (moneyManager == null)
        {
            Debug.LogError("[AgentSpawner] Brak przypisanej referencji moneyManager. Przypisz AddMoney w Inspectorze.");
            return;
        }

        if (!moneyManager.TrySpend(spawnCost))
        {
            Debug.Log("[AgentSpawner] Brak wystarczaj�cych �rodk�w. Koszt: " + spawnCost);
            return;
        }

        Vector3 center = spawnCenter ? spawnCenter.position : transform.position;
        Vector3 spawnPos = RandomNavmeshLocation(center, spawnRadius);
        Instantiate(agentPrefab, spawnPos, Quaternion.identity);
        currentCount++;
    }

    private static Vector3 RandomNavmeshLocation(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
                return hit.position;
        }
        return center;
    }
}