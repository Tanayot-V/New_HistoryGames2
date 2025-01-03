using System.Collections;
using System.Collections.Generic;
using CityTycoon;
using UnityEngine;

[System.Serializable]
public class SelectLevelModel
{
    public int level;
    public Vector3 originalCam;
    public Sprite[] numberIMG;

    public string sceneName;
    
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
    public bool isFinished;
    public SelectLevelModel model;
    public SpriteRenderer moveAreaSR;
    public SelectLevelNumberSlot selectLevelNumberSlot;

}

public class SelectLevelManager : MonoBehaviour
{
    [SerializeField] private List<SelectLevelState> selectLevelStates = new List<SelectLevelState>();
    [SerializeField] private SelectLevelDatabaseSO selectLevelDatabaseSO;
    [SerializeField] private Camera mainCam;
    private int currentLevel;
    
    public void Start(){}
    
    public void InitSelectLevel()
    {
        currentLevel = LevelDataManager.Instance.GetCurrentLevel();
        SetupLevel(currentLevel);
        int index = 0;
        selectLevelStates.ForEach(o => {
            o.model = selectLevelDatabaseSO.selectLevelModels[index];
            o.star = LevelDataManager.Instance.GetLevelData(o.level).star;

            if(o.level < currentLevel) o.isFinished = true;
            else o.isFinished = false;

            if(o.level == currentLevel) o.selectLevelNumberSlot.SetCurrent(o);
            else if(o.isFinished) o.selectLevelNumberSlot.SetFinished(o);
            else o.selectLevelNumberSlot.SetNoClear(o); 

            index++;
        });
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
         mainCam.GetComponent<CameraSmooth>().SetMoveArea(selectLevelStates[level-1].moveAreaSR);
    }
    
}
