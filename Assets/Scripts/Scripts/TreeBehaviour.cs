using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public static TreeBehaviour instance;

    public float maxHealth = 500;
    private float currentHealth;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateTreeHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UIManager.Instance.UpdateTreeHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Ёлка уничтожена! Игра окончена.");
            UIManager.Instance.ShowGameOverMenu();
        }
    }
}
