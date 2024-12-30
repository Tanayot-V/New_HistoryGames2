using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor.Search.Providers;

public class GridCanvas : MonoBehaviour
{
    public GameObject slotPrefab;
    public int rows = 8;
    public int columns = 8;
    public float slotSize = 1f;

    public TextAsset levelData;

    private string[,] dataString;

    public PipeSlotCanvas[,] slots;

    public GridElementDataBase gridDatabase;

    public Transform decorateParent;

    void Start()
    {
        string[] lines = levelData.text.Split('\n');
        string[] cell = lines[0].Split(',');
        rows = lines.Length;
        columns = cell.Length;
        dataString = new string[columns,rows];
        for (int y = 0; y < rows; y++)
        {
            string line = lines[rows - y - 1];
            cell = line.Split(',');
            for (int x = 0; x < columns; x++)
            {
                dataString[x,y] = cell[x];
            }
        }
        CreateGrid();
        CreateLevel();
    }

    public void CreateGrid()
    {
        slots = new PipeSlotCanvas[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);
                slot.name = "Slot_" + x + "-" + y;
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.SetParent(transform);
                slotRect.anchoredPosition = new Vector2(x * slotSize, y * slotSize);
                PipeSlotCanvas pipeSlot = slot.GetComponent<PipeSlotCanvas>();
                pipeSlot.pos = new Vector2(x, y);
                slots[x, y] = pipeSlot;
            }
        }
    }

    public void ClearGrid()
    {
        if (slots.Length > 0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            slots = null;
        }
    }

    public void SaveState(string levelname)
    {
        Savadata data = new Savadata();
        data.datas = new List<SlotData>();
        for (int j = slots.GetLength(1) - 1; j >= 0; j--)
        {
            for (int i = 0; i < slots.GetLength(0); i++)
            {
                if(slots[i, j].isDefault) continue;
                if(slots[i, j]?.item != null)
                {
                    PipeObject po = slots[i, j]?.item.GetComponent<PipeObject>();
                    if(po == null) continue;
                    SlotData sd = new SlotData();
                    sd.posX = i;
                    sd.posY = j;
                    sd.pipeType = po.pipeData.pipeType;
                    sd.direction = po.pipeData.direction;
                    data.datas.Add(sd);
                }
            }
        }

        string dataString = JsonConvert.SerializeObject(data);

        Debug.Log($"Data To Save : {dataString}");
        PlayerPrefs.SetString(levelname, dataString);
    }

    public void LoadState(string levelname)
    {
        if(PlayerPrefs.HasKey(levelname))
        {
            string dataString = PlayerPrefs.GetString(levelname);
            Savadata data = JsonConvert.DeserializeObject<Savadata>(dataString);
            Debug.Log($"Load Data : {data.datas.Count}");
            for (int i = 0; i < data.datas.Count; i++)
            {
                if(slots[data.datas[i].posX, data.datas[i].posY].item != null)
                {
                    string slotItemData = this.dataString[data.datas[i].posX, data.datas[i].posY];
                    if(slotItemData.StartsWith("4") || slotItemData.StartsWith("1") || slotItemData.StartsWith("2")) continue;
                    Destroy(slots[data.datas[i].posX, data.datas[i].posY].item);
                }
                PipeModel pm = GameManager.Instance.pipeManager.GetPipeModel(data.datas[i].pipeType);
                Debug.Log($"Load {data.datas[i].pipeType} Dir {data.datas[i].direction} At {data.datas[i].posX}:{data.datas[i].posY}");
                PipeObject po = Instantiate(pm.prefab,slots[data.datas[i].posX, data.datas[i].posY].transform).GetComponent<PipeObject>();
                po.transform.localPosition = Vector3.zero;
                slots[data.datas[i].posX, data.datas[i].posY].item = po.gameObject;
                po.pipeData.direction = data.datas[i].direction;
                po.transform.rotation = Quaternion.Euler(0, 0, GameManager.Instance.pipeManager.GetRotationZ(data.datas[i].direction));
            }
        }
    }

    public void CreateLevel()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                string id = "";
                if(dataString[x,y].StartsWith("0")) continue;
                else if(dataString[x,y].StartsWith("40"))
                {
                    id = "40000";
                }
                else if(dataString[x,y].StartsWith("1") || dataString[x,y].StartsWith("2"))
                {
                    id = dataString[x,y].Substring(0, 3) + "00";
                }
                else if(dataString[x,y] == "30001" || dataString[x,y] == "30002")
                {
                    id = dataString[x,y];
                }
                else if(dataString[x,y] == "30013" || dataString[x,y] == "30020") continue;
                Debug.Log($"{x}-{y} : {dataString[x,y]}");
                GridObject gridObj = gridDatabase.GetGridObjectByID(id);
                GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
                go.transform.localPosition = Vector3.zero;
                slots[x, y].item = go;
                CreateDecorate(x,y);
            }
        }
    }

    public void CreateDecorate(int _x, int _y)
    {
        if(dataString[_x,_y] == "00") return;
        string id = dataString[_x,_y].Substring(3, 2);
        if(id == "01") return;
        string id_x = "00";
        string id_y = "00";
        if(_x >= 1)
        {
            if(!dataString[_x-1,_y].StartsWith("0"))
            {
                id_x = dataString[_x-1,_y].Substring(3, 2);
            }
        }
        if(_y >= 1)
        {
            if(!dataString[_x,_y-1].StartsWith("0"))
            {
                id_y= dataString[_x,_y-1].Substring(3, 2);
            }
        }
        if((id != id_x && id != id_y) || id == "05" || id == "14")
        {
            GridObject gridObj = gridDatabase.GetDecorateObjectByID(id);
            if(gridObj == null) return;
            GameObject go = Instantiate(gridObj.prefab, slots[_x, _y].transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.SetParent(decorateParent);
        }
    }
}

[System.Serializable]
public class Savadata
{
    public List<SlotData> datas;
}

[System.Serializable]
public class SlotData
{
    public int posX;
    public int posY;
    public PipeType pipeType;
    public Direction direction;
}