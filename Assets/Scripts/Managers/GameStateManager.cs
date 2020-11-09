﻿using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; private set; }
    [HideInInspector] public bool isOnTransition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        initVariables();
    }
    private void Start() {
        MusicManager.instance.StopPlayingAll();
        MusicManager.instance.Play("Overworld");
    }
    private void initVariables()
    {
        isOnTransition = false;
    }


}
