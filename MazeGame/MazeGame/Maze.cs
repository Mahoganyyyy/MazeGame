using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // This is the Maze class. This will store all data regarding the maze in arrays.
    class Maze
    {
        // Array of Room objects
        public Room[] Rooms { get; set; }

        // Array of Passage objects
        public Passage[] Passages { get; set; }

        // Array of Threat objects
        public Threat[] Threats { get; set; }

        // Array of Treasure objects
        public Treasure[] Treasures { get; set; }

        // Constructor. This will be called when creating a new Maze object. Accepts all the properties and sets them.
        public Maze(Room[] rooms, Passage[] passages, Threat[] threats, Treasure[] treasures)
        {
            Rooms = rooms;
            Passages = passages;
            Threats = threats;
            Treasures = treasures;
        }
    }
}
