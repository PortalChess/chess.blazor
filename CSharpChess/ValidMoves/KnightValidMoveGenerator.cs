using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using static CSharpChess.Chess.Rules;

namespace CSharpChess.ValidMoves
{
    public class KnightValidMoveGenerator : ValidMoveGeneratorBase
    {
        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            return ValidMoves(board, at)
                .Concat(ValidTakes(board, at))
                .Concat(ValidCovers(board, at));
        }

        private IEnumerable<ChessMove> AddMoveIf(ChessBoard board, BoardLocation at,
                Func<ChessBoard, BoardLocation, BoardLocation, bool> predicate,
                MoveType moveType)
            => MovementTransformation.ApplyTo(Knights.MoveMatrix, at)
                .Where(to => predicate(board, at, to))
                .Select(m => new ChessMove(at, m, moveType));

        private IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at) 
            => AddMoveIf(board, at, (b, f, t) => b.IsEmptyAt(t), MoveType.Move);

        private IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) 
            => AddMoveIf(board, at, (b, f, t) => b.CanTakeAt(t, b[f].Piece.Colour), MoveType.Take);

        private IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at)
            => AddMoveIf(board, at, (b, f, t) => b.IsCoveringAt(t, b[f].Piece.Colour), MoveType.Cover);

    }
}