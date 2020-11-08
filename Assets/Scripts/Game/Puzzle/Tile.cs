using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event System.Action<Tile> OnTilePressed;
    public event System.Action<Tile> OnTileReleased;
    
    public float id;
    public Vector3 initPosition;
    public Vector2Int listPostion;

    private void Start() {
        initPosition = transform.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
       if(OnTilePressed !=null)
       {
            initPosition = transform.position;
            OnTilePressed(this);
       }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(OnTileReleased !=null)
       {
            OnTileReleased(this);
       }
    }
    
}
