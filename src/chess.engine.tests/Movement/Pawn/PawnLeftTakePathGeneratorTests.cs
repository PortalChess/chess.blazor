using System.Linq;
using board.engine.Actions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Pawn
{
    // TODO: Combine with Right Take tests
    [TestFixture]
    public class PawnLeftTakePathGeneratorTests : ChessPathGeneratorTestsBase
    {
        private PawnLeftTakePathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new PawnLeftTakePathGenerator();
        }
        [Test]
        public void PathsFrom_returns_empty_list_when_on_left_edge()
        {
            Assert.That(_gen.PathsFrom("A2".ToBoardLocation(), (int) Colours.White).Count(), Is.EqualTo(0));
            Assert.That(_gen.PathsFrom("H7".ToBoardLocation(), (int)Colours.Black).Count(), Is.EqualTo(0));
        }

        [Test]
        public void PathsFrom_returns_left_take()
        {
            var pieceLocation = "B2".ToBoardLocation();
            var paths = _gen.PathsFrom(pieceLocation, (int)Colours.White).ToList();

            var ep = new ChessPathBuilder().From("B2")
                .To("A3", (int)DefaultActions.TakeOnly)
                .Build();

            AssertPathContains(paths, ep, Colours.White);
            Assert.That(paths.Count(), Is.EqualTo(1));
        }


        [Test]
        public void PathsFrom_returns_pawn_promotions()
        {
            var startLocation = "B7".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int)Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(4));

            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Queen)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Rook)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Bishop)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Knight)
                .Build(), Colours.White);
        }
    }
}