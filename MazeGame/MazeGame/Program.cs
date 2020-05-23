using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // firstRun is used to track whether it is the first time the game is running. This way, it asks for the config file straight away
            bool firstRun = true;

            // This stores the file path of the configuration file, incase the player wants to replay the current config
            string configFilePath = "";

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("  __  __                  _____                      ");
            Console.WriteLine(" |  \\/  |                / ____|                     ");
            Console.WriteLine(" | \\  / | __ _ _______  | |  __  __ _ _ __ ___   ___ ");
            Console.WriteLine(" | |\\/| |/ _` |_  / _ \\ | | |_ |/ _` | '_ ` _ \\ / _ \\");
            Console.WriteLine(" | |  | | (_| |/ /  __/ | |__| | (_| | | | | | |  __/");
            Console.WriteLine(" |_|  |_|\\__,_/___\\___|  \\_____|\\__,_|_| |_| |_|\\___|");
            Console.WriteLine("                                                      ");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("© Old World Phunne - 2020 // Game by Scott Kinchlea");
            Console.WriteLine("");

            // This puts the game in a constant loop, allowing the game to be restarted. 
            while (true)
            {
                // This will only run on the first run of the game
                if (firstRun)
                {
                    // Gets the file path from the user
                    Console.Write("Please enter the full path of the configuration file: ");
                    configFilePath = Console.ReadLine();
                    
                    // Runs ConfigureMaze(), passing in the file path. This method returns the generated maze object, or null if it didn't generate.
                    Maze maze = ConfigureMaze(configFilePath);
                    
                    // Setting firstRun to false so that this doesn't run again
                    firstRun = false;

                    // Runs StartGame() with the maze object as a paremeter. This actually begins the game.
                    StartGame(maze);
                }
                else
                {
                    // Prints a menu
                    Console.WriteLine("---- Select an Option ----");
                    Console.WriteLine("1. Load new configuration");
                    Console.WriteLine("2. Replay");
                    Console.WriteLine("3. Quit game");
                    Console.Write("Selection: ");
                    int selection = 0;

                    // This error handling statement ensures that the user inputs a number. 
                    try
                    {
                        // Tries to convert the input into an integer
                        selection = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        // If a number isnt entered, this runs. It then restarts the loop, showing the menu again. 
                        Console.WriteLine("Please enter a number");
                        continue;
                    }
                    
                    if(selection == 1)
                    {
                        // Gets new config file and starts the game as it did for the first run
                        Console.Write("Please enter the full path of the configuration file: ");
                        configFilePath = Console.ReadLine();
                        Maze maze = ConfigureMaze(configFilePath);
                        StartGame(maze);
                    }
                    else if(selection == 2)
                    {
                        // Restarts the game with the current configuration
                        Maze maze = ConfigureMaze(configFilePath);
                        StartGame(maze);
                    }
                    else
                    {
                        // Breaks out of the main loop, ending the program.
                        break;
                    }
                }
            }
        }

        public static void StartGame(Maze maze)
        {
            // The maze will be null if it failed to generate. This will allow the user to enter a different config or quit the game
            if (maze == null)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                // Begins the game
                PlayGame(maze);
            }
        }

        public static void PlayGame(Maze maze)
        {
            // Clears the console and displays a message for the player
            Console.Clear();
            Console.WriteLine("---- Game Started. You are in a random room inside the maze. ----");
            Console.WriteLine("");

            // Initialises variables allowing for random number generation
            int numberOfRooms = maze.Rooms.Length + 1;
            Random r = new Random();

            // Booleans deciding behaviour. gameOver is set to true when the exit has been found. canDropCoin is true when the users wealth is over 0, and the room has no treasure
            bool gameOver = false;
            bool canDropCoin = false;

            // Creates an instance of player. Puts player in a random room. r.Next generates a random number between 1 and the number of rooms in the maze.
            Player player = new Player(r.Next(1, numberOfRooms));

            // Calls InsideRoom() with maze and player as parameters
            InsideRoom(maze, player);

            // This loop will run when gameOver is false
            while (!gameOver)
            {
                // Each iteration of the loop starts with canDropCoin being set to false, as it is checked further along whether it should be set to true
                canDropCoin = false;
                
                Console.WriteLine("You can either go North, East, South, or West (N/E/S/W)");

                // Checks if the current room has no treasure, and if the players wealth is above 0.
                if (maze.Rooms[player.CurrentRoomNo - 1].Treasure == 0 && player.Wealth > 0)
                {
                    // Set canDropCoin to true
                    canDropCoin = true;

                }

                // Displays message saying the user can pick up a coin, only if canDropCoin is true
                if (canDropCoin)
                {
                    Console.WriteLine("Or you can drop a coin (D)");
                }

                Console.Write("Selection: ");

                // Gets the user input and converts it to upper case
                string direction = Console.ReadLine().ToUpper();

                // Gets the index of the current room in the room array
                int roomIndex = player.CurrentRoomNo - 1;

                // Gets the room object of the current room from the room array
                Room thisRoom = maze.Rooms[roomIndex];

                // If the user input is one of the directional options
                if (direction == "N" || direction == "E" || direction == "S" || direction == "W")
                {
                    // Gets the passage number of the direction selected.
                    int passage = thisRoom.GetPassage(direction);

                    // If the passage number is not equal to 0
                    if (passage != 0)
                    {
                        // Checks if the passage is an exit
                        if (maze.Passages[passage - 1].IsExit)
                        {
                            // Sets gameOver to true, ending the game
                            gameOver = true;
                        }
                        else
                        {
                            // Calls the GoToRoom() method on player. This uses the GetNewRoom() method of the passage to get the room that the passage leads to
                            player.GoToRoom(maze.Passages[passage - 1].GetNewRoom(player.CurrentRoomNo));

                            // Calls InsideRoom 
                            InsideRoom(maze, player);
                        }
                    }
                    else
                    {
                        // If the passage number is 0, this message will be displayed, and the loop will restart.
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("You can't go this way. ");
                        Console.WriteLine("-----------------------");
                        continue;
                    }
                }
                // If the user inputs 'D' to drop a coin
                else if (direction == "D")
                {
                    // If canDropCoin is true
                    if (canDropCoin)
                    {
                        // Sets the treasure attribute of the current room to be a coin. The coin will always be the last treasure object in the array, so I use Treasure.Length to get the treasure id of the coin
                        maze.Rooms[player.CurrentRoomNo - 1].Treasure = maze.Treasures.Length;
                        
                        // Removes 1 wealth from the player
                        player.Wealth -= 1;
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("Coin dropped. Wealth -1.");
                        Console.WriteLine("-----------------------");
                    }
                    // If the player cannot drop a coin
                    else
                    {
                        // Tells the player they can't do that
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("You can't do that. ");
                        Console.WriteLine("-----------------------");
                    }
                }
                // If anything other than NESWD is entered 
                else
                {
                    // Tells player to enter a valid direction, restarts the loop
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Please enter a valid direction.");
                    Console.WriteLine("-----------------------");
                    continue;
                }
            }

            // Runs when gameOver is true and the loop is over. 
            // Displays the wealth of the player, and prompts the user to press any key. 
            Console.WriteLine("-----------------------");
            Console.WriteLine("You found the exit!");
            Console.WriteLine("Final Wealth: {0}", player.Wealth);
            Console.WriteLine("-----------------------");
            Console.WriteLine("Press any key to continue...");

            // Clears the console after the user presses a key. This leads to the menu being displayed.
            Console.ReadKey();
            Console.Clear();
        }

        // This method runs whenever a user enters a room. It deals with threats and treasures inside each room.
        public static void InsideRoom(Maze maze, Player player) 
        {
            // Room index is the position of the current room in the rooms array in the maze object. This is got by taking one away from the room id. 
            int roomIndex = player.CurrentRoomNo - 1;

            // This gets the actual room object of the current room.
            Room currentRoom = maze.Rooms[roomIndex];

            // Gets the threat id of the threat in the current room
            int threatNumber = currentRoom.Threat;

            // Gets the treasure id of the treasure in the current room
            int treasureNumber = currentRoom.Treasure;

            // To be used to store the action that kills a threat
            string correctOption = "";

            Console.WriteLine("~~ You have entered Room {0} ~~", player.CurrentRoomNo);

            // If there is a threat, this will run. The threat number is 0 if there is not a threat in the room. 
            if (threatNumber != 0) 
            {
                // Gets the threat object of the threat in the current room. 
                Threat threat = maze.Threats[threatNumber - 1];

                // Boolean to store if the threat is alive, will be used in a loop
                bool threatDefeated = false;
                
                // Generates a number that will be either 1 or 2. This randomises the order in which possible actions are printed.
                // This means that the correct action isn't always displayed first. 
                Random r = new Random();
                int randomNum = r.Next(0, 2);

                Console.WriteLine("-----------------------");
                Console.WriteLine("Oh no! There is a {0} in here!", threat.Name);
                Console.WriteLine("What will you do?");
                Console.WriteLine("-----------------------");

                // Loop will run whilst the threat is alive
                while (!threatDefeated) 
                {
                    string userInput = "";

                    // This will get a valid input - either 1 or 2
                    while (userInput != "1" && userInput != "2")
                    {
                        // This is what randomises the order of the actions. It changes depending on if 1 or 2 was generated above.
                        if (randomNum == 1)
                        {
                            // The correct option changes depending on which order the threats are displayed, this stores that in a variable.
                            correctOption = "1";
                            Console.Write("{0} (1) or {1} (2): ", threat.CorrectAction, threat.IncorrectAction);
                        }
                        else
                        {
                            correctOption = "2";
                            Console.Write("{0} (1) or {1} (2): ", threat.IncorrectAction, threat.CorrectAction);
                        }

                        // Gets the user input
                        userInput = Console.ReadLine();

                        // Checks if the input is 1 or 2, if not, displays a message and restarts this loop.
                        if (userInput != "1" && userInput != "2")
                        {
                            Console.WriteLine("Please enter 1 or 2.");
                        }
                    }

                    // If the user selected the correct options
                    if (userInput == correctOption)
                    {
                        // Displays a message saying it died
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("The {0} died! You can now move on!", threat.Name);
                        Console.WriteLine("-----------------------");

                        // Removes the threat from the room by setting the threat to 0
                        currentRoom.Threat = 0;

                        // Marks threatDefeated as true, ending this loop
                        threatDefeated = true;
                    }
                    else 
                    {
                        // Displays a message saying that it survived
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("The {0} survived and stole some of your wealth! Try something else!", threat.Name);
                        Console.WriteLine("-----------------------");

                        // Subtracts 25 wealth from the player
                        player.Wealth -= 25;

                        // Sets threatDefeated to false to ensure the loop restarts.
                        threatDefeated = false;
                    }
                }
            }

            // Checks if there is treasure in the room. treasureNumber will be 0 if there is no treasure.
            if (treasureNumber != 0)
            {
                // Displays a message telling the player which treasure is in the room.
                Console.WriteLine("-----------------------");
                Console.WriteLine("You found a {0}!", maze.Treasures[treasureNumber - 1].Name);
                Console.WriteLine("-----------------------");

                // Initialising a variable to store the selected action
                string action = "";

                // Loop runs when action is not equal to true
                while (action != "true") {
                    // Prompts the player to either pick up or leave the treasure
                    Console.Write("Pick it up? (y/n): ");

                    // Gets the input and converts it to lower case
                    action = Console.ReadLine().ToLower();

                    if (action == "y")
                    {
                        // If the user selects yes, the wealth value of the treasure will be added to the players wealth
                        Console.WriteLine(" + Added {0} wealth", maze.Treasures[treasureNumber - 1].Value);
                        player.Wealth += maze.Treasures[treasureNumber - 1].Value;

                        // Removes the treasure from the room
                        currentRoom.Treasure = 0;

                        // Sets action to true, stopping the loop
                        action = "true";
                    }
                    else if (action == "n")
                    {
                        // If the user selects no, the treasure remains the in the room, and the loop ends.
                        action = "true";
                    }
                    else
                    {
                        // If the user selects anything other than y or n, the user will be told of the error, and the loop will restart

                        Console.WriteLine("Please enter either y or n. ");

                        action = "";
                    }
                }
            }
        }

        public static Maze ConfigureMaze(string configFilePath)
        {
            // Initialising lists of the different classes I need to initialise the maze
            List<Room> rooms = new List<Room>();
            List<Passage> passages = new List<Passage>();
            List<Threat> threats = new List<Threat>();
            List<Treasure> treasures = new List<Treasure>();

            // Creating an array of strings, and calling ValidateConfig() with the configFilePath as a parameter
            string[] errorMessage = ValidateConfig(configFilePath);

            // If errorMessage is equal to null, returns null
            if (errorMessage == null) {
                return null;
            }

            // If the length of the errorMessage array is 0
            if (errorMessage.Length == 0)
            {
                // This will loop through all lines of the file specified 
                foreach (string line in File.ReadLines(configFilePath))
                {
                    // String variable to be used to edit the current line
                    string thisLine;

                    // These if statements will check which type of line it is, it will then create an object of the relevent type, and add it to the relevant list as
                    // initialised above. For more information regarding the configuration file, see the design document. 
                    if (line.StartsWith("room-"))
                    {
                        thisLine = line.Replace("room-", "");
                        // This will split up the line by comma, and convert all items in the array to an integer, and then convert those to another array. 
                        int[] configArray = thisLine.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        rooms.Add(new Room(configArray[0], configArray[1], configArray[2], configArray[3], configArray[4], configArray[5], configArray[6]));
                    }
                    else if (line.StartsWith("passage-"))
                    {
                        thisLine = line.Replace("passage-", "");
                        int[] passageArray = thisLine.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        passages.Add(new Passage(passageArray[0], passageArray[1] != 0, passageArray[2], passageArray[3]));
                    }
                    else if (line.StartsWith("item-"))
                    {
                        thisLine = line.Replace("item-", "");
                        string[] itemArray = thisLine.Split(',');
                        if (itemArray[1] == "treasure")
                        {
                            treasures.Add(new Treasure(Convert.ToInt32(itemArray[0]), itemArray[2], Convert.ToInt32(itemArray[3])));
                        }
                        else if (itemArray[1] == "threat") 
                        {
                            threats.Add(new Threat(Convert.ToInt32(itemArray[0]), itemArray[2], itemArray[4], itemArray[5]));
                        }
                    }
                }

                // Adding coin treasure object. This will always be the last item in the treasure array. 
                treasures.Add(new Treasure(treasures.Count + 1, "Coin", 1));

                // Creates the maze object with the relevant lists converted to arrays.
                Maze maze = new Maze(rooms.ToArray(), passages.ToArray(), threats.ToArray(), treasures.ToArray());

                return maze;
            }
            // If the errorMessages array isnt empty, that means that config file wasnt configured correctly, and there are error messages in the array. 
            else
            {
                Console.WriteLine("Error generating maze. Please check config file.");

                // This will remove all duplicate items from the array
                errorMessage = errorMessage.Distinct().ToArray();

                // Loops over the errorMessages array
                foreach (string error in errorMessage) 
                {
                    // Adds each error to the end of the file
                    using (StreamWriter sw = File.AppendText(configFilePath))
                    {
                        sw.WriteLine("");
                        sw.WriteLine(error);
                    }
                }

                // Returns null, so that the game will not start
                return null;
            }

        }

        // This method validates the configuration file and makes sure that it will generate a functional maze. 
        public static string[] ValidateConfig(string configPath)
        {
            // Initialises various lists to be used throughout the method.
            List<string> rooms = new List<string>();
            List<string> passages = new List<string>();
            List<string> threats = new List<string>();
            List<string> treasures = new List<string>();
            List<string> items = new List<string>();
            List<string> errorMessages = new List<string>();
            List<int> roomIds = new List<int>();
            List<int> passageIds = new List<int>();
            List<int> threatIds = new List<int>();
            List<int> treasureIds = new List<int>();
            bool roomUnique, passageUnique, threatUnique, treasureUnique, exitPassage;

            // This variable will remain false until an exit passage has been found. 
            exitPassage = false;

            // This will try to iterate over the file specified, it will throw an error if the file is not found, which will be caught and dealth with
            try
            {
                // This will set up various lists as defined above. 
                foreach (string line in File.ReadLines(configPath)) 
                {
                    if (line.StartsWith("room-"))
                    {
                        rooms.Add(line.Replace("room-", ""));
                        roomIds.Add(Convert.ToInt32(line.Replace("room-", "").Split(',')[0]));
                    }
                    else if (line.StartsWith("passage-")) 
                    {
                        passages.Add(line.Replace("passage-", ""));
                        passageIds.Add(Convert.ToInt32(line.Replace("passage-", "").Split(',')[0]));
                    }
                    else if (line.StartsWith("item-"))
                    {
                        string item = line.Replace("item-", "");
                        items.Add(item);
                        string[] itemArray = item.Split(',');
                        if(itemArray[1] == "treasure")
                        {
                            threats.Add(item);
                            threatIds.Add(Convert.ToInt32(itemArray[0]));
                        }
                        else if (itemArray[1] == "threat")
                        {
                            treasures.Add(item);
                            treasureIds.Add(Convert.ToInt32(itemArray[0]));
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // If the file hasnt been found, the program will throw a FileNotFoundException. This catches that and tells the user the file hasnt been found.
                Console.WriteLine("Couldn't find file. Please try again.");

                // Returns null to the caller, which will cause the menu to be displayed
                return null;
            }

            // If less than two rooms have been specified, add relevant error to errorMessage array
            if (rooms.Count < 2) 
            {
                errorMessages.Add("Please specify at least 2 rooms");
            }

            // If no passages have been specified, add relevant error to errorMessage array
            if (passages.Count == 0)
            {
                errorMessages.Add("Please specify at least 1 passage");
            }

            // Iterates over rooms
            foreach (string room in rooms) 
            {
                string[] splitRoom = room.Split(',');
                bool correctLength = true;

                // If room has ID of 0, add relevant error to errorMessage array
                if (room.StartsWith("0")) {
                    errorMessages.Add("Room ID may not be 0");
                }

                // If length of room parameters isnt equal to 7, add relevant error to errorMessage array
                if (splitRoom.Length != 7)
                {
                    errorMessages.Add("Please ensure all rooms have 7 parameters");
                    correctLength = false;
                }

                if (correctLength)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        // If using a passage that doesnt exist, add relevant error to errorMessage array
                        if (Convert.ToInt32(splitRoom[i]) != 0 && !passageIds.Contains(Convert.ToInt32(splitRoom[i])))
                        {
                            errorMessages.Add("Passage with ID " + splitRoom[i] + " does not exist.");
                        }
                    }

                    // If using a treasure, add relevant error to errorMessage array
                    if (Convert.ToInt32(splitRoom[5]) != 0 && !treasureIds.Contains(Convert.ToInt32(splitRoom[5])))
                    {
                        errorMessages.Add("Treasure with ID " + splitRoom[5] + " does not exist.");
                    }

                    // If using a threat that doesnt exist, add relevant error to errorMessage array
                    if (Convert.ToInt32(splitRoom[6]) != 0 && !threatIds.Contains(Convert.ToInt32(splitRoom[6])))
                    {
                        errorMessages.Add("Threat with ID " + splitRoom[6] + " does not exist.");
                    }
                }
            }

            // Iterates over passages
            foreach (string passage in passages)
            {
                string[] splitPassage = passage.Split(',');
                bool correctLength = true;

                // If passage ID is 0, add relevant error to errorMessage array
                if (passage.StartsWith("0"))
                {
                    errorMessages.Add("Passage ID may not be 0");
                }

                // If incorrect number of parameters in passage, add relevant error to errorMessage array
                if (splitPassage.Length != 4) 
                {
                    errorMessages.Add("Please ensure all passages have 4 parameters");
                    correctLength = false;
                }

                // If multiple exit passages have been defined, add relevant error to errorMessage array
                if (splitPassage[1] == "1")
                {
                    if (exitPassage)
                    {
                        errorMessages.Add("Only 1 exit is allowed");
                    }
                    exitPassage = true;
                }

                if (correctLength)
                {
                    for (int i = 2; i < 4; i++)
                    {
                        // If using a room that doesn't exist, add relevant error to errorMessage array
                        if (Convert.ToInt32(splitPassage[i]) != 0 && !roomIds.Contains(Convert.ToInt32(splitPassage[i])))
                        {
                            errorMessages.Add("Room with ID " + splitPassage[i] + " does not exist.");
                        }
                    }
                }
            }

            foreach(string item in items)
            {
                // If threat/treasure id is 0, add relevant error to errorMessage array
                if (item.StartsWith("0"))
                {
                    errorMessages.Add("Treasure/Threat ID may not be 0");
                }

                // If incorrect number of parameters in an item, add relevant error to errorMessage array
                if (item.Split(',').Length != 6)
                {
                    errorMessages.Add("Please ensure all items have 6 parameters");
                }
            }

            // This is going over every list of IDs, and checking if all IDs are unique.
            // .Distinct() gets each item that is unique
            // .Count() gets the amount of unique items
            // This is then compared to the length of the list of IDs.
            // They should be equal as every ID should be unique
            roomUnique = roomIds.Distinct().Count() == roomIds.Count();
            passageUnique = passageIds.Distinct().Count() == passageIds.Count();
            threatUnique = threatIds.Distinct().Count() == threatIds.Count();
            treasureUnique = treasureIds.Distinct().Count() == treasureIds.Count();

            // If any ID isn't unique, add relevant error to errorMessage array
            if (!roomUnique) {
                errorMessages.Add("All Room IDs must be unique");
            }

            if (!passageUnique)
            {
                errorMessages.Add("All Passage IDs must be unique");
            }

            if (!threatUnique)
            {
                errorMessages.Add("All Threat IDs must be unique");
            }

            if (!treasureUnique)
            {
                errorMessages.Add("All Treasure IDs must be unique");
            }

            //Returns the errorMessages list as an array.
            return errorMessages.ToArray();
        }

    }
}
