using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Room class, will store data about each room.
    class Room
    {
        // Properties for the RoomId, and the passage in each direct. Also stores the ID of the threat and treasure in the room.
        public int RoomId { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int South { get; set; }
        public int West { get; set; }
        public int Treasure { get; set; }
        public int Threat { get; set; }

        // Constructor that sets all properties
        public Room(int roomId, int north, int east, int south, int west, int treasure, int threat)
        {
            RoomId = roomId;
            North = north;
            East = east;
            South = south;
            West = west;
            Treasure = treasure;
            Threat = threat;
        }

        // Gets the passage of the direction entered.
        public int GetPassage(string direction)
        {
            int passage = 0;

            switch (direction)
            {
                case "N":
                    passage = North;
                    break;
                case "E":
                    passage = East;
                    break;
                case "S":
                    passage = South;
                    break;
                case "W":
                    passage = West;
                    break;
            }

            return passage;

        }
    }
}
