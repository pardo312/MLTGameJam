using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Listens for the onClick events for the main menu menu buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameObject mainTree;
    [SerializeField] private Animator fadeAnim;
    private bool transitionOn;
    private int transitionSpeed = 1;
    private bool playedMusic;

    /// <summary>
    /// Handles the onClick event from the play button
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {

        if (gameStateManager.isOnTransition == false)
            transitionOn = true;

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (transitionOn)
        {
            gameStateManager.isOnTransition = true;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(mainTree.transform.position.x, mainTree.transform.position.y, -10), Time.deltaTime * transitionSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, Time.deltaTime * transitionSpeed);

            if (!playedMusic)
            {
                gameStateManager.PlayBM();
                playedMusic = true;
            }

            if (Camera.main.orthographicSize > 4.9)
            {
                gameStateManager.isOnTransition = false;
                transitionOn = false;
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Handles the onClcik event from the quit button
    /// </summary>
    public void HandleCreditsButtonOnClickEvent()
    {
        fadeAnim.SetBool("FadeOut",false);
        fadeAnim.SetBool("FadeIn",true);
        StartCoroutine(loadScene());
    }

    private IEnumerator loadScene(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneName.Credits.ToString());
    }
}
