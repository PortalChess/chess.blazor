﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess.Common.Extensions;
using Chess.Common.System;
using Chess.Common.System.Metrics;

namespace Chess.Common.Movement
{
    // TODO: Unit-test against different move types
    public class MoveHandler
    {
        private readonly Common.Board _board;

        public MoveHandler(Common.Board board)
        {
            _board = board;
            RebuildMoveLists();
        }

        public MoveResult Move(Move move, Move validMove, BoardPiece boardPiece)
        {
            move.UpdateUnknownMoveType(validMove.MoveType);

            PreMoveActions(move);

            QuickMovePiece(move);

            var movePerformed = PostMoveActions(move, boardPiece);

            RebuildMoveLists();

            return movePerformed;
        }

        internal void QuickMovePiece(Move move)
        {
            var piece = _board[move.From];

            _board.ClearSquare(move.From);
            piece.MoveTo(move.To, move.MoveType);
            _board[move.To] = piece;

            if (move.MoveType == MoveType.TakeEnPassant)
            {
                var clear = new BoardLocation(move.To.File, move.From.Rank);
                _board.ClearSquare(clear);
            }


            RebuildMoveLists();
        }

        private void RebuildMoveLists()
        {
            Counters.Increment(CounterIds.Board.MovelistRebuildAll);

            Timers.Time(TimerIds.Board.RebuildMoveList, () =>
            {
                var tasks = new List<Task>();
                foreach (var boardPiece in _board.Pieces)
                {
                    tasks.Add(Task.Run(() => {
                        RebuildMoveListFor(boardPiece);
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        private void RebuildMoveListFor(BoardPiece boardPiece)
        {
            var all = MoveFactory.For[boardPiece.Piece.Name]().All(_board, boardPiece.Location);
            // TODO: MoveList still contains moves that would uncover check!
            boardPiece.SetAll(all.ToList());
        }

        private void PreMoveActions(Move move)
        {
            switch (move.MoveType)
            {
                case MoveType.Take:
                case MoveType.TakeEnPassant:
                    TakeSquare(move.To);
                    break;
                case MoveType.Promotion:
                    if (_board.IsNotEmptyAt(move.To)) TakeSquare(move.To);
                    break;
                case MoveType.Castle:
                    var rookMove = King.CreateRookMoveForCastling(move);
                    QuickMovePiece(rookMove);
                    break;
            }
        }

        private MoveResult PostMoveActions(Move move, BoardPiece boardPiece)
        {
            MoveResult result;
            switch (move.MoveType)
            {
                case MoveType.TakeEnPassant:
                    var takenLocation = new BoardLocation(move.To.File, move.From.Rank);
                    TakeSquare(takenLocation);
                    result = MoveResult.Enpassant(move);
                    break;
                case MoveType.Promotion:
                    Promote(move.To, boardPiece.Piece.Colour, move.PromotedTo);
                    result = MoveResult.Promotion(move);
                    break;
                default:
                    result = MoveResult.Success(move);
                    break;
            }
            NextTurn();
            return result;
        }

        private void TakeSquare(BoardLocation takenLocation)
        {
            _board[takenLocation].Taken(takenLocation);
            _board.ClearSquare(takenLocation);
        }

        private void Promote(BoardLocation at, Colours colour, PieceNames pieceName) 
            => _board[at] = new BoardPiece(at, new ChessPiece(colour, pieceName));

        private static MoveType DefaultMoveType(MoveType moveType, MoveType @default) 
            => moveType == MoveType.Unknown ? @default : moveType;

        private void NextTurn()
        {
            if (_board.WhoseTurn == Colours.White) _board.WhoseTurn = Colours.Black;
            else if(_board.WhoseTurn == Colours.Black) _board.WhoseTurn = Colours.White;
        }
    }
}