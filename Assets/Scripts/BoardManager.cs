using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int hoverX = -1;
    private int hoverY = -1;

    public bool isLightTurn = true;

    private bool[,] allowedMoves { set; get; }
    public int[] EnPassantMove { set; get; }
    
    public List<GameObject> piecePrefabs;
    private List<GameObject> piecesActive = new List<GameObject>();

    public ChessPiece[,] ChessPiecePositions { set; get; }
    private ChessPiece selectedPiece;

    public GameObject LightVictoryMessage;
    public GameObject DarkVictoryMessage;

    private void Start() {
        Instance = this;
        ChessPiecePositions = new ChessPiece[8,8];
        EnPassantMove = new int[2] { -1, -1 };
        SpawnAllChessPieces();
    }

    private void Update() {
        DrawChessboard();
        UpdateSelection();

        if(Input.GetMouseButtonDown(0)) {
            if(hoverX >= 0 && hoverY >= 0) {
                if(selectedPiece == null) {
                    SelectChessPiece(hoverX, hoverY);
                } else {
                    MoveChessPiece(hoverX, hoverY);
                }
            }
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private void SelectChessPiece(int x, int y) {
        if (ChessPiecePositions[x, y] == null 
            || ChessPiecePositions[x, y].isLight != isLightTurn) return;

        bool canMove = false;
        allowedMoves = ChessPiecePositions[x, y].AllowedMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    canMove = true;
                    i = 8;
                    break;
                }
            }
        }

        if (!canMove)
            return;

        selectedPiece = ChessPiecePositions[x, y];

        BoardHighlighting.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessPiece(int x, int y) {
        if (allowedMoves[x, y]) {
            ChessPiece enemyPiece = ChessPiecePositions[x, y];

            if(enemyPiece != null && enemyPiece.isLight != isLightTurn)
            {
                piecesActive.Remove(enemyPiece.gameObject);
                Destroy(enemyPiece.gameObject); 

                if(enemyPiece.GetType() == typeof(King))
                {
                    selectedPiece.transform.position = GetTileCenter(x, y);
                    EndGame();
                    return;
                }
            }

            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isLightTurn)
                    enemyPiece = ChessPiecePositions[x, y - 1];
                else
                    enemyPiece = ChessPiecePositions[x, y + 1];

                piecesActive.Remove(enemyPiece.gameObject);
                Destroy(enemyPiece.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (selectedPiece.GetType() == typeof(Pawn))
            {
                if(y == 7) // белая пешка в ферзя
                {
                    piecesActive.Remove(selectedPiece.gameObject);
                    Destroy(selectedPiece.gameObject);
                    SpawnChessPiece(1, x, y);
                    selectedPiece = ChessPiecePositions[x, y];
                }
                else if (y == 0) // черная пешка в ферзя
                {
                    piecesActive.Remove(selectedPiece.gameObject);
                    Destroy(selectedPiece.gameObject);
                    SpawnChessPiece(7, x, y);
                    selectedPiece = ChessPiecePositions[x, y];
                }
                // взятие на проходе
                EnPassantMove[0] = x;
                if (selectedPiece.CurrentY == 1 && y == 3)
                    EnPassantMove[1] = y - 1;
                else if (selectedPiece.CurrentY == 6 && y == 4)
                    EnPassantMove[1] = y + 1;
            }

            ChessPiecePositions[selectedPiece.CurrentX, selectedPiece.CurrentY] = null;
            ChessPiecePositions[x, y] = selectedPiece;
            selectedPiece.transform.position = GetTileCenter(x, y);
            selectedPiece.SetPosition(x, y);
            isLightTurn = !isLightTurn;
        }

        BoardHighlighting.Instance.HideHighlights();
        selectedPiece = null;
    }

    private void UpdateSelection() {
        if(!Camera.main) return;

        RaycastHit hit;
        float raycastDistance = 25.0f;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, LayerMask.GetMask("ChessPlane"))) {
            hoverX = (int) hit.point.x;
            hoverY = (int) hit.point.z;
        } else {
            hoverX = -1;
            hoverY = -1;
        }
    }

    private void DrawChessboard() {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++) {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++) {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        if (hoverX >= 0 && hoverY >= 0)
        {
            Debug.DrawLine(Vector3.forward * hoverY + Vector3.right * hoverX,
                Vector3.forward * (hoverY + 1) + Vector3.right * (hoverX + 1));
            Debug.DrawLine(Vector3.forward * hoverY + Vector3.right * (hoverX + 1),
                Vector3.forward * (hoverY + 1) + Vector3.right * hoverX);
        }
    }

    private void SpawnChessPiece(int prefabIndex, int x, int y) {
        GameObject go = Instantiate(piecePrefabs[prefabIndex], GetTileCenter(x, y), y > 3 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        ChessPiecePositions[x, y] = go.GetComponent<ChessPiece>();
        ChessPiecePositions[x, y].SetPosition(x, y);
        piecesActive.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void SpawnAllChessPieces()
    {
        SpawnChessPiece(0, 4, 0); // король
        SpawnChessPiece(1, 3, 0); // ферзь
        SpawnChessPiece(2, 2, 0); // слон
        SpawnChessPiece(2, 5, 0); // слон
        SpawnChessPiece(3, 1, 0); // конь
        SpawnChessPiece(3, 6, 0); // конь
        SpawnChessPiece(4, 0, 0); // ладья
        SpawnChessPiece(4, 7, 0); // ладья
        SpawnChessPiece(5, 0, 1);
        SpawnChessPiece(5, 1, 1);
        SpawnChessPiece(5, 2, 1);
        SpawnChessPiece(5, 3, 1);
        SpawnChessPiece(5, 4, 1);
        SpawnChessPiece(5, 5, 1);
        SpawnChessPiece(5, 6, 1);
        SpawnChessPiece(5, 7, 1);

        // Black
        SpawnChessPiece(6, 4, 7); // король
        SpawnChessPiece(7, 3, 7); // ферзь
        SpawnChessPiece(8, 2, 7); // слон
        SpawnChessPiece(8, 5, 7); // слон
        SpawnChessPiece(9, 1, 7); // конь
        SpawnChessPiece(9, 6, 7); // конь
        SpawnChessPiece(10, 0, 7); // ладья
        SpawnChessPiece(10, 7, 7); // ладья        
        SpawnChessPiece(11, 0, 6);
        SpawnChessPiece(11, 1, 6);
        SpawnChessPiece(11, 2, 6);
        SpawnChessPiece(11, 3, 6);
        SpawnChessPiece(11, 4, 6);
        SpawnChessPiece(11, 5, 6);
        SpawnChessPiece(11, 6, 6);
        SpawnChessPiece(11, 7, 6);
    }

    private void EndGame()
    {
        StartCoroutine(waitVictory());
    }

    IEnumerator waitVictory() {
        if (isLightTurn) {
            Debug.Log("Light wins!");
            LightVictoryMessage.SetActive(true);
            BoardHighlighting.Instance.HideHighlights();
            yield return new WaitForSeconds(3);
            LightVictoryMessage.SetActive(false);
        }
        else {
            Debug.Log("Dark wins!");
            DarkVictoryMessage.SetActive(true);
            BoardHighlighting.Instance.HideHighlights();
            yield return new WaitForSeconds(3);
            DarkVictoryMessage.SetActive(false);
        }

        foreach (GameObject go in piecesActive)
            Destroy(go);

        isLightTurn = true;
        SpawnAllChessPieces();
    }

}
