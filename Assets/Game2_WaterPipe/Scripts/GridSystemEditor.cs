using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridSystem gridManager = (GridSystem)target;
#if !UNITY_WEBGL
        if (GUILayout.Button("Create Grid"))
        {
            gridManager.CreateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            if (gridManager.transform.childCount > 0)
            {
                foreach (Transform child in gridManager.transform)
                {
                    DestroyImmediate(child.gameObject);
                    GameManager.Instance.GridSystem().GetAllGridPositions().Clear();
                    GameManager.Instance.PipeManager().ClearPipeData();
                }
            }
        }
#endif
    }
}
#endif
