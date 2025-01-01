using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectLevelModel
{
    public int level;
    public Vector3 originalCam;
    public Sprite[] numberIMG;

    public SelectLevelModel(int _level, Vector3 _originalCam, Vector3 _camMoveAreaSize,Sprite[] _numberIMG)
    {
        level = _level;
        originalCam = _originalCam;
        numberIMG = _numberIMG;
    }   
}


[System.Serializable]
public class SelectLevelState
{
    public int level;
    public int star;
    public SelectLevelModel model;
}

public class SelectLevelManager : MonoBehaviour
{
    public List<SelectLevelState> selectLevelStates = new List<SelectLevelState>();
    public SelectLevelDatabaseSO selectLevelDatabaseSO;
    public Camera mainCam;
    public SpriteRenderer moveAreaSR;
    public int currentLevel;
    public Sprite[] stars;
    
    public void Start()
    {
        SetupLevel(currentLevel);
    }
    
    public void InitSelectLevel()
    {
       SetupLevel(currentLevel);
    }

    private SelectLevelModel SelectLevel(int level)
    {
        SelectLevelModel selectLevelModel = selectLevelDatabaseSO.GetSelectLevelModel(level);
        return selectLevelModel;
    }

    public void SetupLevel(int level)
    {
         SelectLevelModel selectLevelModel = SelectLevel(currentLevel);
         mainCam.transform.position = selectLevelModel.originalCam;
    }
}
