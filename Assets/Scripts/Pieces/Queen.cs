using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece {
    public override bool[,] AllowedMove()
    {
        bool[,] allowedMoves = new bool[8, 8];

        int i, j;

        // вперед влево
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8) break;

            if (Move(i, j, ref allowedMoves)) break;
        }

        // вперед вправо
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8) break;

            if (Move(i, j, ref allowedMoves)) break;
        }

        // назад влево
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0) break;

            if (Move(i, j, ref allowedMoves)) break;
        }

        // назад вправо
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0) break;

            if (Move(i, j, ref allowedMoves)) break;
        }

        // вправо
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8) break;

            if (Move(i, CurrentY, ref allowedMoves)) break;
        }

        // влево
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0) break;

            if (Move(i, CurrentY, ref allowedMoves)) break;
        }

        // вперед
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8) break;

            if (Move(CurrentX, i, ref allowedMoves)) break;
        }

        // назад
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0) break;

            if (Move(CurrentX, i, ref allowedMoves)) break;
        }

        return allowedMoves;
    }
}