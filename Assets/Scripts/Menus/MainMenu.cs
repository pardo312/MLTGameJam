using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Listens for the onClick events for the main menu menu buttons
/// </summary>
public class MainMenu : MonoBehaviour
{   
    [SerializeField] private Animator fadeAnim;
    /// <summary>
    /// Handles the onClick event from the play button
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {
        fadeAnim.SetBool("FadeOut",false);
        fadeAnim.SetBool("FadeIn",true);
        StartCoroutine(loadScene());
        
    }
    IEnumerator loadScene(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneName.OverWorld.ToString());
    }
    /// <summary>
    /// Handles the onClcik event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
