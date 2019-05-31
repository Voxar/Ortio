using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct WinningCondition: IEquatable<WinningCondition>
    {
        readonly int player;
        readonly List<Position> positions;

        public int Player => player;
        public List<Position> Positions => positions;


        public WinningCondition(int player, List<Position> positions)
        {
            this.player = player;
            this.positions = positions;
        }

        public static List<WinningCondition> FromGameState(int[] board)
        {
            var result = new List<WinningCondition>();
            for (int i = 0; i < 4; i++)
            {
                result.AddRange(FromGameState(board, i));
            }
            return result;
        }

        internal static List<WinningCondition> FromGameState(int[] board, int player)
        {
            var result = new List<WinningCondition>();

            // horz or diag
            void check(int first, int stride)
            {
                var a = board[first];
                var b = board[first + stride];
                var c = board[first + stride + stride];
            
                if (a == player && b == player && c == player)
                {
                    var positions = new List<Position>();
                    positions.Add(new Position(first));
                    positions.Add(new Position(first + stride));
                    positions.Add(new Position(first + stride + stride));
                    result.Add(new WinningCondition(player, positions));
                }
            }

            const int s = Position.StridePerSize;
            const int x = 1;
            const int y = Position.Width;

            // rows
            for (int i = 0; i < 7; i += 3)
            {
                check(first: i, stride: x); // same size
                check(first: i + s, stride: x); // same size
                check(first: i + s + s, stride: x); // same size
                check(first: i, stride: s + x); // growing
                check(first: i + x + x, stride: s - x); // shrinking
            }

            // cols
            for (int i = 0; i < 3; i++)
            {
                check(first: i, stride: y); // same size
                check(first: i + s, stride: y); // same size
                check(first: i + s + s, stride: y); // same size
                check(first: i, stride: s + y);
                check(first: i + y + y, stride: s - y);
            }

            // cells
            for (int i = 0; i < 9; i++)
            {
                check(first: i, stride: s);
            }

            return result;

        }

        public override bool Equals(object obj)
        {
            return obj is WinningCondition && Equals((WinningCondition)obj);
        }

        public bool Equals(WinningCondition other)
        {
            return
                this.player.Equals(other.player) &&
                this.positions.All(other.positions.Contains);
        }

        public override int GetHashCode()
        {
            var hashCode = 257745382;
            hashCode = hashCode * -1521134295 + player.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Position>>.Default.GetHashCode(positions);
            return hashCode;
        }
    }
}