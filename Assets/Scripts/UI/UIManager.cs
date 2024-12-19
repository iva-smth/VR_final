using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject nextWaveMenu;
    public GameObject gameOverMenu;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowNextWaveMenu(System.Action onContinue)
    {
        nextWaveMenu.SetActive(true);
        Button restartButton = nextWaveMenu.transform.Find("Restart").GetComponent<Button>();
        Button quitButton = nextWaveMenu.transform.Find("Quit").GetComponent<Button>();
        Button continueButton = nextWaveMenu.transform.Find("Continue").GetComponent<Button>();

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => {
            nextWaveMenu.SetActive(false);
            GameManager.Instance.RestartGame();
        });

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => {
            Leaderboard.instance.AddNewScore(Name.instance.playerName, EnemyManager.Instance.deadCount);
            SceneManager.LoadScene(0);
        });

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            nextWaveMenu.SetActive(false);
            onContinue?.Invoke();
        });
    }

    public void HideNextWaveMenu()
    {
        nextWaveMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Button restartButton = gameOverMenu.transform.Find("Restart").GetComponent<Button>();
        Button quitButton = gameOverMenu.transform.Find("Quit").GetComponent<Button>();

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => {
            gameOverMenu.SetActive(false);
            GameManager.Instance.RestartGame();
        });

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => {
        Leaderboard.instance.AddNewScore(Name.instance.playerName, EnemyManager.Instance.deadCount);
        Debug.Log(Name.instance.playerName);
        Debug.Log(EnemyManager.Instance.deadCount);
            SceneManager.LoadScene(0);
        });
    }
}