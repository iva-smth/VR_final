using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public Board leaderboard;

    void Awake()
    {
        instance = this;
        Debug.Log("create dir");

        if (!Directory.Exists(Application.persistentDataPath + "/Final/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Final/");
        }
    }

    public void SaveScores(List<LeaderboardEntry> scoresToSave)
    {
        Debug.Log("create file");
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Board));
        FileStream stream = new FileStream(Application.persistentDataPath + "/Final/Leaderboard.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        foreach (LeaderboardEntry entry in scoresToSave)
        {
            Debug.Log(entry.name);
        }    
        stream.Close();
    }

    public List<LeaderboardEntry> LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/Final/Leaderboard.xml"))
        if (File.Exists(Application.persistentDataPath + "/Final/Leaderboard.xml"))
        {
            Debug.Log("load file");
            XmlSerializer serializer = new XmlSerializer(typeof(Board));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Final/Leaderboard.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Board;
            stream.Close();
        }

        return leaderboard.list;
    }
}

[System.Serializable]
public class Board
{
    public List<LeaderboardEntry> list = new List<LeaderboardEntry>();
}