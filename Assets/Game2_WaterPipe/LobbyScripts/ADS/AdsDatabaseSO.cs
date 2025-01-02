using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AdsDatabaseSO", menuName = "ScriptableObjects/AdsDatabaseSO", order = 1)]
public class AdsDatabaseSO : ScriptableObject
{
    public List<AdsModel> adsModels = new List<AdsModel>();

    private Dictionary<AdsType, AdsModel> adsDic = new Dictionary<AdsType, AdsModel>();
    public AdsModel GetAdsModel(AdsType _AdsType)
    {
        if (adsDic.ContainsKey(_AdsType))
        {
            return adsDic[_AdsType];
        }
        else
        {
            return adsDic[_AdsType] = adsModels.ToList().Find(o => o.adsType == _AdsType);
        }
    }
}
