using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class PositionKey
    {
        public int X { get; set; }
        public int Y { get; set;  }

        public PositionKey(int x, int y)
        {
            X = x;
            Y = y;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PositionKey other = (PositionKey)obj;

            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

    }
}
