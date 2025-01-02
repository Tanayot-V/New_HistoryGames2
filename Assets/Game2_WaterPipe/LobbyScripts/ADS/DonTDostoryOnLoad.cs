using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDostoryOnLoad : MonoBehaviour
{
    private static DonDostoryOnLoad _Instance;
    public static DonDostoryOnLoad Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("[Script]: -- DonDostoryOnLoad Instance --");
                _Instance = obj.AddComponent<DonDostoryOnLoad>();
                DontDestroyOnLoad(obj);
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
