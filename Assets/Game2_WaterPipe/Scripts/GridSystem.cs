using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class GridSystem : Singletons<GridSystem>
{
    public GameObject prefab; // ใส่ prefab ที่ต้องการใช้ที่นี่
    [SerializeField] private List<Vector2> gridPositions = new List<Vector2>();
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Vector2 gridOffset;
    public Color gridColor = Color.green;

    public bool isIsometric = false;
    [Header("Layer")]
    public string sortingLayerName = "Default";
    public int sortingOrder = 0;

    private GameObject gridParent;

    private void Start() { }

    public void CreateGrid()
    {
        if (gridParent != null)
        {
            Destroy(gridParent);
        }

        gridParent = new GameObject("GridParent");
        gridParent.transform.SetParent(this.transform);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject slot = Instantiate(prefab, position, Quaternion.identity, transform);
                slot.transform.parent = gridParent.transform;
                Vector3 startPosition = GetGridPosition(x, y);
                gridPositions.Add(startPosition);
                GameManager.Instance.PipeManager().AddPipeData(new PipeData());
            }
        }
        return;
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                Vector3 startPosition = GetGridPosition(x, y);

                if (x < width)
                {
                    //CreateGridMesh();
                    Vector3 endPosition = GetGridPosition(x + 1, y);
                    CreateLine(startPosition, endPosition);
                }

                if (y < height)
                {
                    //CreateGridMesh();
                    Vector3 endPosition = GetGridPosition(x, y + 1);
                    CreateLine(startPosition, endPosition);
                }
            }
        }

        for (int x = 0; x < width; x++) // เปลี่ยนเป็น <
        {
            for (int y = 0; y < height; y++) // เปลี่ยนเป็น <
            {
                Vector3 startPosition = GetGridPosition(x, y);
                gridPositions.Add(startPosition);
            }
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("GridLine");
        lineObject.transform.parent = gridParent.transform;

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.useWorldSpace = true;

        lineRenderer.sortingLayerName = sortingLayerName;
        lineRenderer.sortingOrder = sortingOrder;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public Vector3 GetGridPosition(float x, float y)
    {
        if (isIsometric)
        {
            return new Vector3(
                ((x - y) * (cellSize / 2)) + gridOffset.x,
                ((x + y) * (cellSize / 4)) + gridOffset.y,
                0
            );
        }
        else
        {
            return new Vector3(
                (x * cellSize) + gridOffset.x,
                (y * cellSize) + gridOffset.y,
                0
            );
        }
    }

    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        float x, y;
        if (isIsometric)
        {
            float isoX = worldPosition.x / (cellSize / 2);
            float isoY = worldPosition.y / (cellSize / 4);

            x = Mathf.Round((isoX + isoY) / 2);
            y = Mathf.Round((isoY - isoX) / 2);
        }
        else
        {
            x = Mathf.Round(worldPosition.x / cellSize) + 0.5f;
            y = Mathf.Round(worldPosition.y / cellSize) + 0.5f;
        }

        return GetGridPosition(x,y);
    }

    public List<Vector2> GetAllGridPositions()
    {
        return gridPositions;
    }
}
