using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : MonoBehaviour
{
    public static SnowballManager Instance;

    public GameObject snowballPrefab;
    public Transform spawnPoint; // Место появления снежков
    public int maxSnowballs = 10; // Максимальное количество снежков на сцене

    private List<GameObject> snowballs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnSnowball()
    {
        GameObject newSnowball = Instantiate(snowballPrefab, spawnPoint.position, Quaternion.identity);
        snowballs.Add(newSnowball);

        // Подписываем снежок на событие удаления
        Snowball snowballScript = newSnowball.GetComponent<Snowball>();
        if (snowballScript != null)
        {
            snowballScript.OnDestroyed += () => snowballs.Remove(newSnowball);
        }
    }
}
