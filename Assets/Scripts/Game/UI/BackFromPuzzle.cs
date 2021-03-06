﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackFromPuzzle : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameObject mainTree;
    [SerializeField] private GameObject puzzleUI;
    private int transitionSpeed = 1;
    private bool initBackFromPuzzle = false;
    [SerializeField] private Animator fadeAnim;
    // Update is called once per frame
    public void backFromPuzzle()
    {
        if (gameStateManager.isOnTransition == false)
            initBackFromPuzzle = true;

        if (!gameStateManager.GameFinished())
        {
            MusicManager.instance.Play("Overworld");
        }
    }
    private void Update()
    {
        if (initBackFromPuzzle)
        {
            gameStateManager.isOnTransition = true;
            puzzleUI.SetActive(false);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(mainTree.transform.position.x, mainTree.transform.position.y, -10), Time.deltaTime * transitionSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, Time.deltaTime * transitionSpeed);

            if (Camera.main.orthographicSize > 4.99f)
            {
                initBackFromPuzzle = false;
                gameStateManager.isOnTransition = false;

                if (gameStateManager.GameFinished())
                {
                    gameStateManager.isOnTransition = true;
                    MusicManager.instance.StopPlayingAll();
                    MusicManager.instance.Play("BigTreeGrow");
                    mainTree.GetComponent<Animator>().enabled = true;
                    StartCoroutine( endOfTransition());
                }
            }
        }
    }
    IEnumerator endOfTransition(){
        yield return new WaitForSeconds(23);
        gameStateManager.isOnTransition = false;
        fadeAnim.SetBool("FadeOut",false);
        fadeAnim.SetBool("FadeIn",true);
        yield return new WaitForSeconds(2);
        MusicManager.instance.Play("TitleTheme");
        SceneManager.LoadScene("Credits");
    }
}
