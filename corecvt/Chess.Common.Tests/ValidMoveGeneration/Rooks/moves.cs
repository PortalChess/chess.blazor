﻿using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Chess.Common.Tests.ValidMoveGeneration.Rooks
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        private RookMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new RookMoveGenerator();
        }
        [Test]
        public void can_move_in_four_horizontal_directions()
        {
            const string asOneChar = ".......k" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".P.R.P.." +
                                     "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var expected = BoardLocation.List("D6", "E5", "D4", "C5");

            var chessMoves = _generator.All(board, BoardLocation.At("D5")).Moves();

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }
    }
}