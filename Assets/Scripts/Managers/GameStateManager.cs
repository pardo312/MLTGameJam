using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public bool isOnTransition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad( this.gameObject );

        initVariables();
    }
    private void initVariables(){
        isOnTransition= false;
    }


}
