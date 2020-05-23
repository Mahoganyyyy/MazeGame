using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // Class for threats. Inherits from Item
    class Threat : Item
    {
        // Properties for the action which kills the threat, and an incorrect action which doesn't kill the threat
        public string CorrectAction { get; set; }
        public string IncorrectAction { get; set; }

        // Constructor which accepts all properties, also calls the base class constructor
        public Threat(int itemId, string name, string correctAction, string incorrectAction) : base(itemId, name)
        {
            CorrectAction = correctAction;
            IncorrectAction = incorrectAction;
        }

    }
}

