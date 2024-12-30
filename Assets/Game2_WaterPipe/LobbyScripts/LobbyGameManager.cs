using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGameManager : Singletons<LobbyGameManager>
{
    public DiaryManager diaryManager;

    public void Start()
    {
        diaryManager.InitDiary();
    }
}
