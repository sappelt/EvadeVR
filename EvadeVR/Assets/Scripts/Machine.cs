using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Machine
    {
        public Vector3 Position { get; set; }
        public String Name { get; set; }
        public int Type { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Machine other = (Machine)obj;

            return Name.Equals(other) && Position.Equals(other);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode()^ Position.GetHashCode();
        }
    }
}
