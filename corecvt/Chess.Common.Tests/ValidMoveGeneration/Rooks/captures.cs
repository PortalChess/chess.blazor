﻿using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Chess.Common.Tests.ValidMoveGeneration.Rooks
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        private RookMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new RookMoveGenerator();
        }
        [Test]
        public void can_take_in_four_directions()
        {
            //                        ABCDEFGH
            const string asOneChar = ".......k" + // 8
                                     "...p...." + // 7
                                     "........" + // 6
                                     ".p.R.p.." + // 5
                                     "........" + // 4
                                     "...p...." + // 3
                                     "........" + // 2
                                     ".......K";  // 1

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var expectedTakes = BoardLocation.List("D7", "F5", "D3", "B5");

            var chessMoves = _generator.All(board,BoardLocation.At("D5")).Takes().ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}