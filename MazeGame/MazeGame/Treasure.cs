using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Treasure class, inheriting from Item
    class Treasure : Item
    {
        // Property for the value of the treasure with default getter and setter
        public int Value { get; set; }

        // Constructor. Also calls the base class constructor
        public Treasure(int itemId, string name, int value) : base(itemId, name)
        {
            Value = value;
        }
    }
}
