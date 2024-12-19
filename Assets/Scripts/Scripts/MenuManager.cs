using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject LeaderBoard;
    [SerializeField] GameObject Menu;

    [SerializeField] GameObject keyboard;

    private void Start()
    {
        Leaderboard.instance.UpdateDisplay();
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    
    public void LoadGame()
    {
        keyboard.SetActive(true);  
        Menu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLeaderboard()
    {
        LeaderBoard.SetActive(true);
        Menu.SetActive(false);
    }

    public void ExitLeaderboard()
    {
        LeaderBoard.SetActive(false);
        Menu.SetActive(true);
    }
}
