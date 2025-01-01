using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectLevelDatabaseSO", menuName = "ScriptableObjects/SelectLevelDatabaseSO", order = 1)]
public class SelectLevelDatabaseSO : ScriptableObject
{
    public List<SelectLevelModel> selectLevelModels = new List<SelectLevelModel>(); 

    public SelectLevelModel GetSelectLevelModel(int level)
    {
        return selectLevelModels.Find(o => o.level == level);
    }
}
