using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Puzzle : MonoBehaviour
{
    private int tilesPerLine = 3;
    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private Texture2D image;
    private Tile[,] listOfTiles;
    private Tile[,] originallistOfTiles;
    private Tile emptyTile;
    private Tile selectedTile;
    private Vector2 mouseTileDistance = Vector2.zero;
    private Vector3 initMousePosition = Vector2.zero;
    private float tileWidth = 210;
    private int offset = 110;
    private int maxDistanceFromEmptyTile = 250;
    private bool puzzleFinished = false;

    void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    void OnEnable()
    {
        createPuzzleTiles();
        
    }

    void createPuzzleTiles()
    {
        listOfTiles = new Tile[tilesPerLine, tilesPerLine];
        originallistOfTiles = new Tile[tilesPerLine, tilesPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, tilesPerLine);
        for (int col = 0; col < tilesPerLine; col++)
        {
            for (int row = 0; row < tilesPerLine; row++)
            {
                GameObject tileObject = GameObject.Instantiate(tileGameObject);
                tileObject.transform.parent = this.transform;

                //Bad Practice, dont do it ;_v
                switch (tilesPerLine)
                {
                    case 3:
                        tileObject.transform.localPosition = new Vector3(row * (100 + offset) - (this.GetComponent<Image>().rectTransform.rect.width / 2),
                        col * (100 + offset) - (this.GetComponent<Image>().rectTransform.rect.height / 3), 0);

                        tileWidth = 200f;
                        maxDistanceFromEmptyTile = 250;
                        break;
                    case 4:
                        tileObject.transform.localScale = new Vector3(tileObject.transform.localScale.x / 1.35f, tileObject.transform.localScale.y / 1.35f, tileObject.transform.localScale.z);

                        tileObject.transform.localPosition = new Vector3(row * (48 + offset) - (this.GetComponent<Image>().rectTransform.rect.width / 2),
                        col * (47 + offset) - (this.GetComponent<Image>().rectTransform.rect.height / 2.65f), 0);

                        tileWidth = 155f;
                        maxDistanceFromEmptyTile = 200;
                        break;
                    case 5:
                        tileObject.transform.localScale = new Vector3(tileObject.transform.localScale.x / 1.82f, tileObject.transform.localScale.y / 1.82f, tileObject.transform.localScale.z);

                        tileObject.transform.localPosition = new Vector3(row * (18 + offset) - (this.GetComponent<Image>().rectTransform.rect.width / 2),
                        col * (18 + offset) - (this.GetComponent<Image>().rectTransform.rect.height / 2.425f), 0);
                        tileWidth = 125f;
                        maxDistanceFromEmptyTile = 150;
                        break;
                }

                tileObject.GetComponent<RawImage>().texture = imageSlices[row, col];

                Tile tile = tileObject.GetComponent<Tile>();
                tile.listPostion = new Vector2Int(row, col);
                tile.OnTilePressed += moveTileInput;
                tile.OnTileReleased += releaseTileInput;

                listOfTiles[row, col] = tile;

                if (col == 0 && row == tilesPerLine - 1)
                {
                    tileObject.SetActive(false);
                    emptyTile = tile;
                }
            }
        }
        System.Array.Copy(listOfTiles, originallistOfTiles, listOfTiles.Length);
    }
    public void randomizePuzzle()
    {
        List<int> iRandomAlredy = new List<int>();
        List<int> jRandomAlredy = new List<int>();
        for (int i = 0; i < tilesPerLine; i++)
        {
            for (int j = 0; j < tilesPerLine; j++)
            {
                Tile tileToRandomize = listOfTiles[i,j];
                Vector3 tileToRandomizePos = listOfTiles[i,j].transform.position;
                int rndI = Random.Range(0,i);
                int rndJ = Random.Range(0,j);
                listOfTiles[i,j].transform.position = listOfTiles[rndI,rndJ].transform.position;
                listOfTiles[i,j] = listOfTiles[rndI,rndJ];
                listOfTiles[rndI,rndJ].transform.position = tileToRandomizePos;
                listOfTiles[rndI,rndJ] = tileToRandomize;
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
        releaseTile();
        selectedTile = null;
    }

    private void Update()
    {
        if (puzzleFinished)
        {
            Debug.Log("Puzzle solved");
            puzzleFinished=false;
        }
        else
        {
            manageSelectedTile();
            checkIfWinGame();
        }
    }
    private void checkIfWinGame()
    {
        for (int i = 0; i < tilesPerLine; i++)
        {
            for (int j = 0; j < tilesPerLine; j++)
            {
                if (originallistOfTiles[i, j] != listOfTiles[i, j])
                    return;
            }
        }
        puzzleFinished = true;
    }
    private void manageSelectedTile()
    {
        if (selectedTile != null)
        {
            if (Vector3.Distance(emptyTile.transform.position, selectedTile.transform.position) < maxDistanceFromEmptyTile)
            {
                Canvas myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Vector2 Mousepos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Mousepos);

                mouseTileDistance = initMousePosition - selectedTile.initPosition;
                Vector2 currentMousePosition = myCanvas.transform.TransformPoint(Mousepos - mouseTileDistance);

                if (Mathf.Abs(emptyTile.transform.position.x - selectedTile.initPosition.x) > tileWidth / 2)
                {
                    if (
                        //Si el bloque esta a la derecha del emptyBlock
                        (emptyTile.transform.position.x - selectedTile.initPosition.x > 0
                        && currentMousePosition.x < selectedTile.initPosition.x + tileWidth
                        && currentMousePosition.x >= selectedTile.initPosition.x)
                    ||
                        //Si el bloque esta a la izquierda del emptyBlock
                        (emptyTile.transform.position.x - selectedTile.initPosition.x < 0
                        && currentMousePosition.x > selectedTile.initPosition.x - tileWidth
                        && currentMousePosition.x <= selectedTile.initPosition.x))
                    {
                        selectedTile.transform.position = new Vector3(currentMousePosition.x, selectedTile.transform.position.y, selectedTile.transform.position.z);
                    }
                }
                else if (Mathf.Abs(emptyTile.transform.position.y - selectedTile.initPosition.y) > tileWidth / 2)
                {
                    if (
                        //Si el bloque esta arriba del emptyBlock
                        (emptyTile.transform.position.y - selectedTile.initPosition.y > 0
                        && currentMousePosition.y < selectedTile.initPosition.y + tileWidth
                        && currentMousePosition.y >= selectedTile.initPosition.y)
                    ||
                        //Si el bloque esta abajo del emptyBlock
                        (emptyTile.transform.position.y - selectedTile.initPosition.y < 0
                        && currentMousePosition.y > selectedTile.initPosition.y - tileWidth
                        && currentMousePosition.y <= selectedTile.initPosition.y))
                    {
                        selectedTile.transform.position = new Vector3(selectedTile.transform.position.x, currentMousePosition.y, selectedTile.transform.position.z);
                    }
                }
            }
        }
    }
    private void releaseTile()
    {
        if (Vector3.Distance(selectedTile.transform.position, selectedTile.initPosition) > tileWidth / 1.25f)
        {
            //Cambiar empty tile y current tile
            Vector3 emptyTilePosTemp = emptyTile.transform.position;
            emptyTile.transform.position = selectedTile.initPosition;
            selectedTile.transform.position = emptyTilePosTemp;

            //Intercambiarlos en la lista de tiles
            Tile tempSelectedTile = listOfTiles[selectedTile.listPostion.x, selectedTile.listPostion.y];

            Tile tempEmptyTile = listOfTiles[emptyTile.listPostion.x, emptyTile.listPostion.y];

                //Cambia selected por empty
            listOfTiles[selectedTile.listPostion.x, selectedTile.listPostion.y] = tempEmptyTile;
                //Cambia selected por empty
            listOfTiles[emptyTile.listPostion.x, emptyTile.listPostion.y] = tempSelectedTile;
        }
        else
        {
            selectedTile.transform.position = selectedTile.initPosition;
        }
    }
}
