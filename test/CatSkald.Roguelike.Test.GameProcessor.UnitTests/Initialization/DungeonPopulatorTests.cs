﻿using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.GameProcessor.Initialization;
using NUnit.Framework;
using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests.Initialization
{
    public class DungeonPopulatorTests
    {
        [Test]
        public void Fill_UpstairsAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 7);
            var points = new[] { new Point(2, 2), new Point(3, 3) };
            foreach (var p in points)
                dungeon[p].Type = XType.Empty;

            populator.Fill(dungeon, new DungeonParameters());

            Assert.That(dungeon,
                Has.Exactly(1).Matches<Cell>(cell =>
                    cell.Type == XType.StairsUp
                    && points.Contains(cell.Location)));
        }

        [Test]
        public void Fill_DownstairsAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 7);
            var points = new[] { new Point(2, 2), new Point(3, 3) };
            foreach (var p in points)
                dungeon[p].Type = XType.Empty;

            populator.Fill(dungeon, new DungeonParameters());

            Assert.That(dungeon,
                Has.Exactly(1).Matches<Cell>(cell => 
                    cell.Type == XType.StairsDown 
                    && points.Contains(cell.Location)));
        }

        [Test]
        public void Fill_CreaturesAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 7);
            var points = new[] { new Point(2, 2), new Point(3, 3) };
            foreach (var p in points)
                dungeon[p].Type = XType.Empty;

            populator.Fill(dungeon, new DungeonParameters());

            var expectedLocation = dungeon
                .Single(cell => cell.Type == XType.StairsUp)
                .Location;

            Assert.That(dungeon.Character,
                Is.Not.Null
                .And.With.Property(nameof(Character.Location))
                .EqualTo(expectedLocation));
        }

        [Test]
        public void Fill_CharacterAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 7);
            var points = new[] { new Point(2, 2), new Point(3, 3) };
            foreach (var p in points)
                dungeon[p].Type = XType.Empty;

            populator.Fill(dungeon, new DungeonParameters());

            var expectedLocation = dungeon
                .Single(cell => cell.Type == XType.StairsUp)
                .Location;

            Assert.That(dungeon.Character,
                Is.Not.Null
                .And.With.Property(nameof(Character.Location))
                .EqualTo(expectedLocation));
        }
    }
}
