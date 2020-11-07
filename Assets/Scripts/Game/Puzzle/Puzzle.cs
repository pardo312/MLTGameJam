using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    private int tilesPerLine = 3;
    [SerializeField]private GameObject tileGameObject;
    private Tile emptyTile;
    private Tile selectedTile;
    private Vector2 mouseTileRatio =Vector2.zero;

    private float offset = 110f;
    private void Start() {
        createPuzzleTiles();
    }
    void createPuzzleTiles()
    {
        for(int col = 0; col<tilesPerLine;col++){
            for(int row = 0; row<tilesPerLine;row++){
                GameObject tileObject = GameObject.Instantiate(tileGameObject);
                tileObject.transform.parent = this.transform;
                tileObject.transform.localPosition = new Vector3(row*(100 + offset)-(this.GetComponent<Image>().rectTransform.rect.width/2),col*(100 + offset)-320+ offset,0);


                Tile tile = tileObject.GetComponent<Tile>();
                tile.OnTilePressed += moveTileInput;
                tile.OnTileReleased += releaseTileInput;

                if (col == 0 && row == tilesPerLine -1)
                {
                    tileObject.SetActive(false);
                    emptyTile = tile;
                }
            }
        }
    }
    void moveTileInput(Tile tileToMove){
        selectedTile = tileToMove;
        
    } 
    void releaseTileInput(Tile tileToMove){
        selectedTile = null;
    }
    
    private void Update() {
        if(selectedTile!=null)
        {
            Canvas myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            Vector2 Mousepos;
            Vector2 Tilepos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Mousepos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, selectedTile.initPosition, myCanvas.worldCamera, out Tilepos);
            if(mouseTileRatio==Vector2.zero){
                mouseTileRatio = Mousepos-Tilepos;
            }
            Vector2 currentMouse = myCanvas.transform.TransformPoint(Mousepos-mouseTileRatio);
            
            Vector2 emptyTilePos = emptyTile.transform.position;
            Vector2 CurrentPosition = (Vector2)selectedTile.transform.position;

            Debug.Log("CurrentPosition: " + (int)currentMouse.x);
            // Debug.Log("selectedTile: " + selectedTile.transform.localPosition);
            // Debug.Log("MousePosX: "+MousePosition.x);

            Debug.Log("InitLimitX: "+(int)selectedTile.initPosition.x);
            Debug.Log("EmptyLimitX: "+(int)(emptyTilePos.x));
            // Debug.Log("InitLimitY: "+(int)selectedTile.initPosition.y);
            // Debug.Log("EmptyLimitY: "+(int)(emptyTilePos.y));

            if((currentMouse.x <= Mathf.Max(emptyTilePos.x,selectedTile.initPosition.x)
            && currentMouse.x >= Mathf.Min(emptyTilePos.x,selectedTile.initPosition.x))&&
            (currentMouse.y <= Mathf.Max(emptyTilePos.y,selectedTile.initPosition.y)
            && currentMouse.y >= Mathf.Min(emptyTilePos.y,selectedTile.initPosition.y)))
            {     
                selectedTile.transform.position = currentMouse;
            }
            
        }
        else{
            mouseTileRatio=Vector2.zero;
        }
    }
}
