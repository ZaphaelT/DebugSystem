using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public float roamRadius = 10f;
    public float arriveThreshold = 0.5f;
    public float waitAtPoint = 1f;

    private NavMeshAgent agent;
    private Coroutine patrolCoroutine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError($"[{nameof(NPCPatrol)}] Brak komponentu NavMeshAgent na prefabie.");
    }

    void OnEnable() => StartPatrol();
    void OnDisable() => StopPatrol();

    public void StartPatrol()
    {
        if (agent == null) return;
        if (patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(Patrol());
    }

    public void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            Vector3 next = RandomNavmeshLocation(transform.position, roamRadius);
            agent.SetDestination(next);

            // Czekaj a¿ agent dotrze do celu
            while (agent.pathPending || agent.remainingDistance > arriveThreshold)
                yield return null;

            yield return new WaitForSeconds(waitAtPoint);
        }
    }

    public static Vector3 RandomNavmeshLocation(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
                return hit.position;
        }
        // jeœli nic nie znajdzie, zwróæ center (bezpieczny fallback)
        return center;
    }
}
