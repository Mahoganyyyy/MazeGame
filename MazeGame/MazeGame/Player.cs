using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Player class that stores information about the player
    class Player
    {
        // Private wealth variable. This is needed because I am using some code in the setter. 
        private int _wealth;

        public int CurrentRoomNo { get; set; }
        public int Wealth {
            get {
                // Gets the wealth value
                return _wealth;    
            }
            set {
                // Checks if the value entered is less than zero. If it is, wealth will be set to 0.
                // This is because wealth cannot be negative.

                if (value < 0)
                {
                    _wealth = 0;
                }
                else 
                {
                    _wealth = value;
                }
                Console.WriteLine("Current Wealth: {0}", _wealth);
            } 
        }

        // Constructor that accepts 1 parameter, as the wealth is set to 0 by default
        public Player(int roomNo)
        {
            CurrentRoomNo = roomNo;
            _wealth = 0;
        }

        // Changes the current room of the player
        public void GoToRoom(int roomNo) 
        {
            CurrentRoomNo = roomNo;
        }
    
    }
}
