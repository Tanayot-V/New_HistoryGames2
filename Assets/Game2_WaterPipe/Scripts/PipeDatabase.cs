using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "PipeDatabase", menuName = "ScriptableObjects/PipeDatabase", order = 1)]
public class PipeDatabase : ScriptableObject
{
    public List<PipeModel> pipeModels = new List<PipeModel>();

    private Dictionary<PipeType, PipeModel> pipeDic = new Dictionary<PipeType, PipeModel>();
    public PipeModel GetPipeModel(PipeType _pipeType)
    {
        if (pipeDic.ContainsKey(_pipeType))
        {
            return pipeDic[_pipeType];
        }
        else
        {
            return pipeDic[_pipeType] = pipeModels.ToList().Find(o => o.pipeType == _pipeType);
        }
    }

    public Sprite GetPicture(PipeType _pipeType)
    {
        return GetPipeModel(_pipeType).picture;
    }
}
