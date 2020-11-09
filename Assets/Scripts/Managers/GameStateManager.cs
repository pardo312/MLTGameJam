using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public bool isOnTransition;
    [SerializeField] private Animator fadeAnim;
    [HideInInspector] public int currentTree = 1 ;
    [HideInInspector] public bool[] treeAlredySolved ;


    private void Awake()
    {        
        treeAlredySolved = new bool[2];
        treeAlredySolved[0]=false;
        treeAlredySolved[1]=false;
        initVariables();
    }
    public void PlayBM() {
        MusicManager.instance.StopPlayingAll();
        StartCoroutine(WaitSeconds());
    }

    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        MusicManager.instance.Play("Overworld");
    }

    private void initVariables()
    {
        isOnTransition = false;
    }


}
