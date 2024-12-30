using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridElementDataBase", menuName = "ScriptableObjects/GridDatabase", order = 2)]
public class GridElementDataBase : ScriptableObject
{
    public List<GridObject> gridModels;
    public List<GridObject> decorateModels;

    public GridObject GetGridObjectByID(string mID)
    {
        int index = gridModels.FindIndex((o) => o.id == mID);
        if(index != -1)
        {
            return gridModels[index];
        }
        return null;
    }

    public GridObject GetDecorateObjectByID(string mID)
    {
        int index = decorateModels.FindIndex((o) => o.id == mID);
        if(index != -1)
        {
            return decorateModels[index];
        }
        return null;
    }
}

[System.Serializable]
public class GridObject
{
    public string id;
    public GameObject prefab;
}
