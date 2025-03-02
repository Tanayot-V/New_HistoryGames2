using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GridCanvas))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridCanvas gridManager = (GridCanvas)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Level"))
        {
            gridManager.CreateLevel();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            gridManager.ClearGrid();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Level Data"))
        {
            gridManager.SaveState(gridManager.savelevelName);
        }
        if (GUILayout.Button("Load Level Data"))
        {
            gridManager.LoadState(gridManager.loadlevelName);
        }
        GUILayout.EndHorizontal();

        // Draw table data in slots array
        if (gridManager.slots != null)
        {
            for (int j = gridManager.slots.GetLength(1) - 1; j >= 0; j--)
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < gridManager.slots.GetLength(0); i++)
                {
                    if(gridManager.slots[i, j]?.item != null)
                    {
                        GUILayout.Label(gridManager.slots[i, j]?.item.name, GUILayout.Width(50));
                    }
                    else
                    {
                        GUILayout.Label("-", GUILayout.Width(50));
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
#endif
