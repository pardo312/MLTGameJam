using System.Collections;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField]private GameObject backButton;
    // Update is called once per frame
    public void continueButton()
    {
        StartCoroutine(playSolvedMusic());
    }
    
    IEnumerator playSolvedMusic()
    {
        GameObject puzzleUI = this.transform.parent.parent.parent.gameObject;
        puzzleUI.GetComponent<Animator>().SetBool("PuzzleStart",false);
        puzzleUI.GetComponent<Animator>().SetBool("PuzzleSolved",true);
        

        yield return new WaitForSeconds(2);

        GameObject.FindGameObjectWithTag($"Tree{gameStateManager.currentTree}").GetComponent<Animator>().enabled = true;
        
        yield return new WaitForSeconds(1);
        MusicManager.instance.Play($"TreeGrow{gameStateManager.currentTree}");

        yield return new WaitForSeconds(8);

        puzzleUI.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<BackFromPuzzle>().backFromPuzzle();
        MusicManager.instance.Play("Overworld");
        backButton.SetActive(true);
    }
}
