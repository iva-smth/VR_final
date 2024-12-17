using UnityEngine;
using System;

public class Snowball : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    public Action OnDestroyed; // Событие при уничтожении снежка

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Если снежок попадает в врага
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Уничтожаем снежок, если столкновение не с Player и Box
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Box") && !collision.gameObject.CompareTag("Snow"))
        {
            DestroySnowball();
        }
    }

    private void DestroySnowball()
    {
        OnDestroyed?.Invoke(); // Генерируем событие
        Destroy(gameObject);
    }
}
