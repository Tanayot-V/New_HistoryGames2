using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum AdsType
{
    MoveOut, //มูฟหมด
    AdsItem, //เอาไอเทม
    EndStage2, //จบด่าน2
    EndStage4, //ด่าน 4 การบริหารจัดการน้ำได้ดี
    WastWaterMove, //น้ำเสีย
    WastWaterGood, //ใส่ใจในการบำบัดน้ำเสีย (จัดการน้ำเสียได้ดี) 
    EndStage9, //จบเกม
}

[System.Serializable]
public class AdsModel
{
    public string modelID;
    public AdsType adsType;
    public string[] paths;
}

public class AdsManager : Singletons<AdsManager>
{
    public AdsDatabaseSO adsDatabaseSO;
    public VidPlayer vidPlayer;
    [Header("Do Ads Page")]
    public GameObject adsPageOBJ;
    public GameObject contentOBJ;
    private System.Action adsCallback;   
    
    public void OpenDOAdsPage(Image _itemIMG = null)
    {
        adsPageOBJ.SetActive(true);
        adsPageOBJ.transform.GetChild(0).gameObject.SetActive(true);
        contentOBJ.GetComponent<CanvasGroupTransition>().FadeIn();
        if(_itemIMG != null) adsPageOBJ.GetComponent<AdsPrefab>().itemImage.sprite = _itemIMG.sprite;
    }

    public void OpenDOAdsPage(System.Action _callback, Image _itemIMG = null)
    {
        adsCallback = _callback;
        OpenDOAdsPage(_itemIMG);
    }

    public void AdsClick()
    {
        OpenAdsVideo(AdsType.AdsItem, adsCallback);
        adsPageOBJ.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OpenAdsVideo(AdsType _AdsType)
    {
        AdsModel adsModel = adsDatabaseSO.GetAdsModel(_AdsType);
        if (adsModel != null)
        {
                if(_AdsType == AdsType.AdsItem)
                {
                    int randomIndex = Random.Range(0, adsModel.paths.Length);
                    vidPlayer.PlayVideoURL(adsModel.paths[randomIndex]);
                }
                else
                {
                    vidPlayer.PlayVideoURL(adsModel.paths[0]);
                }
        }
    }
    public void OpenAdsVideo(AdsType _AdsType, System.Action _callback)
    {
        AdsModel adsModel = adsDatabaseSO.GetAdsModel(_AdsType);
        if (adsModel != null)
        {
            if(_AdsType == AdsType.AdsItem)
            {
                int randomIndex = Random.Range(0, adsModel.paths.Length);
                vidPlayer.PlayVideoURL(adsModel.paths[randomIndex], _callback);
            }
            else
            {
                vidPlayer.PlayVideoURL(adsModel.paths[0],_callback);
            }
        }
    }
}
