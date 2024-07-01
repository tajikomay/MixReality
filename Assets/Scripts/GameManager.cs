using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject chickPrefab;
    public GameObject elephantPrefab;
    public GameObject giraffePrefab;
    public GameObject lionPrefab;
    public int rows;
    public int columns;
    public float tileSpacing;

    private GameObject[,] tiles;
    private GameObject[,] pieces;
    public Player[,] owners;
    private Player currentPlayer;

    public enum Player
    {
        Player1,
        Player2,
        None
    }

    void Start()
    {
        tiles = new GameObject[rows, columns];
        pieces = new GameObject[rows, columns];
        owners = new Player[rows, columns];
        currentPlayer = Player.Player1;
        Debug.Log("Player1's Turn");

        CreateField();
        PlacePieces();
    }

    void CreateField()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * tileSpacing, 0, i * tileSpacing);
                tiles[i, j] = Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }

    void PlacePieces()
    {
        InstantiatePiece(chickPrefab, 1, 1, Player.Player1);
        InstantiatePiece(chickPrefab, 1, 2, Player.Player2);
        InstantiatePiece(elephantPrefab, 0, 0, Player.Player1);
        InstantiatePiece(elephantPrefab, 2, 3, Player.Player2);
        InstantiatePiece(giraffePrefab, 2, 0, Player.Player1);
        InstantiatePiece(giraffePrefab, 0, 3, Player.Player2);
        InstantiatePiece(lionPrefab, 1, 0, Player.Player1);
        InstantiatePiece(lionPrefab, 1, 3, Player.Player2);
    }

    void InstantiatePiece(GameObject prefab, int row, int column, Player owner)
    {
        Vector3 position = tiles[row, column].transform.position + Vector3.up * 0.5f;
        GameObject piece = Instantiate(prefab, position, Quaternion.identity);
        pieces[row, column] = piece;
        owners[row, column] = owner;
        if (prefab == chickPrefab)
        {
            ChickManager chickManager = piece.GetComponent<ChickManager>();
            if (chickManager != null)
            {
                chickManager.SetOwner(owner);
            }
        }
        else if (prefab == giraffePrefab)
        {
            GiraffeManager giraffeManager = piece.GetComponent<GiraffeManager>();
            if (giraffeManager != null)
            {
                giraffeManager.SetOwner(owner);
            }
        }
        else if (prefab == elephantPrefab)
        {
            ElephantManager elephantManager = piece.GetComponent<ElephantManager>();
            if (elephantManager != null)
            {
                elephantManager.SetOwner(owner);
            }
        }
        else if (prefab == lionPrefab)
        {
            LionManager lionManager = piece.GetComponent<LionManager>();
            if (lionManager != null)
            {
                lionManager.SetOwner(owner);
            }
        }
    }

    public void SwitchTurn()
    {
        if (currentPlayer == Player.Player1)
        {
            currentPlayer = Player.Player2;
            Debug.Log("Player2's Turn");
        }
        else
        {
            currentPlayer = Player.Player1;
            Debug.Log("Player1's Turn");
        }
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsWithinField(Vector3 position)
    {
        return position.x >= 0 && position.x < columns &&
               position.z >= 0 && position.z < rows;
    }

    public GameObject GetPieceAtPosition(Vector3 position)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (pieces[i, j] != null && pieces[i, j].transform.position == position)
                {
                    return pieces[i, j];
                }
            }
        }
        return null;
    }

    public bool IsFriendlyPiece(GameObject piece, Player player)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return owners[i, j] == player;
                }
            }
        }
        return false;
    }

    public bool IsFriendlyPieceAt(Vector3 position, Player player)
    {
        GameObject pieceAtPosition = GetPieceAtPosition(position);
        if (pieceAtPosition != null)
        {
            return IsFriendlyPiece(pieceAtPosition, player);
        }
        return false;
    }

    public bool CheckWinCondition()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject piece = pieces[i, j];
                if (piece != null)
                {
                    if (piece.CompareTag("Lion") && owners[i, j] != currentPlayer)
                    {
                        Debug.Log(currentPlayer.ToString() + " wins!");
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
