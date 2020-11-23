using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickOnBody : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField, Range(1, 4)] private int bodyNumber;
    [SerializeField] private GameObject puzzleUI;
    private int transitionSpeed = 1;
    private GameObject associatedTree;
    private bool transitionOn;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        associatedTree = GameObject.Find("Tree" + bodyNumber);
        transitionOn = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnMouseUp()
    {
        if (gameStateManager.isOnTransition == false)
            transitionOn = true;
    }
    void LateUpdate()
    {
        if (transitionOn)
        {
            gameStateManager.isOnTransition = true;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(associatedTree.transform.position.x, associatedTree.transform.position.y, -10), Time.deltaTime * transitionSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, Time.deltaTime * transitionSpeed);

            if (Camera.main.orthographicSize < 3.02)
            {
                gameStateManager.currentTree = bodyNumber;
                gameStateManager.isOnTransition = false;
                transitionOn = false;
                puzzleUI.SetActive(true);
            }
        }

    }

    private void Update()
    {
        if (gameStateManager.treeAlredySolved[bodyNumber - 1]){
            spriteRenderer.color = Color.white;
            animator.enabled = false;
        }
    }
}
