using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Weak, Normal, Strong }

public class EnemyBehaviour : MonoBehaviour
{
    public Action OnDeath;

    public EnemyType enemyType;

    private float health;
    private float damage;
    private float speed;

    private Transform treeTarget;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    private bool isNearTree = false; // Флаг нахождения рядом с ёлкой

    private Coroutine damageCoroutine;

    private float damageTimer = 0;
    private float damageInterval = 3;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        treeTarget = GameObject.FindWithTag("Tree")?.transform;
        if (treeTarget != null) agent.SetDestination(treeTarget.position);

       // Debug.Log(gameObject.GetComponent<Collider>().size);
    }

    void Update()
    {
        // Если враг находится рядом с ёлкой, наносим ей урон
        //if (isNearTree)
        //{
          //  damageTimer += Time.deltaTime;
            //if (damageTimer >= damageInterval)
            //{
                //TreeBehaviour.instance.TakeDamage(damage);
              //  damageTimer = 0f;
            //}
        //}
    }

    public void SetStats(float healthMultiplier, float damageMultiplier)
    {
        switch (enemyType)
        {
            case EnemyType.Weak:
                health = 50f * healthMultiplier;
                damage = 0.5f * damageMultiplier;
                speed = 2.75f;
                break;

            case EnemyType.Normal:
                health = 100f * healthMultiplier;
                damage = 1f * damageMultiplier;
                speed = 2f;
                break;

            case EnemyType.Strong:
                health = 150f * healthMultiplier;
                damage = 2f * damageMultiplier;
                speed = 1.5f;
                break;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            isNearTree = true;

            agent.enabled = false;
            obstacle.enabled = true;

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageTreeOverTime());
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            isNearTree = false;

            obstacle.enabled = false;
            agent.enabled = true;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }



    private IEnumerator DamageTreeOverTime()
    {
        while (isNearTree)
        {
            TreeBehaviour tree = treeTarget.GetComponent<TreeBehaviour>();
            if (tree != null)
            {
                tree.TakeDamage((int)damage);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
