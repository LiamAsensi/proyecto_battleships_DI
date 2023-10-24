using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsProto
{
    public struct Coords
    {
        public int x;
        public int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coords coord &&
                   x == coord.x &&
                   y == coord.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static bool operator ==(Coords left, Coords right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coords left, Coords right)
        {
            return !left.Equals(right);
        }

        public override string? ToString()
        {
            return $"[{x}, {y}]";
        }
    }
}
