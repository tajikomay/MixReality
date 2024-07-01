using UnityEngine;

public class LionManager : MonoBehaviour
{
    public float moveDistance;

    private GameManager.Player owner;
    private HighlightManager highlightManager;
    private GameManager gameManager;
    private bool isHighlighted = false;

    void Start()
    {
        highlightManager = FindObjectOfType<HighlightManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetOwner(GameManager.Player owner)
    {
        this.owner = owner;
    }

    public void ShowMoveOptions()
    {
        highlightManager.DestroyAllHighlights();

        Vector3 currentPosition = transform.position;

        Vector3[] moveDirections = new Vector3[]
        {
        transform.forward,    // 前
        -transform.forward,   // 後
        transform.right,      // 右
        -transform.right,     // 左
        transform.forward + transform.right,    // 右前
        transform.forward - transform.right,    // 左前
        -transform.forward + transform.right,   // 右後
        -transform.forward - transform.right    // 左後
        };

        foreach (Vector3 direction in moveDirections)
        {
            Vector3 highlightPosition = currentPosition + direction * moveDistance;

            // フィールド内にあり、味方の駒がいない場合にハイライトを生成
            if (gameManager.IsWithinField(highlightPosition))
            {
                GameObject pieceAtPosition = gameManager.GetPieceAtPosition(highlightPosition);
                if (pieceAtPosition == null || !gameManager.IsFriendlyPiece(pieceAtPosition, owner))
                {
                    GameObject highlight = highlightManager.CreateHighlight(highlightPosition);
                    highlight.transform.parent = this.transform;
                }
            }
        }

        isHighlighted = true;
    }

    void OnMouseDown()
    {
        if (gameManager.GetCurrentPlayer() == owner)
        {
            if (isHighlighted)
            {
                highlightManager.DestroyAllHighlights();
                isHighlighted = false;
            }
            else
            {
                ShowMoveOptions();
            }
        }
    }

    public void MoveToPosition(GameObject highlight, Vector3 targetPosition)
    {
        if (highlight.transform.parent == this.transform)
        {
            if (gameManager.IsWithinField(targetPosition))
            {
                GameObject pieceAtTarget = gameManager.GetPieceAtPosition(targetPosition);
                if (pieceAtTarget != null)
                {
                    // 敵の駒がいる場合、その駒を取り除く
                    if (!gameManager.IsFriendlyPiece(pieceAtTarget, owner))
                    {
                        Destroy(pieceAtTarget);

                        // 相手のライオンを取った場合、勝利条件をチェックする
                        if (pieceAtTarget.CompareTag("Lion"))
                        {
                            if (gameManager.CheckWinCondition())
                            {
                                transform.position = targetPosition;
                                highlightManager.DestroyAllHighlights();
                                isHighlighted = false;
                                Debug.Log("Game Over!");
                                return;
                            }
                        }
                    }
                }

                // 自分の駒を移動先に配置する
                transform.position = targetPosition;
                highlightManager.DestroyAllHighlights();
                isHighlighted = false;
            }
        }
        gameManager.SwitchTurn();
    }
}
