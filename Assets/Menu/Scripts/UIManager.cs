using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject nextWaveMenu;
    public GameObject gameOverMenu;
    public Slider treeHealthSlider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowNextWaveMenu(System.Action onContinue)
    {
        nextWaveMenu.SetActive(true);
        Button continueButton = nextWaveMenu.GetComponentInChildren<Button>();
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

    public void UpdateTreeHealth(float currentHealth, float maxHealth)
    {
        if (treeHealthSlider != null)
        {
            treeHealthSlider.maxValue = maxHealth;
            treeHealthSlider.value = currentHealth;
        }
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Button restartButton = gameOverMenu.transform.Find("RestartButton").GetComponent<Button>();
        Button quitButton = gameOverMenu.transform.Find("QuitButton").GetComponent<Button>();

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => {
            gameOverMenu.SetActive(false);
            GameManager.Instance.RestartGame();
        });

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}