using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class PositionTests
{
    [Test]
    public void TestConversion()
    {
        for (int size = 0; size < 3; size ++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var source = new Game.Position(size, x, y);
                    var destination = new Game.Position(source.index);

                    Assert.That(destination.size, Is.EqualTo(source.size));
                    Assert.That(destination.x, Is.EqualTo(source.x));
                    Assert.That(destination.y, Is.EqualTo(source.y));
                    Assert.That(destination.isNest, Is.EqualTo(source.isNest));
                    Assert.That(destination.player, Is.EqualTo(source.player));

                    Assert.That(destination.size, Is.EqualTo(size));
                    Assert.That(destination.x, Is.EqualTo(x));
                    Assert.That(destination.y, Is.EqualTo(y));

                    Assert.That(destination, Is.EqualTo(source));
                }
            }
        }
    }

    [Test]
    public void InNest()
    {
        for (int player = 0; player < 4; player++)
        {
            for (int size = 0; size < 3; size++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var source = Game.Position.InPlayerNest(player, size, x);
                    Assert.That(source.player, Is.EqualTo(player));
                    Assert.That(source.size, Is.EqualTo(size));
                    Assert.That(source.x, Is.EqualTo(x));
                    Assert.That(source.isNest, Is.True);
                }
            }
        }
    }

    [Test]
    public void EqualityInLists()
    {
        var a = new List<Game.Position>();
        var b = new List<Game.Position>();

        a.Add(new Game.Position(1));
        b.Add(new Game.Position(1));
        Assert.That(a, Is.EqualTo(b));

        a.Add(new Game.Position(2));
        b.Add(new Game.Position(2));
        Assert.That(a, Is.EqualTo(b));

        a.Add(new Game.Position(3));
        b.Add(new Game.Position(4));
        Assert.That(a, !Is.EqualTo(b));

    }

}
