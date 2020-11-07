using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event System.Action<Tile> OnTilePressed;
    public event System.Action<Tile> OnTileReleased;
    
    public Vector2 initPosition;

    private void Start() {
        initPosition = transform.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
       if(OnTilePressed !=null)
       {
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
