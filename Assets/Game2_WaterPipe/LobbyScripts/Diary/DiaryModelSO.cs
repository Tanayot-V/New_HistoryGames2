using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiaryModelSO", menuName = "ScriptableObjects/DiaryModelSO", order = 1)]
public class DiaryModelSO : ScriptableObject
{
    public string id;
    public Sprite headSP;
    public Sprite itemSP;
    public string topicST;
    public string descriptionST;
}
