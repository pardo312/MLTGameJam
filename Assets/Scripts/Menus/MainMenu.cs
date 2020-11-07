using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Listens for the onClick events for the pause menu buttons
/// </summary>
public class MainMenu : MonoBehaviour
{   
    /// <summary>
    /// Handles the onClick event from the play button
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {
        SceneManager.LoadScene(SceneName.LvL1.ToString());
    }

    /// <summary>
    /// Handles the onClcik event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
