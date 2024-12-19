using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using TMPro;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard instance;
    List<LeaderboardEntry> scores = new List<LeaderboardEntry>();
    public HighScoreDisplay[] highScoreDisplayArray;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scores = XMLManager.instance.LoadScores();
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        scores.OrderByDescending(score => score.score).ToList();
        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (i < scores.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(scores[i].playername, scores[i].score);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }

    public void AddNewScore(string entryName, int entryScore)
    {
        scores.Add(new LeaderboardEntry { playername = entryName, score = entryScore });
        XMLManager.instance.SaveScores(scores);
        Debug.Log(scores);
    }

}

public class LeaderboardEntry
{
    public string playername;
    public int score;
}