using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece {
    public override bool[,] AllowedMove()
    {
        bool[,] allowedMoves = new bool[8, 8];

        Move(CurrentX + 1, CurrentY, ref allowedMoves); // вперед
        Move(CurrentX - 1, CurrentY, ref allowedMoves); // назад
        Move(CurrentX, CurrentY - 1, ref allowedMoves); // влево
        Move(CurrentX, CurrentY + 1, ref allowedMoves); // вправо
        Move(CurrentX + 1, CurrentY - 1, ref allowedMoves); // вперед влево
        Move(CurrentX - 1, CurrentY - 1, ref allowedMoves); // назад влево
        Move(CurrentX + 1, CurrentY + 1, ref allowedMoves); // вперед вправо
        Move(CurrentX - 1, CurrentY + 1, ref allowedMoves); // назад вправо

        return allowedMoves;
    }
}