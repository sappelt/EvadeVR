using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ItemDetailsHandler : MonoBehaviour
    {
        public delegate void ItemDetailsClickedHandler(object sender, ItemClickedEventArgs e);
        public event ItemDetailsClickedHandler ItemClicked;
        public Item Item;
        public ItemDetailsHandler(Item item)
        {
            Item = item;
        }

        private void OnMouseDown()
        {
            ItemClicked(this.gameObject, new ItemClickedEventArgs(Item));
        }
    }
}
