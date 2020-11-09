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
    private void Start() {
        fadeAnim.SetBool("FadeOut",true);
        MusicManager.instance.StopPlayingAll();
        MusicManager.instance.Play("Overworld");
    }
    private void initVariables()
    {
        isOnTransition = false;
    }


}
