using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectLevelNumberSlot : MonoBehaviour
{
    public GameObject numberOBJ;
    public GameObject pointOBJ;
    public GameObject starOBJ;
    public SpriteRenderer[] starIndexs;

    public void Start()
    {
    }

    public void SetFinished(SelectLevelState state)
    {
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

    public void SetCurrent(SelectLevelState state)
    {
        pointOBJ.SetActive(true);
        starOBJ.SetActive(false);
        SpriteRenderer numberOBJSR = numberOBJ.GetComponent<SpriteRenderer>();  
        numberOBJSR.sprite = state.model.numberIMG[0];
    }

    public void SetNoClear(SelectLevelState state)
    {
        pointOBJ.SetActive(false);
        starOBJ.SetActive(false);
        SpriteRenderer numberOBJSR = numberOBJ.GetComponent<SpriteRenderer>();  
        numberOBJSR.sprite = state.model.numberIMG[1];
    }

    public void OnMouseOver()
    {
        Debug.Log("OnMouseOver:" + name);
        numberOBJ.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void OnMouseExit()
    {
        Debug.Log("OnMouseExit:" + name);
        numberOBJ.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnMouseUp()
    {
        Debug.Log("OnMouseUp:" + name);
    }

    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown:" + name);
        if(UiController.IsPointerOverUIObject()) return;
        UILobbyGameManager.Instance.StartLoading(() =>
        {
            Debug.Log("Loading Complete");
        });
    }
}