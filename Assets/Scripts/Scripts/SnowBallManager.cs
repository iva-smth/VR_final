using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : MonoBehaviour
{
    public static SnowballManager Instance;

    public GameObject snowballPrefab;
    public Transform spawnPoint; // ����� ��������� �������
    public int maxSnowballs = 10; // ������������ ���������� ������� �� �����

    private List<GameObject> snowballs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // ��������� ������, ���� �� ������ 10
        while (snowballs.Count < maxSnowballs)
        {
            SpawnSnowball();
        }
    }

    private void SpawnSnowball()
    {
        GameObject newSnowball = Instantiate(snowballPrefab, spawnPoint.position, Quaternion.identity);
        snowballs.Add(newSnowball);

        // ����������� ������ �� ������� ��������
        Snowball snowballScript = newSnowball.GetComponent<Snowball>();
        if (snowballScript != null)
        {
            snowballScript.OnDestroyed += () => snowballs.Remove(newSnowball);
        }
    }
}
