using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreManager = this;
    }

    public void UpdateScoreText(int score, int totalScore, int frameCount, int throwsCount)
    {
        scoreText.text = frameCount + "-� �����" + "," + throwsCount + "-� ������" + "\n" +
                            "���� - " + score + " / 10; " + "����� ���� - " + totalScore;
    }
}
