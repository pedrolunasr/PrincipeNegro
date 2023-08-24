using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GameMaster : MonoBehaviour
{

    public static GameMaster instance { get; private set; }
    public Vector2 lastCheckPointPos;
    public int GoldAtCheckPoint;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
