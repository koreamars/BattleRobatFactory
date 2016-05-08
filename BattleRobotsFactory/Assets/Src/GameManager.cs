using UnityEngine;
using System.Collections;

public class GameManager
{
    private static GameManager instance;

    public static GameManager getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private ArrayList LogList = new ArrayList();

    private RobotData _robotData;

    public RobotData robotData { 
        set
        {
            _robotData = value;
        }
        get
        {
            if (_robotData == null)
            {
                _robotData = new RobotData();
                _robotData.currentBullet = 600;
                _robotData.maxHeadHP = 0;
                _robotData.currentHeadHP = _robotData.maxHeadHP;
                _robotData.maxBodyHP = 200;
                _robotData.currentBodyHP = _robotData.maxBodyHP;
                _robotData.maxLowerHP = 500;
                _robotData.currentLowerHP = _robotData.maxLowerHP;
            }
            return _robotData;
        }
    }

    public float GetRandomValue(float min, float max) {
        return Random.Range(min, max); 
    }

    public void ShowLog(string meg)
    {
        LogList.Add(meg);
        GameObject LogTxt = GameObject.Find("Log");
        LogTxt.GetComponent<Renderer>().sortingOrder = 200;
        
        if(LogList.Count > 10) {
            LogList.RemoveAt(0);
        }

        string totalMsg = "";
        foreach (string currentMsg in LogList)
        {
            totalMsg = totalMsg + "\n" + currentMsg;
        }
        LogTxt.GetComponent<TextMesh>().text = totalMsg;
    }

    public BattleSceneManager battleSceneManager;
    
}