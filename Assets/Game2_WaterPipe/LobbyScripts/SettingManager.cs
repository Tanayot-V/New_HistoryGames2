using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public GameObject settingOBJ;
    public void OpenPage()
    {
        settingOBJ.SetActive(true);
    }

    public void ClosePage()
    {
        settingOBJ.SetActive(false);
    }
}
