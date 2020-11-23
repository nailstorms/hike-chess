using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece {

    public override bool[,] AllowedMove()
    {
        bool[,] allowedMoves = new bool[8, 8];

        int i;

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