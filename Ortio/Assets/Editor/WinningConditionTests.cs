using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class WinningConditionTests
{

    [Test]
    public void Nothing()
    {
        var state = new Game.State();

        var expectation = new List<Game.WinningCondition>();
        var reality = Game.WinningCondition.FromGameState(state.RawBoardData);

        Assert.That(reality, Is.EqualTo(expectation));
    }


    [Test]
    public void Row()
    {
        Test(true,
            // player, size, x, y
            1, 1, 0, 1,
            1, 1, 1, 1,
            1, 1, 2, 1
        );

        Test(true, // growing on a row
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 2, 2, 0
        );

        Test(true, // shrinking on a row
            0, 2, 0, 0,
            0, 1, 1, 0,
            0, 0, 2, 0
        );

        Test(true, // growing on a column
            0, 0, 1, 0,
            0, 1, 1, 1,
            0, 2, 1, 2
        );

        Test(true, // on one cell
            3, 0, 1, 1,
            3, 1, 1, 1,
            3, 2, 1, 1
        );

        Test(false, //not the same player
            0, 0, 1, 1,
            1, 1, 1, 1,
            0, 2, 1, 1
        );

        Test(false, //no wins
            0, 0, 0, 0,
            1, 0, 1, 0,
            2, 0, 2, 0
        );
    }

    public void Test(bool shouldSucceed, params int[] positions)
    {
        var state = new Game.State();
        var board = new int[Game.Position.LastIndex];

        int i = 0;
        var pos = new List<Game.Position>();
        var expectation = new List<Game.WinningCondition>();
        while (i < positions.Length)
        {
            var player = positions[i++];
            var p = new Game.Position(positions[i++], positions[i++], positions[i++]);
            state.SetPlayerAtPosition(player, p);
            pos.Add(p);

            if (pos.Count == 3)
            {
                expectation.Add(new Game.WinningCondition(player, pos));
                pos = new List<Game.Position>();
            }
        }


        var reality = Game.WinningCondition.FromGameState(state.RawBoardData);

        if (shouldSucceed)
        {
            Assert.That(reality, Is.EqualTo(expectation));
        }
        else
        {
            Assert.That(reality, !Is.EqualTo(expectation));
        }
    }

    [Test]
    public void Equality()
    {
        var p1 = new List<Game.Position>();
        p1.Add(new Game.Position(1));

        var p2 = new List<Game.Position>();
        p2.Add(new Game.Position(1));

        var a = new Game.WinningCondition(1, p1);
        var b = new Game.WinningCondition(1, p2);

        Assert.That(a, Is.EqualTo(b));
    }
}