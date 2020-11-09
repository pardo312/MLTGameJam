﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFromPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject mainTree;
    [SerializeField] private GameObject puzzleUI;
    private int transitionSpeed = 1;
    private bool initBackFromPuzzle = false;
    // Update is called once per frame
    public void backFromPuzzle()
    {
        if (GameStateManager.instance.isOnTransition == false)
            initBackFromPuzzle = true;
    }
    private void Update()
    {
        if (initBackFromPuzzle)
        {
            GameStateManager.instance.isOnTransition = true;
            puzzleUI.SetActive(false);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(mainTree.transform.position.x, mainTree.transform.position.y, -10), Time.deltaTime * transitionSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, Time.deltaTime * transitionSpeed);

            if (Camera.main.orthographicSize > 4.9f)
            {
                initBackFromPuzzle = false;
                GameStateManager.instance.isOnTransition = false;
            }
        }
    }
}