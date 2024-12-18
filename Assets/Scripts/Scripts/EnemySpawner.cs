using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ������� ������ (Weak, Normal, Strong)
    public float spawnInterval = 5f; // �������� ������ ������

    private float timeSinceLastSpawn;
    public List<Transform> spawnPoints = new List<Transform>(); // ������ ����� ��� ������

    private int enemiesPerSpawn = 3; // ���������� ������ �� ���
    private int totalEnemiesInWave = 9; // ����� ������ � ����� �����
    public float waveDelay = 1f; // �������� ����� ���������� ���� ����� �����
    private float healthMultiplier = 1f; // ��������� �������� ������
    private float damageMultiplier = 1f; // ��������� ����� ������
    private int lastWaveNum = 0;
    public List<Transform> spawnerList = new List<Transform>();

    private void Start()
    {
        //GenerateSpawnPoints();
        StartCoroutine(WaveController());
    }

    private void FindSpawners(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // �������� ���� ��������� �������
            if (child.CompareTag("Spawner"))
            {
                spawnerList.Add(child);
            }

            // ����������� ����� ��� ������ � �������� ��������
            if (child.childCount > 0)
            {
                FindSpawners(child);
            }
        }
    }
    /*
    private void GenerateSpawnPoints()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("������ ������ ����� Collider!");
            return;
        }

        Bounds bounds = collider.bounds; // ������� �������
        Debug.Log(collider.bounds);
        FindSpawners(transform);

        float step = 0.5f; // ��� ����� �������, ��������� ��� ������� �����������
        int count = 0;
        for (float x = bounds.min.x; x <= bounds.max.x; x += step)
        {
            for (float z = bounds.min.z; z <= bounds.max.z; z += step)
            {
                Vector3 rayOrigin = new Vector3(x, bounds.max.y + 1f, z); // ����� ���� ��� ��������

                // ��������� ����������� ������� ����� Raycast
                if (Physics.Raycast(rayOrigin, Vector3.up, out RaycastHit upHit, Mathf.Infinity))
                {
                    Debug.Log("1");
                    if (upHit.collider.gameObject.tag == "Spawner")
                    {
                        Debug.Log("spawner");
                        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit downHit, Mathf.Infinity))
                        {
                            // ���������, ����������� �� ����� NavMesh
                            NavMeshHit navMeshHit;
                            if (NavMesh.SamplePosition(downHit.point, out navMeshHit, 1f, NavMesh.AllAreas))
                            {
                                Debug.Log(downHit.point);
                                spawnPoints.Add(navMeshHit.position);
                            }
                        }
                    }
                }
            }
        }

        Debug.Log($"������������� ����� ������: {count}");
        Debug.Log($"������������� ����� ������: {spawnPoints.Count}");
    }*/


    private IEnumerator WaveController()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(waveDelay);
            yield return StartCoroutine(WaitForPlayerDecision());
            healthMultiplier *= 1.1f;
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
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < count; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("������������ ����� ��� ������ ������!");
                break;
            }

            // �������� ��������� ����� ������ �� ���������
            //Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 spawnPosition;
            int randomIndex = Random.Range(0, spawnPoints.Count);
            spawnPosition = availableSpawnPoints[randomIndex].position;


            availableSpawnPoints.RemoveAt(randomIndex);

            int enemyIndex = GetEnemyTypeBasedOnProbability();
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);

            // ��������� ����� �� �������� ��� ������
            float heightOffset = GetEnemyHeight(enemy) / 2f;
            enemy.transform.position += Vector3.up * heightOffset;

            EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour != null)
            {
                enemyBehaviour.SetStats(healthMultiplier, damageMultiplier);
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
