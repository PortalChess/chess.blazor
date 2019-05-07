﻿using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Movement.ChessPieces.Pawn
{
    public class PawnLeftTakePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();

            var takeType = location.Y == Pieces.Pawn.EnPassantRankFor(forPlayer)
                ? MoveType.TakeEnPassant
                : MoveType.TakeOnly;

            var takeLocation = location.MoveForward(forPlayer).MoveLeft(forPlayer);

            if (takeLocation == null) return paths;
            if (takeLocation.Y != ChessGame.EndRankFor(forPlayer))
            {
                var move = BoardMove.Create(location, takeLocation, takeType);

                Path path = new Path();
                path.Add(move);
                paths.Add(path);
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = BoardMove.CreateUpdatePiece(location, takeLocation, promotionPieces);
                    paths.Add(new Path { move });
                }
            }

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation)location, forPlayer);
    }
}