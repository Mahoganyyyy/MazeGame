using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Item class. This will be inherited by Threat and Treasure
    class Item
    {
        // ItemId and Name property with default getter and setter
        public int ItemId { get; set; }
        public string Name { get; set; }

        // Constructor for both properties
        public Item(int itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }
    }
}
