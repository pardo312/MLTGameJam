using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [HideInInspector] public bool isOnTransition;
    [SerializeField] private Animator fadeAnim;
    [HideInInspector] public int currentTree = 1 ;

    private void Awake()
    {        
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
