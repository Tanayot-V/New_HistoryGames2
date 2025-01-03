using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UIElements.Experimental;

public class GridCanvas : MonoBehaviour
{
    public bool isChallangeMode = false;
    public GameObject slotPrefab;
    public int rows = 8;
    public int columns = 8;
    public float slotSize = 1f;

    public TextAsset levelData;

    private string[,] dataString;

    public PipeSlotCanvas[,] slots;

    public GridElementDataBase gridDatabase;

    public Transform decorateParent;

    public Transform roadParemt;

    public string savelevelName;
    public string loadlevelName;

    void Start()
    {
        if(isChallangeMode)
        {
            rows = 8;
            columns = 8;
            CreateGrid();
            RandomPipe();
        }

        ReadMapData();
        // CreateGrid();
        // CreateLevel();

        slots = new PipeSlotCanvas[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject slot = GameObject.Find("Slot_" + x + "-" + y);
                PipeSlotCanvas pipeSlot = slot.GetComponent<PipeSlotCanvas>();
                pipeSlot.pos = new Vector2(x, y);
                slots[x, y] = pipeSlot;
            }
        }
        
        if(!string.IsNullOrEmpty(loadlevelName))
        {
            LoadState(loadlevelName);
        }
    }

    public void ReadMapData()
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
                dataString[x, y] = cell[x].Trim();
            }
        }
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
        slots = null;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        childCount = decorateParent.childCount;
        for(int i = 0; i < childCount; i++)
        {
            DestroyImmediate(decorateParent.GetChild(0).gameObject);
        }
        childCount = roadParemt.childCount;
        for(int i = 0; i < childCount; i++)
        {
            DestroyImmediate(roadParemt.GetChild(0).gameObject);
        }
    }

    public void RandomPipe()
    {
        int startCount = Random.Range(1,4);
        int endCount = Random.Range(3,10);
        int obstacleCount = Random.Range(8,14);

        int counter = 0;
        while (counter < startCount)
        {
            int x = Random.Range(1,columns-1);
            int y = Random.Range(1,rows-1);
            if(slots[x, y].item != null) continue;

            string id = "1";
            int direction = Random.Range(1,16);
            id += direction.ToString("00") + "00";
            Debug.Log(id);
            GridObject gridObj = gridDatabase.GetGridObjectByID(id);
            GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
            go.transform.localPosition = Vector3.zero;
            slots[x, y].item = go;
            slots[x,y].isDefault = true;
            PipeStart ps = go.GetComponent<PipeStart>();
            if(ps != null)
            {
                GameManager.Instance.pipeStarts.Add(ps);
            }
            else
            {
                DestroyImmediate(go);
                continue;
            }
            counter++;
        }
        counter = 0;
        while (counter < endCount)
        {
            int x = Random.Range(1,columns-1);
            int y = Random.Range(1,rows-1);
            if(slots[x, y].item != null) continue;

            string id = "2";
            int direction = Random.Range(1,5);
            id += direction.ToString("00") + "00";

            GridObject gridObj = gridDatabase.GetGridObjectByID(id);
            GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
            go.transform.localPosition = Vector3.zero;
            slots[x, y].item = go;
            slots[x,y].isDefault = true;
            PipeEnd pipeEnd = go.GetComponent<PipeEnd>();
            if(pipeEnd != null)
            {
                GameManager.Instance.pipeEnds.Add(pipeEnd);
            }
            else
            {
                DestroyImmediate(go);
                continue;
            }
            gridObj = gridDatabase.GetDecorateObjectByID("14");
            if(gridObj == null) continue;
            go = Instantiate(gridObj.prefab, slots[x, y].transform);
            go.GetComponent<PipeObject>().pipeData.pos = new Vector2(x, y);
            go.transform.localPosition = Vector3.zero;
            go.transform.SetParent(decorateParent);
            
            Decoration decoration = go.GetComponent<Decoration>();
            if(decoration != null)
            {
                decoration.SuccessPipeEnds = new List<PipeEnd>();
            }
            decoration.SuccessPipeEnds.Add(slots[x, y].item.GetComponent<PipeEnd>());
            counter++;
        }
        counter = 0;
        while (counter < obstacleCount)
        {
            // 30001,30002
            int x = Random.Range(0,columns);
            int y = Random.Range(0,rows);
            if(slots[x, y].item != null) continue;

            string id = "3000" + Random.Range(1,3);
            GridObject gridObj = gridDatabase.GetGridObjectByID(id);
            GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
            go.transform.localPosition = Vector3.zero;
            slots[x, y].item = go;
            slots[x,y].isDefault = true;
            counter++;
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

        //Debug.Log($"Data To Save : {dataString}");
        PlayerPrefs.SetString(levelname, dataString);
        PlayerPrefs.Save();
    }

    public void LoadState(string levelname)
    {
        if(PlayerPrefs.HasKey(levelname))
        {
            string dataString = PlayerPrefs.GetString(levelname);
            Savadata data = JsonConvert.DeserializeObject<Savadata>(dataString);
            //Debug.Log($"Load Data : {data.datas.Count}");
            for (int i = 0; i < data.datas.Count; i++)
            {
                if(slots[data.datas[i].posX, data.datas[i].posY].item != null)
                {
                    //Debug.Log($"detect default item at {data.datas[i].posX} : {data.datas[i].posY} = slotItemData");
                    string slotItemData = this.dataString[data.datas[i].posX, data.datas[i].posY];
                    if(slotItemData.StartsWith("4") || slotItemData.StartsWith("1") || slotItemData.StartsWith("2"))
                    {
                        continue;
                    }
                    else
                    {
                        DestroyImmediate(slots[data.datas[i].posX, data.datas[i].posY].item);
                    }
                }
                PipeModel pm = GameManager.Instance.pipeManager.GetPipeModel(data.datas[i].pipeType);
                //Debug.Log($"Load {data.datas[i].pipeType} Dir {data.datas[i].direction} At {data.datas[i].posX}:{data.datas[i].posY}");
                PipeObject po = Instantiate(pm.prefab,slots[data.datas[i].posX, data.datas[i].posY].transform).GetComponent<PipeObject>();
                po.GetComponent<DraggableItem>().SnapToSlot();
                po.transform.localPosition = Vector3.zero;
                slots[data.datas[i].posX, data.datas[i].posY].item = po.gameObject;
                slots[data.datas[i].posX, data.datas[i].posY].isDefault = false;
                po.pipeData.direction = data.datas[i].direction;
                po.transform.rotation = Quaternion.Euler(0, 0, GameManager.Instance.pipeManager.GetRotationZ(data.datas[i].direction));
            }
        }
    }

    public void CreateLevel()
    {
        ReadMapData();
        ClearGrid();
        CreateGrid();
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                string id = "";
                if(dataString[x,y].StartsWith("0")) continue;
                else if(dataString[x,y].StartsWith("4"))
                {
                    id = "40000";
                }
                else if(dataString[x,y].StartsWith("1") || dataString[x,y].StartsWith("2"))
                {
                    id = dataString[x,y].Substring(0, 3) + "00";
                }
                else if(dataString[x,y] == "30001" || dataString[x,y] == "30002" || dataString[x,y] == "30020")
                {
                    id = dataString[x,y].Substring(0, 5);
                }
                else if(dataString[x,y] == "30013")
                {
                    CreateRoad(x,y);
                    continue;
                }
                //Debug.Log($"{x}-{y} : {dataString[x,y]} / {id}");
                GridObject gridObj = gridDatabase.GetGridObjectByID(id);
                GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
                go.transform.localPosition = Vector3.zero;
                slots[x, y].item = go;
                slots[x,y].isDefault = true;
                if(dataString[x,y].StartsWith("1"))
                {
                    GameManager.Instance.pipeStarts.Add(go.GetComponent<PipeStart>());
                }
                if(dataString[x,y].StartsWith("2"))
                {
                    PipeEnd pipeEnd = go.GetComponent<PipeEnd>();
                    if(pipeEnd != null)
                    {
                        GameManager.Instance.pipeEnds.Add(pipeEnd);
                    }
                }
            }
        }
        LinkWastePipe();
        CreateDecorate();
    }

    public void LinkWastePipe()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if(dataString[x,y].StartsWith("2"))
                {
                    PipeStart pipeStart = slots[x,y].item.GetComponent<PipeStart>();
                    if(pipeStart != null)
                    {
                        pipeStart.pipeEnds = new List<PipeEnd>();
                        if(dataString[x,y].Substring(3, 2) == "14")
                        {
                            //Debug.Log($"LinkWastePipe {x}-{y} : {dataString[x,y]} / 14");
                            PipeEnd pipeEnd = slots[x, y].item.GetComponent<PipeEnd>();
                            if(pipeEnd != null)
                            {
                                pipeStart.pipeEnds.Add(pipeEnd);
                            }
                        }
                        if(dataString[x,y].Length > 5)
                        {
                            string decoreLinkPos = dataString[x,y].Split('-')[1];
                            string[] linkPos = decoreLinkPos.Split('&');
                            for(int i = 0; i < linkPos.Length; i++)
                            {
                                int decoreLinkPosX = int.Parse(linkPos[i].Split(':')[0]);
                                int decoreLinkPosY = int.Parse(linkPos[i].Split(':')[1]);
                                //Debug.Log($"LinkWastePipe {x}-{y} : {dataString[x,y]} / {decoreLinkPosX}, {decoreLinkPosY}");
                                PipeEnd pipeEnd = slots[decoreLinkPosX, decoreLinkPosY].item.GetComponent<PipeEnd>();
                                if(pipeEnd != null)
                                {
                                    pipeStart.pipeEnds.Add(pipeEnd);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void CreateDecorate()
    {
        Debug.Log("Create Decorate");
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if(dataString[x,y].StartsWith("0")) continue;
                string id = dataString[x,y].Substring(3, 2);
                Debug.Log($"Decorate {x}-{y} : {dataString[x,y]} / {id}");
                if(id == "01") continue;
                string id_x = "00";
                string id_y = "00";
                if(x >= 1)
                {
                    if(!dataString[x-1,y].StartsWith("0"))
                    {
                        id_x = dataString[x-1,y].Substring(3, 2);
                    }
                }
                if(y >= 1)
                {
                    if(!dataString[x,y-1].StartsWith("0"))
                    {
                        id_y= dataString[x,y-1].Substring(3, 2);
                    }
                }
                if((id != id_x && id != id_y) || id == "05" || id == "14" || id == "11")
                {
                    GridObject gridObj = gridDatabase.GetDecorateObjectByID(id);
                    if(gridObj == null) continue;
                    GameObject go = Instantiate(gridObj.prefab, slots[x, y].transform);
                    go.GetComponent<PipeObject>().pipeData.pos = new Vector2(x, y);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.SetParent(decorateParent);
                    
                    Decoration decoration = go.GetComponent<Decoration>();
                    if(decoration != null)
                    {
                        decoration.SuccessPipeEnds = new List<PipeEnd>();
                    }
                    if(dataString[x,y].Length > 5)
                    {
                        string decoreLinkPos = dataString[x,y].Split('-')[1];
                        string[] linkPos = decoreLinkPos.Split('&');
                        for(int i = 0; i < linkPos.Length; i++)
                        {
                            int decoreLinkPosX = int.Parse(linkPos[i].Split(':')[0]);
                            int decoreLinkPosY = int.Parse(linkPos[i].Split(':')[1]);
                            //Debug.Log($"LinkWastePipe {x}-{y} : {dataString[x,y]} / {decoreLinkPosX}, {decoreLinkPosY}");
                            PipeEnd pipeEnd = slots[decoreLinkPosX, decoreLinkPosY].item.GetComponent<PipeEnd>();
                            if(pipeEnd != null)
                            {
                                decoration.SuccessPipeEnds.Add(pipeEnd);
                            }
                        }
                    }
                    if(id == "14")
                    {
                        decoration.SuccessPipeEnds.Add(slots[x, y].item.GetComponent<PipeEnd>());
                    }
                }
            }
        }
    }

    public void CreateRoad(int _x, int _y)
    {
        GridObject gridObj = gridDatabase.GetGridObjectByID("30013");
        GameObject go = Instantiate(gridObj.prefab, slots[_x, _y].transform);
        go.GetComponent<Obstacle>().slot = slots[_x, _y];
        go.GetComponent<DraggableItem>().SnapToSlot();
        go.transform.localPosition = Vector3.zero;
        slots[_x, _y].isDefaultRoad = true;
        slots[_x, _y].roadObject = go;
        go.transform.SetParent(roadParemt);
    }

    public bool CheckRoad()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if(slots[x,y].isDefaultRoad)
                {
                    //Debug.Log($"Check Road {x} - {y} : {slots[x,y].roadObject == null}");
                    if(slots[x,y].roadObject == null)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void PrepairEndgame()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if(slots[x,y].item != null)
                {
                    PipeObject po = slots[x,y].item.GetComponent<PipeObject>();
                    if(po != null)
                    {
                        if(po.pipeData.pipeType == PipeType.Obstacle)
                        {
                            po.transform.SetParent(decorateParent);
                        }
                    }
                }
                if(slots[x,y].roadObject != null)
                {
                    slots[x,y].roadObject.transform.SetParent(roadParemt);
                }
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