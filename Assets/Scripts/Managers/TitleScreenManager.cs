using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    // Start is called before the first frame update
    void Start()
    {
        fadeAnim.SetBool("FadeOut",true);
        MusicManager.instance.StopPlayingAll();
        MusicManager.instance.Play("TitleTheme");
    }

}
