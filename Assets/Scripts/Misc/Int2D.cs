﻿using System;
using UnityEngine;

namespace MailSnail.Misc
{
    public struct Int2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Int2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }


        public static implicit operator Vector2Int(Int2D a)
        {
            return new Vector2Int(a.X, a.Y);
        }

        public static bool operator ==(Int2D a, Int2D b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Int2D a, Int2D b)
        {
            return !(a.X == b.X && a.Y == b.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Int2D other && Equals(other);
        }

        public bool Equals(Int2D other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}