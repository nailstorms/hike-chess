using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour {
    public int CurrentX{ set; get; }
    public int CurrentY{ set; get; }
    public bool isLight;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] AllowedMove() {
        return new bool[8,8];
    }

    public bool Move(int x, int y, ref bool[,] allowedMoves)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            ChessPiece enemyPiece = BoardManager.Instance.ChessPiecePositions[x, y];
            if (enemyPiece == null)
                allowedMoves[x, y] = true;
            else
            {
                if (isLight != enemyPiece.isLight)
                    allowedMoves[x, y] = true;
                return true;
            }
        }
        return false;
    }
}