using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece {
    public override bool[,] AllowedMove() {
        bool[,] allowedMoves = new bool[8,8];
        int[] enpassantMoves = BoardManager.Instance.EnPassantMove;

        ChessPiece firstTile, secondTile;

        if (isLight)
        {
            // диагональ, влево
            if (CurrentX != 0 && CurrentY != 7)
            {
                if(enpassantMoves[0] == CurrentX - 1 && enpassantMoves[1] == CurrentY + 1)
                    allowedMoves[CurrentX - 1, CurrentY + 1] = true;

                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX - 1, CurrentY + 1];
                if (firstTile != null && !firstTile.isLight)
                    allowedMoves[CurrentX - 1, CurrentY + 1] = true;
            }

            // диагональ, вправо
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (enpassantMoves[0] == CurrentX + 1 && enpassantMoves[1] == CurrentY + 1)
                    allowedMoves[CurrentX + 1, CurrentY + 1] = true;

                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX + 1, CurrentY + 1];
                if (firstTile != null && !firstTile.isLight)
                    allowedMoves[CurrentX + 1, CurrentY + 1] = true;
            }

            // вперед
            if (CurrentY != 7)
            {
                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY + 1];
                if (firstTile == null)
                    allowedMoves[CurrentX, CurrentY + 1] = true;
            }

            // вперед, первый ход
            if (CurrentY == 1)
            {
                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY + 1];
                secondTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY + 2];
                if (firstTile == null && secondTile == null)
                    allowedMoves[CurrentX, CurrentY + 2] = true;
            }
        }
        else
        {
            // диагональ, влево
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (enpassantMoves[0] == CurrentX - 1 && enpassantMoves[1] == CurrentY - 1)
                    allowedMoves[CurrentX - 1, CurrentY - 1] = true;

                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX - 1, CurrentY - 1];
                if (firstTile != null && firstTile.isLight)
                    allowedMoves[CurrentX - 1, CurrentY - 1] = true;
            }

            // диагональ, вправо
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (enpassantMoves[0] == CurrentX + 1 && enpassantMoves[1] == CurrentY - 1)
                    allowedMoves[CurrentX + 1, CurrentY - 1] = true;

                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX + 1, CurrentY - 1];
                if (firstTile != null && firstTile.isLight)
                    allowedMoves[CurrentX + 1, CurrentY - 1] = true;
            }

            // вперед
            if (CurrentY != 0)
            {
                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY - 1];
                if (firstTile == null)
                    allowedMoves[CurrentX, CurrentY - 1] = true;
            }

            // вперед, первый ход
            if (CurrentY == 6)
            {
                firstTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY - 1];
                secondTile = BoardManager.Instance.ChessPiecePositions[CurrentX, CurrentY - 2];
                if (firstTile == null && secondTile == null)
                    allowedMoves[CurrentX, CurrentY - 2] = true;
            }
        }

        return allowedMoves;
    }
}