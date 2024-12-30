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

    public PipeSlotCanvas[,] slots;

    public GridElementDataBase gridDatabase;

    void Start()
    {
        //CreateGrid();
        //CreateLevel();
        // slots = new PipeSlotCanvas[columns, rows];
        // for (int x = 0; x < columns; x++)
        // {
        //     for (int y = 0; y < rows; y++)
        //     {
        //         slots[x,y] = GameObject.Find("Slot_" + x + "-" + y).GetComponent<PipeSlotCanvas>();
        //         if(slots[x,y] != null)
        //             Debug.Log(slots[x,y].name);
        //     }
        // }
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
                if(slots[data.datas[i].posX, data.datas[i].posY].isDefault) continue;
                if(slots[data.datas[i].posX, data.datas[i].posY].item != null)
                {
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
        string[] lines = levelData.text.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[lines.Length - y - 1];
            string[] cell = line.Split(',');
            for (int x = 0; x < cell.Length; x++)
            {
                string id = "";
                if(cell[x].StartsWith("0")) continue;
                else if(cell[x].StartsWith("40"))
                {
                    id = "40000";
                    if(cell[x] == "40005")
                    {
                        id = "40005";
                    }
                }
                else if(cell[x].StartsWith("1") || cell[x].StartsWith("2"))
                {
                    id = cell[x].Substring(0, 3) + "00";
                }
                else if(cell[x] == "30001" || cell[x] == "30002")
                {
                    id = cell[x];
                }
                else if(cell[x] == "30013" || cell[x] == "30020") continue;
                Debug.Log($"{x}-{y} : {id}");
                GridObject gridObj = gridDatabase.GetGridObjectByID(id);
                GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
                go.transform.localPosition = Vector3.zero;
                slots[x, y].item = go;
            }
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