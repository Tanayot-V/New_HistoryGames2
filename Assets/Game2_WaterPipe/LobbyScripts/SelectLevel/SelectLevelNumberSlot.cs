using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectLevelNumberSlot : MonoBehaviour
{
    private SelectLevelState state;
    public GameObject numberOBJ;
    public GameObject pointOBJ;
    public GameObject starOBJ;
    public SpriteRenderer[] starIndexs;

    public void Start()
    {
    }

    public void SetFinished(SelectLevelState _state)
    {
        state = _state;
        SpriteRenderer numberOBJSR = numberOBJ.GetComponent<SpriteRenderer>();  
       if(state.isFinished)
        {
            starOBJ.SetActive(true);
            pointOBJ.SetActive(false);
            numberOBJSR.sprite = state.model.numberIMG[2];
            
            starIndexs.ToList().ForEach(o => o.material = UILobbyGameManager.Instance.spriteMAT);
            switch (state.star)
            {
                case 1:
                    starIndexs[1].material = UILobbyGameManager.Instance.garyMATWorld;
                    starIndexs[2].material = UILobbyGameManager.Instance.garyMATWorld;
                    break;
                case 2:
                    starIndexs[2].material = UILobbyGameManager.Instance.garyMATWorld;
                    break;
            }
        }
    }

    public void SetCurrent(SelectLevelState _state)
    {
        state = _state;
        pointOBJ.SetActive(true);
        starOBJ.SetActive(false);
        SpriteRenderer numberOBJSR = numberOBJ.GetComponent<SpriteRenderer>();  
        numberOBJSR.sprite = state.model.numberIMG[0];
    }

    public void SetNoClear(SelectLevelState _state)
    {
        state = _state;
        pointOBJ.SetActive(false);
        starOBJ.SetActive(false);
        SpriteRenderer numberOBJSR = numberOBJ.GetComponent<SpriteRenderer>();  
        numberOBJSR.sprite = state.model.numberIMG[1];
    }

    public void OnMouseOver()
    {
        numberOBJ.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void OnMouseExit()
    {
        numberOBJ.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnMouseDown()
    {
        if(!state.isFinished && state.level != LevelDataManager.Instance.GetCurrentLevel()) return;
        if(UiController.IsPointerOverUIObject()) return;
        if(state == null) return;

        LevelDataManager.Instance.SetIsAllReadDiary(LobbyGameManager.Instance.DiaryIsCheckRead());
        UILobbyGameManager.Instance.StartLoading(() =>
        {
            DataCenterManager.Instance.LoadSceneByName(state.model.sceneName);
            Debug.Log("Loading Complete");
        });
    }
}
