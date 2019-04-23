﻿using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Chess.Common.Tests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        private KnightMoveGenerator _knightMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightMoveGenerator = new KnightMoveGenerator();
        }
        [Test]
        public void all_moves_generated()
        {
            var asOneChar =
                    ".......k" +
                    "........" +
                    "........" +
                    "........" +
                    "...N...." +
                    "........" +
                    "........" +
                    ".......K";
            var expected = BoardLocation.List("E6", "F5", "F3", "E2", "C2", "B3", "B5", "C6");
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _knightMoveGenerator.All(board, BoardLocation.At("D4")).Moves().ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Move);
            AssertAllMovesAreOfType(moves, MoveType.Move);
        }
    }
}