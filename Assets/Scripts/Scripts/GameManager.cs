using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartGame()
    {
        Leaderboard.instance.AddNewScore(Name.instance.playerName, EnemyManager.Instance.deadCount);

        SceneManager.LoadScene(1);
    }
}
