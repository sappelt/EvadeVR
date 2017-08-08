using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ItemClickedEventArgs
    {
        public Item Item {get; private set; }

        public ItemClickedEventArgs(Item item)
        {
            Item = item;
        }
    }
}
