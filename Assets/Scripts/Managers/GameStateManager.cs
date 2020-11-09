using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public bool isOnTransition;
    [SerializeField] private Animator fadeAnim;

    private void Awake()
    {        
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
