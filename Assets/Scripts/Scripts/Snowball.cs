using UnityEngine;
using System;

public class Snowball : MonoBehaviour
{
    private float damage = 55;

    public Action OnDestroyed; // ������� ��� ����������� ������

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ������ �������� � �����
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // ���������� ������, ���� ������������ �� � Player � Box
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("SnowBallReloader") && !collision.gameObject.CompareTag("SnowBall"))
        {
            DestroySnowball();
            SnowballManager.Instance.SpawnSnowball();
        }
    }

    private void DestroySnowball()
    {
        OnDestroyed?.Invoke(); // ���������� �������
        Destroy(gameObject);
    }
}
