using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    swordman, archer
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public Character currentChar;

    private void Awake()
    {
        if(instance == null) instance = this;
        else if(instance != null) return;

        DontDestroyOnLoad(gameObject);
    }
}
