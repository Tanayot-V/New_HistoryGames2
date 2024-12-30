using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiaryDatabaseSO", menuName = "ScriptableObjects/DiaryDatabaseSO", order = 1)]
public class DiaryDatabaseSO : ScriptableObject
{
    public List<DiaryModelSO> diaryModelSO = new List<DiaryModelSO>();
}
