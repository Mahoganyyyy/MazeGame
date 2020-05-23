using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Class for passages. Stores PassageID, IsExit, and the two rooms which it connects
    class Passage
    {
        public int PassageId { get; set; }
        public bool IsExit { get; set; }
        public int Room1 { get; set; }
        public int Room2 { get; set; }

        // Constructor which accepts all properties and sets them
        public Passage(int passageId, bool isExit, int r1, int r2)
        {
            PassageId = passageId;
            IsExit = isExit;
            Room1 = r1;
            Room2 = r2;
        }

        // Gets destination room based on current room of player. Since a passage only has an entrance and an exit, the destination will always be the opposite of the current room. 
        public int GetNewRoom(int currentRoom) 
        {
            if (currentRoom == Room1)
                return Room2;
            else
                return Room1;
        }
    }
}
