﻿using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        private PawnValidMoveGenerator _pawnValidMoveGenerator;
        const int WhitePawnRank = 2;
        const int BlackPawnRank = 7;

        [SetUp]
        public void SetUp()
        {
            _pawnValidMoveGenerator = new PawnValidMoveGenerator();
        }
        [Test]
        public void can_move_one_or_two_squares()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Chess.Board.Files)
            {
                var whitePawn = BoardLocation.At(file, WhitePawnRank);
                var blackPawn = BoardLocation.At(file, BlackPawnRank);
                var whiteExpected = BoardLocation.List($"{whitePawn.File}{whitePawn.Rank + 1}",
                    $"{whitePawn.File}{whitePawn.Rank + 2}");
                var blackExpected = BoardLocation.List($"{blackPawn.File}{blackPawn.Rank - 1}",
                    $"{blackPawn.File}{blackPawn.Rank - 2}");

                AssertMovesContainsExpectedWithType(_pawnValidMoveGenerator.Moves(board, whitePawn),
                    whiteExpected, MoveType.Move);
                AssertMovesContainsExpectedWithType(_pawnValidMoveGenerator.Moves(board, blackPawn),
                    blackExpected, MoveType.Move);
            }
        }

        [Test]
        public void covers_nothing()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Chess.Board.Files)
            {
                Assert.That(_pawnValidMoveGenerator.Covers(board, BoardLocation.At(file, WhitePawnRank)), Is.Empty);
                Assert.That(_pawnValidMoveGenerator.Covers(board, BoardLocation.At(file, BlackPawnRank)), Is.Empty);
            }
        }

        [Test]
        public void has_no_takes()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Chess.Board.Files)
            {
                Assert.That(_pawnValidMoveGenerator.Takes(board, BoardLocation.At(file, WhitePawnRank)), Is.Empty);
                Assert.That(_pawnValidMoveGenerator.Takes(board, BoardLocation.At(file, BlackPawnRank)), Is.Empty);
            }
        }
    }
}
