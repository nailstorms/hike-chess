using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece {
    public override bool[,] AllowedMove()
    {
        bool[,] allowedMoves = new bool[8, 8];

        // (северо-северо-запад)
        Move(CurrentX - 1, CurrentY + 2, ref allowedMoves);

        // (северо-северо-восток)
        Move(CurrentX + 1, CurrentY + 2, ref allowedMoves);

        // (юго-юго-запад)
        Move(CurrentX - 1, CurrentY - 2, ref allowedMoves);

        // (юго-юго-восток)
        Move(CurrentX + 1, CurrentY - 2, ref allowedMoves);


        // (запад-юго-запад)
        Move(CurrentX - 2, CurrentY - 1, ref allowedMoves);

        // (восток-юго-восток)
        Move(CurrentX + 2, CurrentY - 1, ref allowedMoves);

        // (запад-северо-запад)
        Move(CurrentX - 2, CurrentY + 1, ref allowedMoves);

        // (восток-северо-восток)
        Move(CurrentX + 2, CurrentY + 1, ref allowedMoves);

        return allowedMoves;
    }
}