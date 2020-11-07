using UnityEngine;

/// <summary>
/// Listens for the onClick events for the pause menu buttons
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Pauses the game when added to the scene
    /// </summary>
    private void Start()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Handles the on click event from the resume button
    /// </summary>
    public void HandleResumeOnClickEvent()
    {
        //unpause game and destroy menu
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    /// <summary>
    /// Hanldes the onClick event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        //unpause the game, destroy menu, and go to main menu
        Time.timeScale = 1;
        Destroy(gameObject);
        MenuManager.GoToMenu(MenuName.Main);
    }
}
