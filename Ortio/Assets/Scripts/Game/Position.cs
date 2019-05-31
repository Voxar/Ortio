using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct Position : IEquatable<Position>
    {
        internal const int Width = 3;
        internal const int Height = 3 + 4;
        internal const int Sizes = 3;
        internal const int StridePerSize = Width * Height;

        public const int FirstIndex = 0;
        public const int LastIndex = Width * Height * Sizes;

        public int size;
        public int x;
        public int y;

        private static void ToPosition(Position destination, int size, int x, int y)
        {
            destination.size = size;
            destination.x = x;
            destination.y = y;
        }

        private static void ToPosition(out Position destination, int index)
        {
            int size = index / StridePerSize;
            destination.size = size;
            destination.x = index % Width;
            destination.y = (index - size * StridePerSize) / Width;
        }

        private static int ToIndex(Position position)
        {
            return ToIndex(position.size, position.x, position.y);
        }

        public static int ToIndex(int size, int x, int y)
        {
            return size * StridePerSize + y * Width + x;
        }

        public int index
        {
            get { return ToIndex(this); }
            set { ToPosition(out this, value); }
        }

        public Position(int index)
        {
            size = 0;
            x = 0;
            y = 0;
            ToPosition(out this, index);
        }

        public Position(int size, int xy)
        {
            this.size = size;
            this.x = xy % 3;
            this.y = xy / 3;
        }

        public Position(int size, int x, int y)
        {
            this.size = size;
            this.x = x;
            this.y = y;
        }

        public static Position InPlayerNest(int player, int size, int x)
        {
            return new Position(size, x, 3 + player);
        }

        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position)obj);
        }

        public bool Equals(Position other)
        {
            return index == other.index;
        }

        public override int GetHashCode()
        {
            return -1982729373 + index.GetHashCode();
        }


        public bool isNest => y >= 3;

        public int nestOwner => Math.Max(-1, y - 3);

        // the plain xy index ignoring size
        public int xy => y * Width + x;

        //deprecated
        public int player => nestOwner;
        public bool isStable => isNest;


        public static IEnumerable all => new BoardIndexEnumerator();

        public class BoardIndexEnumerator : IEnumerator, IEnumerable
        {
            Position _current = new Position();

            public object Current => _current;

            public BoardIndexEnumerator(int firstIndex = -1)
            {
                _current.index = firstIndex;
            }

            public bool MoveNext()
            {
                _current.index++;
                return _current.index < Position.LastIndex;
            }

            public void Reset()
            {
                _current.index = -1;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }
        }

    }

    
}


