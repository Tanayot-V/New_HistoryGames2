using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCanvas : MonoBehaviour
{
    public GameObject slotPrefab;
    public int rows = 8;
    public int columns = 8;
    public float slotSize = 1f;

    public TextAsset levelData;

    public PipeSlotCanvas[,] slots;

    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject[] obstaclePrefabs;
    void Start()
    {
        CreateGrid();
        CreateLevel();
    }

    void CreateGrid()
    {
        slots = new PipeSlotCanvas[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.anchoredPosition = new Vector2(x * slotSize, y * slotSize);
                PipeSlotCanvas pipeSlot = slot.GetComponent<PipeSlotCanvas>();
                pipeSlot.pos = new Vector2(x, y);
                slots[x, y] = pipeSlot;
            }
        }
    }

    void CreateLevel()
    {
        string[] lines = levelData.text.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[lines.Length - y - 1];
            string[] cell = line.Split(',');
            for (int x = 0; x < cell.Length; x++)
            {
                if(cell[x].StartsWith("0")) continue;
                char cellType = cell[x][0];
                string cellDirection = cell[x][1].ToString() + cell[x][2].ToString();
                PipeObject pipeObject = null;
                switch (cellType)
                {
                    case '1':
                        GameObject start = Instantiate(startPrefab, slots[x, y].transform);
                        start.transform.localPosition = Vector3.zero;
                        slots[x, y].item = start;
                        pipeObject = start.GetComponent<PipeObject>();
                        switch (cellDirection)
                        {
                            case "00":
                                break;
                            case "01":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 0);
                                pipeObject.pipeData.direction = Direction.Up;
                                break;
                            case "02":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 180);
                                pipeObject.pipeData.direction = Direction.Down;
                                break;
                            case "03":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 90);
                                pipeObject.pipeData.direction = Direction.Left;
                                break;
                            case "04":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 270);
                                pipeObject.pipeData.direction = Direction.Right;
                                break;
                        }
                        break;
                    case '2':
                        GameObject end = Instantiate(endPrefab, slots[x, y].transform);
                        end.transform.localPosition = Vector3.zero;
                        slots[x, y].item = end;
                        pipeObject = end.GetComponent<PipeObject>();
                        switch (cellDirection)
                        {
                            case "00":
                                break;
                            case "01":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 0);
                                pipeObject.pipeData.direction = Direction.Up;
                                break;
                            case "02":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 180);
                                pipeObject.pipeData.direction = Direction.Down;
                                break;
                            case "03":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 90);
                                pipeObject.pipeData.direction = Direction.Left;
                                break;
                            case "04":
                                slots[x, y].item.transform.localEulerAngles = new Vector3(0, 0, 270);
                                pipeObject.pipeData.direction = Direction.Right;
                                break;
                        }
                        break;
                    case '3':
                        GameObject obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], slots[x, y].transform);
                        obstacle.transform.localPosition = Vector3.zero;
                        slots[x, y].item = obstacle;
                        break;
                    case '4':
                        GameObject map = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], slots[x, y].transform);
                        map.transform.localPosition = Vector3.zero;
                        slots[x, y].item = map;
                        break;
                }
            }
        }
    }
}
