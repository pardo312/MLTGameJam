using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBody : MonoBehaviour
{
    [SerializeField, Range(1,4)]private int bodyNumber;
    [SerializeField]private GameObject puzzleUI;
    private int transitionSpeed = 1;
    private GameObject associatedTree;
    private bool transitionOn;
    private void Start() {
        associatedTree = GameObject.Find("Tree"+bodyNumber);
        transitionOn=false;
    }
    void OnMouseUp() 
    {
        transitionOn=true;
    } 
    void LateUpdate () {
        if(transitionOn)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position ,new Vector3(associatedTree.transform.position.x,associatedTree.transform.position.y,-10),Time.deltaTime*transitionSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,3,Time.deltaTime*transitionSpeed);
            if(Camera.main.orthographicSize<4)
                puzzleUI.SetActive(true);
            if(Camera.main.orthographicSize<3.02)
                transitionOn=false;
        }

    }
}
