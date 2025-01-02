using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "DiaryModelSO", menuName = "ScriptableObjects/DiaryModelSO", order = 1)]
public class DiaryModelSO : ScriptableObject
{
    public string id;
    public Sprite headSP;
    public Sprite itemSP;
    public string topicST;
    public string descriptionST;
    public string descriptionST2;
    public bool isVideo;

    [Header("Description")]
    public Sprite headIMG;
    public Sprite textDesIMG;
}
