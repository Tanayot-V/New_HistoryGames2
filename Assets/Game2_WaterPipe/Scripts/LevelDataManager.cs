using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public int star;

    public int score;

    public LevelData(int level, int star, int score)
    {
        this.level = level;
        this.star = star;
        this.score = score;
    }
}

public class LevelDataManager : Singletons<LevelDataManager>
{
   [SerializeField] private int currentLevel;
   [SerializeField] private List<LevelData> levelDatas = new List<LevelData>();
   [SerializeField] private bool isAllReadDiary;

    public void Start()
    {
        if (levelDatas.Count == 0)
        {
            SetupDefault();
        }    
    }
    
    //พี่เอิร์ธ ใช้อันนี้เพื่อเช็คว่าอ่านหมดยัง T = ขึ้นจุดแดง F = ไม่ขึ้นจุดแดง
    public bool IsAllReadDiary()
    {
        return isAllReadDiary;
    }

    public void SetIsAllReadDiary(bool _isAllReadDiary)
    {
        isAllReadDiary = _isAllReadDiary;
    }
    
    public void SetupDefault()
    {
        currentLevel = 1;
        levelDatas.Clear();
        for(int i = 1; i < 10; i++)
        {
            levelDatas.Add(new LevelData(i, 1, 0));
        }
    }

    public int GetCurrentLevel()
    {
        if(currentLevel <= 0) currentLevel = 1;
        if(currentLevel >= 10) currentLevel = 9;
        return currentLevel;
    }

    //พี่เอิร์ธ อันนี้เอาไว้เซ็คว่าเราเล่นถึงไหนแล้ว
    public void SetCurrentLevel(int _level)
    {
        if(currentLevel <= 0) currentLevel = 1;
        currentLevel = _level;
    }

    private Dictionary<int, LevelData> levelDataDIC = new Dictionary<int, LevelData>();
    public LevelData GetLevelData(int _level)
    {
        if (levelDataDIC.ContainsKey(_level))
        {
            return levelDataDIC[_level];
        }
        else
        {
            return levelDataDIC[_level] = levelDatas.Find(o => o.level == _level);
        }
    }

    public void SetLevelData(int _level, int _star, int _score)
    {
        LevelData levelData = GetLevelData(_level);
        levelData.star = _star;
        levelData.score = _score;
    }

    //พี่เอิร์ธ ขอดาวหน่อย
    public void SetStar(int _level, int _star)
    {
        LevelData levelData = GetLevelData(_level);
        if(_star <= 0) _star = 1;
        levelData.star = _star;
    }

    public void SetScore(int _level, int _score)
    {
        LevelData levelData = GetLevelData(_level);
        levelData.score = _score;
    }
}
