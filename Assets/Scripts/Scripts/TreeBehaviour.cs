using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeBehaviour : MonoBehaviour
{
    public static TreeBehaviour instance;

    public float maxHealth = 1000;
    [SerializeField] private float currentHealth;

    public Image healthBar;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateTreeHealth(currentHealth, maxHealth);
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth < maxHealth/2) {
            healthBar.color = Color.yellow;
        }
        if (currentHealth < maxHealth / 3)
        {
            healthBar.color = Color.red;
        }
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
