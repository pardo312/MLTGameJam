using System.Collections;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject puzzleUI;
    // Update is called once per frame
    public void continueButton()
    {
        StartCoroutine(playSolvedMusic());
    }
    
    IEnumerator playSolvedMusic()
    {
        puzzleUI.GetComponent<Animator>().SetBool("PuzzleStart",false);
        puzzleUI.GetComponent<Animator>().SetBool("PuzzleSolved",true);
        

        yield return new WaitForSeconds(2);

        GameObject.FindGameObjectWithTag($"Tree{gameStateManager.currentTree}").GetComponent<Animator>().enabled = true;
        
        yield return new WaitForSeconds(1);
        MusicManager.instance.Play($"TreeGrow{gameStateManager.currentTree}");

        yield return new WaitForSeconds(8);

        puzzleUI.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<BackFromPuzzle>().backFromPuzzle();
        backButton.SetActive(true);
    }
}
