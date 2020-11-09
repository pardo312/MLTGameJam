using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.StopPlayingAll();
        MusicManager.instance.Play("TitleTheme");
    }

}
