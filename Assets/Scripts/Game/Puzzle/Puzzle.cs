using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Puzzle : MonoBehaviour
{
    private int tilesPerLine = 3;
    [SerializeField] private GameObject tileGameObject;
    private List<GameObject> listOfTiles;
    private Tile emptyTile;
    private Tile selectedTile;
    private Vector2 mouseTileDistance = Vector2.zero;
    private Vector3 initMousePosition = Vector2.zero;
    private int tileWidth = 210;
    private float offset = 110f;
    private void Start()
    {
        listOfTiles = new List<GameObject>();
        createPuzzleTiles();
    }
    void createPuzzleTiles()
    {
        for (int col = 0; col < tilesPerLine; col++)
        {
            for (int row = 0; row < tilesPerLine; row++)
            {
                GameObject tileObject = GameObject.Instantiate(tileGameObject);
                listOfTiles.Add(tileObject);
                tileObject.transform.parent = this.transform;
                tileObject.transform.localPosition = new Vector3(row * (100 + offset) - (this.GetComponent<Image>().rectTransform.rect.width / 2), col * (100 + offset) - 320 + offset, 0);


                Tile tile = tileObject.GetComponent<Tile>();
                tile.OnTilePressed += moveTileInput;
                tile.OnTileReleased += releaseTileInput;

                if (col == 1 && row == tilesPerLine - 2)
                {
                    tileObject.SetActive(false);
                    emptyTile = tile;
                }
            }
        }
    }
    void moveTileInput(Tile tileToMove)
    {
        selectedTile = tileToMove;
        initMousePosition = Input.mousePosition;
    }
    void releaseTileInput(Tile tileToMove)
    {
        ReleaseTile();
        selectedTile = null;
    }

    private void Update()
    {
        if (selectedTile != null)
        {
            if(Vector3.Distance(emptyTile.transform.position, selectedTile.transform.position) < 250 )
            {
                Canvas myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Vector2 Mousepos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Mousepos);

                mouseTileDistance = initMousePosition - selectedTile.initPosition;
                Vector2 currentMousePosition = myCanvas.transform.TransformPoint(Mousepos - mouseTileDistance);
                
                if(Mathf.Abs(emptyTile.transform.position.x-selectedTile.initPosition.x)>tileWidth/2)
                {
                    if(
                        //Si el bloque esta a la derecha del emptyBlock
                        (emptyTile.transform.position.x-selectedTile.initPosition.x >0 
                        && currentMousePosition.x<selectedTile.initPosition.x+tileWidth 
                        && currentMousePosition.x>=selectedTile.initPosition.x)
                    ||
                        //Si el bloque esta a la izquierda del emptyBlock
                        (emptyTile.transform.position.x-selectedTile.initPosition.x <0 
                        && currentMousePosition.x>selectedTile.initPosition.x-tileWidth 
                        && currentMousePosition.x<=selectedTile.initPosition.x))
                    {
                        selectedTile.transform.position= new Vector3(currentMousePosition.x,selectedTile.transform.position.y,selectedTile.transform.position.z);
                    }   
                }
                else if(Mathf.Abs(emptyTile.transform.position.y-selectedTile.initPosition.y)>100){
                    if(
                        //Si el bloque esta arriba del emptyBlock
                        (emptyTile.transform.position.y-selectedTile.initPosition.y >0 
                        && currentMousePosition.y<selectedTile.initPosition.y+tileWidth 
                        && currentMousePosition.y>=selectedTile.initPosition.y)
                    ||
                        //Si el bloque esta abajo del emptyBlock
                        (emptyTile.transform.position.y-selectedTile.initPosition.y <0 
                        && currentMousePosition.y>selectedTile.initPosition.y-tileWidth
                        && currentMousePosition.y<=selectedTile.initPosition.y))
                    {
                         selectedTile.transform.position= new Vector3(selectedTile.transform.position.x,currentMousePosition.y,selectedTile.transform.position.z);
                    }
                }
            }
        }
    }

    private void ReleaseTile(){
        if(Vector3.Distance(selectedTile.transform.position, selectedTile.initPosition) > 150){
            Debug.Log("hi");
            //Cambiar empty tile y current tile
            Vector3 emptyTilePosTemp = emptyTile.transform.position;
            emptyTile.transform.position = selectedTile.initPosition;
            selectedTile.transform.position = emptyTilePosTemp;
        }
        else{
            selectedTile.transform.position = selectedTile.initPosition;
        }
    }
}
