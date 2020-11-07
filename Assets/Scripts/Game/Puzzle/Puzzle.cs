using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    private int tilesPerLine = 3;
    [SerializeField] private GameObject tileGameObject;
    private Tile emptyTile;
    private Tile selectedTile;
    private Vector2 mouseTileDistance = Vector2.zero;
    private Vector3 initMousePosition = Vector2.zero;

    private float offset = 110f;
    private void Start()
    {
        createPuzzleTiles();
    }
    void createPuzzleTiles()
    {
        for (int col = 0; col < tilesPerLine; col++)
        {
            for (int row = 0; row < tilesPerLine; row++)
            {
                GameObject tileObject = GameObject.Instantiate(tileGameObject);
                tileObject.transform.parent = this.transform;
                tileObject.transform.localPosition = new Vector3(row * (100 + offset) - (this.GetComponent<Image>().rectTransform.rect.width / 2), col * (100 + offset) - 320 + offset, 0);


                Tile tile = tileObject.GetComponent<Tile>();
                tile.OnTilePressed += moveTileInput;
                tile.OnTileReleased += releaseTileInput;

                if (col == 0 && row == tilesPerLine - 1)
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
        selectedTile = null;
    }

    private void Update()
    {
        if (selectedTile != null)
        {
           
                Canvas myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Vector2 Mousepos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Mousepos);

                mouseTileDistance = (Vector2)initMousePosition - selectedTile.initPosition;

                Vector2 currentTilePosition = myCanvas.transform.TransformPoint(Mousepos - mouseTileDistance);

                Vector2 emptyTilePos = emptyTile.transform.position;
                Vector2 CurrentPosition = (Vector2)selectedTile.transform.position;

                if ((currentTilePosition.x <= Mathf.Max(emptyTilePos.x, selectedTile.initPosition.x)
                && currentTilePosition.x >= Mathf.Min(emptyTilePos.x, selectedTile.initPosition.x)) &&
                (currentTilePosition.y <= Mathf.Max(emptyTilePos.y, selectedTile.initPosition.y)
                && currentTilePosition.y >= Mathf.Min(emptyTilePos.y, selectedTile.initPosition.y)))
                {
                    if (((Vector2)Input.mousePosition - CurrentPosition).x < 70 ||
                        ((Vector2)Input.mousePosition - CurrentPosition).x > 130)
                    {
                        selectedTile.transform.position = new Vector3(currentTilePosition.x, selectedTile.transform.position.y, selectedTile.transform.position.z);
                    }
                    if (((Vector2)Input.mousePosition - CurrentPosition).y < -35 ||
                        ((Vector2)Input.mousePosition - CurrentPosition).y > 35)
                    {
                        selectedTile.transform.position = new Vector3(selectedTile.transform.position.x, currentTilePosition.y, selectedTile.transform.position.z);
                    }
                }
        }
        else
        {
            initMousePosition = Vector2.zero;
            mouseTileDistance = Vector2.zero;
        }
    }
}
