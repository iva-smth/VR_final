using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ������� ������ (Weak, Normal, Strong)
    public float spawnInterval = 5f; // �������� ������ ������

    private float timeSinceLastSpawn;
    private List<Vector3> spawnPoints = new List<Vector3>(); // ������ ����� ��� ������

    private int enemiesPerSpawn = 3; // ���������� ������ �� ���
    private int totalEnemiesInWave = 9; // ����� ������ � ����� �����
    public float waveDelay = 1f; // �������� ����� ���������� ���� ����� �����
    private float healthMultiplier = 1f; // ��������� �������� ������
    private float damageMultiplier = 1f; // ��������� ����� ������
    private int lastWaveNum = 0;

    private void Start()
    {
        GenerateSpawnPoints();
        StartCoroutine(WaveController());
    }

    private void GenerateSpawnPoints()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("������ ������ ����� Collider!");
            return;
        }

        Bounds bounds = collider.bounds; // ������� �������
        float step = 0.5f; // ��� ����� �������, ��������� ��� ������� �����������

        for (float x = bounds.min.x; x <= bounds.max.x; x += step)
        {
            for (float z = bounds.min.z; z <= bounds.max.z; z += step)
            {
                Vector3 rayOrigin = new Vector3(x, bounds.max.y + 1f, z); // ����� ���� ��� ��������

                // ��������� ����������� ������� ����� Raycast
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == this.gameObject) // ��������, ��� ����� ����������� �������� �������
                    {
                        // ���������, ����������� �� ����� NavMesh
                        NavMeshHit navMeshHit;
                        if (NavMesh.SamplePosition(hit.point, out navMeshHit, 1f, NavMesh.AllAreas))
                        {
                            spawnPoints.Add(navMeshHit.position);
                        }
                    }
                }
            }
        }

        Debug.Log($"������������� ����� ������: {spawnPoints.Count}");
    }

    private IEnumerator WaveController()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(waveDelay);
            yield return StartCoroutine(WaitForPlayerDecision());
            healthMultiplier *= 1.05f;
            damageMultiplier *= 1.05f;
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < totalEnemiesInWave; i += enemiesPerSpawn)
        {
            SpawnEnemies(enemiesPerSpawn);
            yield return new WaitForSeconds(spawnInterval);
        }

        while (EnemyManager.Instance.EnemiesAlive > 0)
        {
            yield return null; // ��� ����������� ���� ������
        }
    }

    private void SpawnEnemies(int count)
    {
        List<Vector3> availableSpawnPoints = new List<Vector3>(spawnPoints);

        for (int i = 0; i < count; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("������������ ����� ��� ������ ������!");
                break;
            }

            // �������� ��������� ����� ������ �� ���������
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            spawnPosition = availableSpawnPoints[randomIndex];

            availableSpawnPoints.RemoveAt(randomIndex);

            int enemyIndex = GetEnemyTypeBasedOnProbability();
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);

            // ��������� ����� �� �������� ��� ������
            float heightOffset = GetEnemyHeight(enemy) / 2f;
            enemy.transform.position += Vector3.up * heightOffset;


            if (enemy.TryGetComponent<EnemyBehaviour>(out var behaviour))
            {
                behaviour.SetStats(healthMultiplier, damageMultiplier);
            }

            EnemyManager.Instance.RegisterEnemy(enemy);
        }
    }

    private float GetEnemyHeight(GameObject enemy)
    {
        // �������� �������� ������ ����� Collider
        Collider col = enemy.GetComponent<Collider>();
        if (col != null)
        {
            return col.bounds.size.y;
        }

        // ���� Collider �����������, ������� ����� Renderer
        Renderer rend = enemy.GetComponent<Renderer>();
        if (rend != null)
        {
            return rend.bounds.size.y;
        }

        // ���� ������ �� �������, ���������� �������� �� ���������
        Debug.LogWarning("�� ������� ���������� ������ �����. ������������ �������� �� ���������: 2.");
        return 2f; // �������� �� ���������
    }

    private int GetEnemyTypeBasedOnProbability()
    {
        float randomValue = Random.value;
        if (randomValue < 0.5f) return 0; // Weak
        if (randomValue < 0.85f) return 1; // Normal
        return 2; // Strong
    }

    private IEnumerator WaitForPlayerDecision()
    {
        bool playerReady = false;
        UIManager.Instance.ShowNextWaveMenu(() => playerReady = true);

        while (!playerReady)
        {
            yield return null;
        }

        UIManager.Instance.HideNextWaveMenu();
    }
}
