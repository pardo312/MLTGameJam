using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Puzzle : MonoBehaviour
{
    private int tilesPerLine = 3;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private Texture2D[] images;
    [SerializeField] private Animator puzzleUIAniamator;
    private Tile[,] listOfTiles;
    private Tile[,] originallistOfTiles;
    private Tile emptyTile;
    private Tile selectedTile;
    private Vector2 mouseTileDistance = Vector2.zero;
    private Vector3 initMousePosition = Vector2.zero;
    private float tileWidth;
    private bool puzzleFinishedInit = false;
    private bool puzzleFinished = false;

    void OnDisable()
    {
        foreach (Transform child in transform)
        {
            if(!child.name.Equals("ContinueButton"))
                Destroy(child.gameObject);
        }
    }
    void OnEnable()
    {
        GameStateManager gsm = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        if(gsm.currentTree==1){
            tilesPerLine=3;
        }
        else
        {
            tilesPerLine=4;   
        }
        puzzleUIAniamator.SetBool("PuzzleStart",true);
        puzzleUIAniamator.SetBool("PuzzleSolved",false);
        puzzleFinished=false;
        createPuzzleTiles();
        if(!gsm.treeAlredySolved[gsm.currentTree-1]){
            randomizePuzzle();
        }
        else{
            emptyTile.gameObject.SetActive(true);
            puzzleFinished=true;
            puzzleFinishedInit=false;
        }
    }

    void createPuzzleTiles()
    {
        listOfTiles = new Tile[tilesPerLine, tilesPerLine];
        originallistOfTiles = new Tile[tilesPerLine, tilesPerLine];
        GameStateManager gsm = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(images[gsm.currentTree-1], tilesPerLine);

        float separationBetweenTiles = 15 / tilesPerLine;
        float CanvasWidth = GetComponent<RectTransform>().rect.width;
        tileWidth = (CanvasWidth - separationBetweenTiles * tilesPerLine - separationBetweenTiles) / tilesPerLine;
        for (int row = 0; row < tilesPerLine; row++)
        {
            for (int col = 0; col < tilesPerLine; col++)
            {
                GameObject tileObject = Instantiate(tileGameObject);
                
                RectTransform tileObjectRectTransform = tileObject.GetComponent<RectTransform>();
                tileObjectRectTransform.sizeDelta = new Vector2(tileWidth, tileWidth);

                tileObject.transform.position = new Vector3(
                    row * (tileWidth + separationBetweenTiles) + tileWidth / 2 + separationBetweenTiles,
                    col * (tileWidth + separationBetweenTiles) + tileWidth / 2 + separationBetweenTiles, 
                    0
                );
                
                tileObject.transform.SetParent(transform, false);

                tileObject.GetComponent<RawImage>().texture = imageSlices[row, col];

                Tile tile = tileObject.GetComponent<Tile>();
                tile.listPosition = new Vector2Int(row, col);
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
        for (int nmoOfShuffle = 0; nmoOfShuffle < 100; nmoOfShuffle++)
        {
            Tile tileToChange = null;
            int iORj = Random.Range(0, 2);
            int moreORLess = Random.Range(0, 2);
            
            if (iORj == 0)
            {
                if (emptyTile.listPosition.x < tilesPerLine - 1 && moreORLess == 1)
                    tileToChange = listOfTiles[emptyTile.listPosition.x + 1, emptyTile.listPosition.y];
                else if (emptyTile.listPosition.x > 0 && moreORLess == 0)
                    tileToChange = listOfTiles[emptyTile.listPosition.x - 1, emptyTile.listPosition.y];
            }
            else
            {
                if (emptyTile.listPosition.y < tilesPerLine - 1 && moreORLess == 1)
                    tileToChange = listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y + 1];
                else if (emptyTile.listPosition.y > 0 && moreORLess == 0)
                    tileToChange = listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y - 1];
            }
            if (tileToChange)
            {
                //Cambiar empty tile y current tile
                Vector3 emptyTilePosTemp = emptyTile.transform.position;
                emptyTile.transform.position = tileToChange.transform.position;
                tileToChange.transform.position = emptyTilePosTemp;

                //Temporal Variables
                Tile tempTileToChange = listOfTiles[tileToChange.listPosition.x, tileToChange.listPosition.y];
                Vector2Int tileToChangeListPosition = tileToChange.listPosition;
                Tile tempEmptyTile = listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y];
                Vector2Int emptyTileListPosition = emptyTile.listPosition;

                //Cambia tileToChange por empty
                listOfTiles[tileToChange.listPosition.x, tileToChange.listPosition.y] = tempEmptyTile;
                tileToChange.listPosition = emptyTileListPosition;
                //Cambia empty por tileToChange
                listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y] = tempTileToChange;
                emptyTile.listPosition = tileToChangeListPosition;
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
            if(puzzleFinishedInit)
            {
                GameStateManager gsm = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
                gsm.treeAlredySolved[gsm.currentTree-1] = true;
                emptyTile.gameObject.SetActive(true);
                MusicManager.instance.StopPlayingAll();
                SoundFXManager.instance.Play("PuzzleSolvedOST");
                puzzleFinishedInit=false;
                Debug.Log("PuzzleSolved");
                backButton.SetActive(false);
                continueButton.SetActive(true);
            }
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
                if (originallistOfTiles[i, j].listPosition != listOfTiles[i, j].listPosition)
                {
                    return;
                }
            }
        }
        puzzleFinishedInit=true;
        puzzleFinished = true;
    }
    private void manageSelectedTile()
    {
        if (selectedTile != null)
        {
            if (Mathf.Abs(emptyTile.listPosition.x - selectedTile.listPosition.x) + Mathf.Abs(emptyTile.listPosition.y - selectedTile.listPosition.y) == 1)
            {
                Canvas myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Vector2 Mousepos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out Mousepos);

                mouseTileDistance = initMousePosition - selectedTile.initPosition;
                Vector2 currentMousePosition = myCanvas.transform.TransformPoint(Mousepos - mouseTileDistance);

                if (
                    //Si el bloque esta a la derecha del emptyBlock
                    (emptyTile.listPosition.x - selectedTile.listPosition.x > 0
                    && currentMousePosition.x < selectedTile.initPosition.x + tileWidth
                    && currentMousePosition.x >= selectedTile.initPosition.x)
                ||
                    //Si el bloque esta a la izquierda del emptyBlock
                    (emptyTile.listPosition.x - selectedTile.listPosition.x < 0
                    && currentMousePosition.x > selectedTile.initPosition.x - tileWidth
                    && currentMousePosition.x <= selectedTile.initPosition.x))
                {
                    selectedTile.transform.position = new Vector3(currentMousePosition.x, selectedTile.transform.position.y, selectedTile.transform.position.z);
                }
                else if (
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
    private void releaseTile()
    {
        if (Vector3.Distance(selectedTile.transform.position, selectedTile.initPosition) > tileWidth / 2)
        {
            string soundToPlay = "MovePuzzle"+Random.Range(1,4);
            SoundFXManager.instance.Play(soundToPlay);
            //Cambiar empty tile y current tile
            Vector3 emptyTilePosTemp = emptyTile.transform.position;
            emptyTile.transform.position = selectedTile.initPosition;
            selectedTile.transform.position = emptyTilePosTemp;

            //Intercambiarlos en la lista de tiles
            Tile tempSelectedTile = listOfTiles[selectedTile.listPosition.x, selectedTile.listPosition.y];
            Vector2Int selectedTileListPosition = selectedTile.listPosition;
            Tile tempEmptyTile = listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y];

            //Cambia selected por empty
            listOfTiles[selectedTile.listPosition.x, selectedTile.listPosition.y] = tempEmptyTile;
            selectedTile.listPosition = emptyTile.listPosition;
            //Cambia selected por empty
            listOfTiles[emptyTile.listPosition.x, emptyTile.listPosition.y] = tempSelectedTile;
            emptyTile.listPosition = selectedTileListPosition;
        }
        else
        {
            selectedTile.transform.position = selectedTile.initPosition;
        }
    }
}
