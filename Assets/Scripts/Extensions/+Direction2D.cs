using MailSnail.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSnail.Misc
{
    public static partial class Extension
    {
        public static int GetArrayIndex(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.North:
                    return 0;
                case Direction2D.East:
                    return 1;
                case Direction2D.South:
                    return 2;
                case Direction2D.West:
                    return 3;
            }

            return -1;
        }

        public static Int2D GetOffset(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.North:
                    return new Int2D(0, 1);
                case Direction2D.East:
                    return new Int2D(1, 0);
                case Direction2D.South:
                    return new Int2D(0, -1);
                case Direction2D.West:
                    return new Int2D(-1, 0);
            }

            return new Int2D(0, 0);
        }

        public static Int2D OffsetDirection(this Direction2D direction, Int2D origin)
        {
            return direction.OffsetDirection(origin, false);
        }

        public static Int2D OffsetDirection(this Direction2D direction, Int2D origin, bool inverse)
        {
            int sign = inverse ? -1 : 1;

            switch (direction)
            {
                case Direction2D.North:
                    return new Int2D(origin.X, origin.Y + 1 * sign);
                case Direction2D.South:
                    return new Int2D(origin.X, origin.Y - 1 * sign);
                case Direction2D.East:
                    return new Int2D(origin.X + 1 * sign, origin.Y);
                case Direction2D.West:
                    return new Int2D(origin.X - 1 * sign, origin.Y);
                default:
                    break;
            }

            return origin;
        }

        public static Direction2D Inverse(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.North:
                    return Direction2D.South;
                case Direction2D.East:
                    return Direction2D.West;
                case Direction2D.South:
                    return Direction2D.North;
                case Direction2D.West:
                    return Direction2D.East;
            }

            throw new NotImplementedException();
        }
    }
}
