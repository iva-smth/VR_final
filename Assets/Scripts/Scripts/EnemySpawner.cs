using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ������� ������ (Weak, Normal, Strong)
    private float spawnInterval = 10f; // �������� ������ ������

    private float timeSinceLastSpawn;
    public List<Transform> spawnPoints = new List<Transform>(); // ������ ����� ��� ������

    private int enemiesPerSpawn = 1; // ���������� ������ �� ���
    private int totalEnemiesInWave = 1; // ����� ������ � ����� �����
    private float waveDelay = 1f; // �������� ����� ���������� ���� ����� �����
    private float healthMultiplier = 1f; // ��������� �������� ������
    private float damageMultiplier = 1f; // ��������� ����� ������
    private int lastWaveNum = 0;
    public List<Transform> spawnerList = new List<Transform>();
    public TMP_Text timerText;

    private void Start()
    {
        StartCoroutine(WaveController());
    }
    
    private IEnumerator WaveController()
    {
        while (true)
        {
            for (int countdown = 3; countdown > 0; countdown--)
            {
                timerText.text = $"�� ������ �����: {countdown}...";
                yield return new WaitForSeconds(1f);
            }
            timerText.text = ""; 
            TreeBehaviour.instance.resetTree();
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
            Vector3 spawnPosition;
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
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
