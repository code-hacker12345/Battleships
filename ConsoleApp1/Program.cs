// See https://aka.ms/new-console-template for more information

using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

// BATTLESHIP

bool continueGame = true;
while (continueGame)
{
    continueGame = TriggerGame();
}


//--------------------------------------------------------------------------------------------------------------//

// Functions

// Starts the game
static bool TriggerGame()
{

    // Asks the user if they want to Play against the Computer or again another PLayer
    Console.WriteLine("Enter if you want to play aginst the computer (c) or against a player (p)");
    string computerOrplayer = Console.ReadLine();
    Console.WriteLine();

    while (computerOrplayer.ToUpper() != "C" && computerOrplayer.ToUpper() != "P")
    {
        Console.WriteLine("Invalid input. Enter again. (c/p)");
        Console.WriteLine();
        Console.WriteLine("Enter if you want to play aginst the computer (c) or against a player (p)");
        computerOrplayer = Console.ReadLine();
        Console.WriteLine();
    }

    if (computerOrplayer.ToUpper() == "C")
    {
        // Enter code to play against computer
        return MainGameCvP();
    }
    else if (computerOrplayer.ToUpper() == "P")
    {
        // Calls the main program for Player V Player
        return MainGamePvP();
    }
    else
    {
        Console.WriteLine("You did not follow the instructions so you don't get to play.");
        return false;
    }
}

// A function that contains the MAIN PROGRAM for computer V player
static bool MainGameCvP()
{

    bool playAgain = true;

    // Explanation of the game
    Console.WriteLine("Welcome to the *Battleships*!!!!!!");
    Console.WriteLine();
    Console.WriteLine("THE RULES: ");
    Console.WriteLine("You are playing against the computer. The player and computer arranges a set number of ships into a grid. \nThe number of rows and columns in the grid can be decided. \n The player searches for a ship to destroy in the other player's grid by entering coordinates (Coordinates starts 0,0). \nThe one who destroys the most ships WINS!!! \nGOOD LUCK...");
    Console.WriteLine();

    // Ask for user names
    Console.WriteLine("Enter the name of the computer.");
    string? computerName = Console.ReadLine();

    Console.WriteLine("Player. Enter your name.");
    string? player1Name = Console.ReadLine();

    Console.Clear();


    // The Setup
    // The two grids for the two players

    // Taking Inputs of the row size, column size and the number of ships to be placed in each grid
    // --------------------------------------------------------------------------------------------
    string StringrowSize = "";
    int rowSize = 0;
    string StringcolumnSize = "";
    int columnSize = 0;
    string StringnoOfShips = "";
    int noOfShips = 0;
    bool valid = false;
    bool correctRow = false;
    bool correctColm = false;
    bool correctShips = false;

    //Validate the row, column, no. of ships inputs

    // checks row size
    while (!correctRow)
    {
        Console.WriteLine("Enter the number of rows in your grid");
        StringrowSize = Console.ReadLine();
        valid = validateRowColsShips(StringrowSize, out rowSize);

        if (!valid)
        {
            correctRow = false;
        }
        else
        {
            correctRow = true;
        }
    }
    // end while

    // checks column size
    while (!correctColm)
    {
        Console.WriteLine();

        Console.WriteLine("Enter the number of columns in your grid");
        StringcolumnSize = Console.ReadLine();
        valid = validateRowColsShips(StringcolumnSize, out columnSize);

        if (!valid)
        {
            correctColm = false;
        }
        else
        {
            correctColm = true;
        }
    }
    // end while

    // cheks no. of ships
    while (!correctShips)
    {
        Console.WriteLine();

        Console.WriteLine("Enter the number of ships to be placed");
        StringnoOfShips = Console.ReadLine();
        valid = validateRowColsShips(StringnoOfShips, out noOfShips, rowSize, columnSize);


        if (!valid)
        {
            correctShips = false;
        }
        else
        {
            correctShips = true;
        }
    }
    // end while

    //Validation ends

    Console.Clear();
    //---------------------------------------------------------------------------------------------

    string[,]? player1 = new string[rowSize, columnSize]; // grid 1
    string[,]? computer = new string[rowSize, columnSize]; // grid 2

    string[,]? copyP1 = new string[rowSize, columnSize]; // copy of grid 1 for searches
    string[,]? copyComputer = new string[rowSize, columnSize]; // copy of grid 2 for seatches 

    string[,]? copyEntryP1 = new string[rowSize, columnSize]; // copy of grid 1 for entries
    string[,]? copyEntryComputer = new string[rowSize, columnSize]; // copy of grid 2 for entries

    //----------------------------------------------------------------------------------------------
    //Initialises player and computer grids

    Console.WriteLine("{0}'s turn to place ships", computerName);
    computer = playerInputs(computer, noOfShips, rowSize, columnSize, copyEntryComputer, "c");

    Console.Clear(); // clear screen

    Console.WriteLine("{0}'s turn to place ships", player1Name);
    player1 = playerInputs(player1, noOfShips, rowSize, columnSize, copyEntryP1, "p");


    Console.Clear(); // clear screen

    // Let The Game Begin--------------------------------------------------------------------------

    Random rnd = new Random();

    int successP1 = 0;
    int successComputer = 0;
    int failP1 = 0;
    int failComputer = 0;
    int noOfTries = noOfShips;
    int therow = 0;
    int thecolumn = 0;
    string searchInput = "";
    string[] tempArray = new string[2];
    List<string> InputStackP1 = new List<string>();  // List in C# !!!!!
    List<string> InputStackP2 = new List<string>();  // List in C# !!!!!

    Console.WriteLine("You have {0} chances. Let the games begin!!!", noOfTries);
    Console.WriteLine();

    bool Validation = false;
    bool correctInput = false;
    string[] tempSplit = new string[2];

    // Player1's turn to check
    Console.WriteLine("{0}'s turn to check", player1Name);

    for (int i = 0; i < noOfTries; i++)
    {
        // Printing out a copy of a player grid filled with X
        Console.WriteLine();
        initNullValuesToGrid(copyP1, rowSize, columnSize);
        printGrid(copyP1, rowSize, columnSize);

        Console.WriteLine();
        Console.WriteLine("Check {0}", (i + 1));
        Console.WriteLine("Enter a coordinate to search row,column: ");
        searchInput = Console.ReadLine();

        correctInput = false;

        while (!correctInput)
        {
            Validation = validateInput(computer, rowSize, columnSize, searchInput, 1, InputStackP1);
            if (Validation)
            {
                tempSplit = searchInput.Split(',');
                therow = int.Parse(tempSplit[0]);
                thecolumn = int.Parse(tempSplit[1]);

                if (computer[therow, thecolumn] == "S")
                {
                    successP1++;
                    Console.WriteLine("You found a ship! Keep going!");
                    copyP1[therow, thecolumn] = "S";
                }
                else
                {
                    failP1++;
                    Console.WriteLine("You did not find a ship yet :( Keep powering through!");
                }

                correctInput = true;
            }
            else
            {
                searchInput = Console.ReadLine();
                correctInput = false;
            }

        }
    }
    Console.WriteLine();
    initNullValuesToGrid(copyP1, rowSize, columnSize);
    printGrid(copyP1, rowSize, columnSize);

    Console.WriteLine();
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    Console.Clear();

    // Computer's turn to check
    Console.WriteLine("{0}'s turn to check", computerName);

    for (int i = 0; i < noOfTries; i++)
    {

        int searchComputerRow = rnd.Next(rowSize);
        int searchComputerColumn = rnd.Next(columnSize);

        if (player1[searchComputerRow, searchComputerColumn] == "S")
        {
            successComputer++;
            copyComputer[therow, thecolumn] = "S";
        }
        else
        {
            failComputer++;
        }
    }

    Console.WriteLine();
    Console.WriteLine("Computer has finished checking for ships");
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    Console.Clear();


    // Displays player stats
    Console.WriteLine("Player Stats: ");
    playerStats(player1Name, successP1, failP1);
    playerStats(computerName, successComputer, failComputer);

    Console.WriteLine();

    // Displays the winner
    if (successP1 > successComputer)
    {
        Console.WriteLine("{0} has won by {1} point(s). Congratulations!", player1Name, (successP1 - successComputer));
    }
    else if (successComputer > successP1)
    {
        Console.WriteLine("{0} has won by {1} point(s). HAHAHA!", computerName, (successComputer - successP1));
    }
    else
    {
        Console.WriteLine("Unbelievable! It's a draw...");
    }

    // Asks if user wants to play again
    Console.WriteLine("Do you want to play again? (y/n)");
    string choice = Console.ReadLine();
    Console.WriteLine();

    if (choice == "y")
    {
        playAgain = true;
    }
    else if (choice == "n")
    {
        playAgain = false;
    }
    else
    {
        Console.WriteLine("You did not enter in the right format so you don't get a turn -_-");
        playAgain = false;
    }

    return playAgain;
}

// A function that contains the MAIN PROGRAM for player V player
static bool MainGamePvP()
{

    bool playAgain = true;

    // Explanation of the game
    Console.WriteLine("Welcome to the Battleship!!!!!!");
    Console.WriteLine();
    Console.WriteLine("THE RULES: ");
    Console.WriteLine("This is a two player game. Each player arranges a set number of ships into a grid. \nThe number of rows and columns in the grid can be decided. \nEach player searches for a ship to destroy in the other player's grid by entering coordinates (Coordinates starts 0,0). \nThe one who destroys the most ships WINS!!! \nGOOD LUCK...");
    Console.WriteLine();

    // Ask for user names
    Console.WriteLine("Player 1. Enter your name.");
    string? player1Name = Console.ReadLine();

    Console.WriteLine("Player 2. Enter your name.");
    string? player2Name = Console.ReadLine();

    Console.Clear();


    // The Setup
    // The two grids for the two players

    // Taking Inputs of the row size, column size and the number of ships to be placed in each grid
    // --------------------------------------------------------------------------------------------
    string StringrowSize = "";
    int rowSize = 0;
    string StringcolumnSize = "";
    int columnSize = 0;
    string StringnoOfShips = "";
    int noOfShips = 0;
    bool valid = false;
    bool correctRow = false;
    bool correctColm = false;
    bool correctShips = false;

    //Validate the row, column, no. of ships inputs

    // checks row size
    while (!correctRow)
    {
        Console.WriteLine("Enter the number of rows in your grid");
        StringrowSize = Console.ReadLine();
        valid = validateRowColsShips(StringrowSize, out rowSize);

        if (!valid)
        {
            correctRow = false;
        }
        else
        {
            correctRow = true;
        }
    }
    // end while

    // checks column size
    while (!correctColm)
    {
        Console.WriteLine();

        Console.WriteLine("Enter the number of columns in your grid");
        StringcolumnSize = Console.ReadLine();
        valid = validateRowColsShips(StringcolumnSize, out columnSize);

        if (!valid)
        {
            correctColm = false;
        }
        else
        {
            correctColm = true;
        }
    }
    // end while

    // cheks no. of ships
    while (!correctShips)
    {
        Console.WriteLine();

        Console.WriteLine("Enter the number of ships to be placed");
        StringnoOfShips = Console.ReadLine();
        valid = validateRowColsShips(StringnoOfShips, out noOfShips, rowSize, columnSize);


        if (!valid)
        {
            correctShips = false;
        }
        else
        {
            correctShips = true;
        }
    }
    // end while

    //Validation ends

    Console.Clear();
    //---------------------------------------------------------------------------------------------

    string[,]? player1 = new string[rowSize, columnSize]; // grid 1
    string[,]? player2 = new string[rowSize, columnSize]; // grid 2

    string[,]? copyP1 = new string[rowSize, columnSize]; // copy of grid 1 for searches
    string[,]? copyP2 = new string[rowSize, columnSize]; // copy of grid 2 for seatches 

    string[,]? copyEntryP1 = new string[rowSize, columnSize]; // copy of grid 1 for entries
    string[,]? copyEntryP2 = new string[rowSize, columnSize]; // copy of grid 1 for entries

    //----------------------------------------------------------------------------------------------
    //Initialises player 1 and 2 grid

    Console.WriteLine("{0}'s turn", player1Name);
    player1 = playerInputs(player1, noOfShips, rowSize, columnSize, copyEntryP1, "p");

    Console.Clear(); // clear screen

    Console.WriteLine("{0}'s turn", player2Name);
    player2 = playerInputs(player2, noOfShips, rowSize, columnSize, copyEntryP2, "p");


    Console.Clear(); // clear screen

    // Let The Game Begin--------------------------------------------------------------------------

    int successP1 = 0;
    int successP2 = 0;
    int failP1 = 0;
    int failP2 = 0;
    int noOfTries = noOfShips;
    int therow = 0;
    int thecolumn = 0;
    string searchInput = "";
    string[] tempArray = new string[2];
    List<string> InputStackP1 = new List<string>();  // List in C# !!!!!
    List<string> InputStackP2 = new List<string>();  // List in C# !!!!!

    Console.WriteLine("You each have {0} chances. Let the games begin!!!", noOfTries);
    Console.WriteLine();

    bool Validation = false;
    bool correctInput = false;
    string[] tempSplit = new string[2];

    // Player1's turn to check
    Console.WriteLine("{0}'s turn", player1Name);

    for (int i = 0; i < noOfTries; i++)
    {
        // Printing out a copy of a player grid filled with X
        Console.WriteLine();
        initNullValuesToGrid(copyP1, rowSize, columnSize);
        printGrid(copyP1, rowSize, columnSize);

        Console.WriteLine();
        Console.WriteLine("Check {0}", (i + 1));
        Console.WriteLine("Enter a coordinate to search row,column: ");
        searchInput = Console.ReadLine();

        correctInput = false;

        while (!correctInput)
        {
            Validation = validateInput(player2, rowSize, columnSize, searchInput, 1, InputStackP1);
            if (Validation)
            {
                tempSplit = searchInput.Split(',');
                therow = int.Parse(tempSplit[0]);
                thecolumn = int.Parse(tempSplit[1]);

                if (player2[therow, thecolumn] == "S")
                {
                    successP1++;
                    Console.WriteLine("You found a ship! Keep going!");
                    copyP1[therow, thecolumn] = "S";
                }
                else
                {
                    failP1++;
                    Console.WriteLine("You did not find a ship yet :( Keep powering through!");
                }

                correctInput = true;
            }
            else
            {
                searchInput = Console.ReadLine();
                correctInput = false;
            }

        }
    }
    Console.WriteLine();
    initNullValuesToGrid(copyP1, rowSize, columnSize);
    printGrid(copyP1, rowSize, columnSize);

    Console.WriteLine();
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    Console.Clear();

    Console.WriteLine("You each have {0} chances. Let the games begin!!!", noOfTries);
    Console.WriteLine();

    Validation = false;
    correctInput = false;

    // Player 2's turn to check
    Console.WriteLine("{0}'s turn", player2Name);

    for (int i = 0; i < noOfTries; i++)
    {
        Console.WriteLine();
        initNullValuesToGrid(copyP2, rowSize, columnSize);
        printGrid(copyP2, rowSize, columnSize);

        Console.WriteLine();
        Console.WriteLine("Check {0}", (i + 1));
        Console.WriteLine("Enter a coordinate to search row,column: ");
        searchInput = Console.ReadLine();

        correctInput = false;

        while (!correctInput)
        {
            Validation = validateInput(player1, rowSize, columnSize, searchInput, 1, InputStackP2);
            if (Validation)
            {
                tempSplit = searchInput.Split(',');
                therow = int.Parse(tempSplit[0]);
                thecolumn = int.Parse(tempSplit[1]);

                if (player1[therow, thecolumn] == "S")
                {
                    successP2++;
                    Console.WriteLine("You found a ship! Keep going!");
                    copyP2[therow, thecolumn] = "S";
                }
                else
                {
                    failP2++;
                    Console.WriteLine("You did not find a ship yet :( Keep powering through!");
                }

                correctInput = true;
            }
            else
            {
                searchInput = Console.ReadLine();
                correctInput = false;
            }

        }
    }
    Console.WriteLine();
    initNullValuesToGrid(copyP2, rowSize, columnSize);
    printGrid(copyP2, rowSize, columnSize);

    Console.WriteLine();
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    Console.Clear();


    // Displays player stats
    Console.WriteLine("Player Stats: ");
    playerStats(player1Name, successP1, failP1);
    playerStats(player2Name, successP2, failP2);

    Console.WriteLine();

    // Displays the winner
    if (successP1 > successP2)
    {
        Console.WriteLine("{0} has won by {1} point(s). Congratulations!", player1Name, (successP1 - successP2));
    }
    else if (successP2 > successP1)
    {
        Console.WriteLine("{0} has won by {1} point(s). Congratulations!", player2Name, (successP2 - successP1));
    }
    else
    {
        Console.WriteLine("Unbelievable! It's a draw...");
    }

    // Asks if user wants to play again
    Console.WriteLine("Do you want to play again? (y/n)");
    string choice = Console.ReadLine();
    Console.WriteLine();

    if (choice == "y")
    {
        playAgain = true;
    }
    else if (choice == "n")
    {
        playAgain = false;
    }
    else
    {
        Console.WriteLine("You did not enter in the right format so you don't get a turn -_-");
        playAgain = false;
    }

    return playAgain;
}




// Printing out player stats
static void playerStats(string? p_playerName, int p_success, int p_faliure)
{
    Console.WriteLine();
    Console.WriteLine("{0}'s stats", p_playerName);
    Console.WriteLine("Success(es) : {0} (You found {0} ship(s).)   |    Failure(s) : {1} (You didn't find the other {1} ship(s).)", p_success, p_faliure);
}

// Setting up the grid with player inputs
static string[,]? playerInputs(string[,] p_array, int p_noShips, int rows, int cols, string[,] copyArray, string mode = "")
{
    Random rnd = new Random();
    bool correctInput = false;

    if (mode == "p")
    {
        for (int i = 0; i < p_noShips; i++)
        {
            // Displays an empty grid before each entry
            Console.WriteLine();
            initNullValuesToGrid(copyArray, rows, cols);
            printGrid(copyArray, rows, cols);
            Console.WriteLine();

            Console.WriteLine("Entry {0}", (i + 1));
            Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
            string theInputs = Console.ReadLine();
            bool Validation = false;
            correctInput = false;
            string[] tempSplit = new string[2];


            // Validations for the coordinate inputs

            while (!correctInput)
            {
                Validation = validateInput(p_array, rows, cols, theInputs);
                if (Validation)
                {
                    tempSplit = theInputs.Split(',');
                    int row = int.Parse(tempSplit[0]);
                    int column = int.Parse(tempSplit[1]);
                    p_array[row, column] = "S";
                    copyArray[row, column] = "S";
                    correctInput = true;
                }
                else
                {
                    theInputs = Console.ReadLine();
                    correctInput = false;
                }

            }
            // Validation ends here

        }
        // Displays an empty grid before each entry
        Console.WriteLine();
        initNullValuesToGrid(copyArray, rows, cols);
        printGrid(copyArray, rows, cols);

        Console.WriteLine();
        Console.WriteLine("Please press enter to continue... ");
        Console.ReadLine();
        return p_array;
    }
    else if (mode == "c")
    {
        for (int i = 0; i < p_noShips; i++)
        {
            //Console.WriteLine();
            initNullValuesToGrid(copyArray, rows, cols);
            //Console.WriteLine();

            int computerRow = rnd.Next(rows);
            int computerColumn = rnd.Next(cols);
            p_array[computerRow, computerColumn] = "S";
            copyArray[computerRow, computerColumn] = "S";

        }
        Console.WriteLine();
        Console.WriteLine("Computer has finished placing the ships.");
        Console.WriteLine("Please press enter to continue... ");
        Console.ReadLine();
        return p_array;
    }
    
    p_array[0, 0] = "No"; 
    return p_array;
}
// Validation
static bool validateInput(string[,] p_array, int rows, int cols, string coordInput, int defVal = 0, List<string>? p_InputStack = null)
{
    string rowCoord = "";
    string columnCoord = "";
    int row = 0;
    int column = 0;
    string[] playerInputs = new string[2];


    // Checks if the input is empty
    if (coordInput.Length == 0)
    {
        Console.WriteLine();
        Console.WriteLine("Invalid Input. Try again.");
        //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
        //coordInput = Console.ReadLine();
        return false;
    }
    // end if

    // Checks if each coordinate is not seperated b column commas
    if (coordInput.Contains(",") == false)
    {
        Console.WriteLine();
        Console.WriteLine("Seperate the coordinate with commas please -_-");
        //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
        return false;
    }
    // end if

    // Checks if row and  column is empty
    playerInputs = coordInput.Split(",");

    rowCoord = playerInputs[0];
    columnCoord = playerInputs[1];

    if ((rowCoord == null) || (rowCoord.Trim().Length == 0) || (columnCoord == null) || (columnCoord.Trim().Length == 0))
    {
        Console.WriteLine();
        Console.WriteLine("row and column can not be nothing!!! Pay column attention this time. -_-");
        //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
        return false;
    }
    // end if

    // Checks if row and  column are not infact... integers
    bool isrowNum = int.TryParse(rowCoord, out row);
    bool iscolumnNum = int.TryParse(columnCoord, out column);

    if ((isrowNum == false) || (iscolumnNum == false))
    {
        Console.WriteLine();
        Console.WriteLine("row and column have to be integers... -_-");
        //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
        return false;

    }
    // end if

    // Checks if row and  column are within the range of the rows and columns of the grid
    if ((row >= rows) || (row < 0) || (column >= cols) || (column < 0))
    {
        Console.WriteLine();
        Console.WriteLine("Make sure your coordinates are within the range of the grid. Coordinates can be anywhere between (0,0) to ({0},{1})", (rows - 1), (cols - 1));
        //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
        return false;
    }
    // end if

    // Checks if the coordinates entered are repeated 
    if (defVal == 0)
    {
        if (p_array[row, column] == "S")
        {
            Console.WriteLine();
            Console.WriteLine("You have repeated a coordinate. Enter again.");
            //Console.WriteLine("Enter a coordinate where your ship should be placed row,column: ");
            return false;
        }
        // end if
    }
    else
    {
        if (!p_InputStack.Contains(coordInput))
        {
            p_InputStack.Add(coordInput);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("You have repeated a coordinate to search!! Try again. (Don't worry, you have and extra turn!)");
            return false;
        }
    }

    return true;
}

// Validate the row size, column size, no. of ships inputs
static bool validateRowColsShips(string p_size, out int p_numSize, int optional_row = 0, int optional_cols = 0)
{
    p_numSize = 0;

    if (p_size.Length == 0)
    {
        Console.WriteLine("Please enter a value for any of the inputs above :( .");
        Console.WriteLine("Enter Again");
        Console.WriteLine();
        return false;
    }
    else if (int.TryParse(p_size, out p_numSize) == false)
    {
        Console.WriteLine("Above inputs must be INTEGERS!!!");
        Console.WriteLine("Enter Again");
        Console.WriteLine();
        return false;
    }
    else if (p_numSize > 30 || p_numSize <= 0)
    {
        Console.WriteLine("STOP BREAKING MY CODE!!!!");
        Console.WriteLine("Enter a number between 0 and 30!!!!");
        Console.WriteLine("Enter Again");
        Console.WriteLine();
        return false;
    } 
    else if (optional_row != 0 || optional_cols != 0)
    {
        if (p_numSize > (optional_row*optional_cols))
        {
            Console.WriteLine("Number of ships can't be more than there is space to store the ships");
            Console.WriteLine("Enter Again");
            Console.WriteLine();
            return false;
        }
        return true;
    }
    else
    {
        return true;
    }
}


// Player 1 null values init
static void initNullValuesToGrid(string[,] p_array, int rows, int cols)
{
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            //Console.WriteLine(player1[i,j]);
            if (p_array[i, j] == null)
            {
                p_array[i, j] = "X";
            }
        }
    }
}

// Printing player grids
static void printGrid(string[,] p_array, int rows, int cols)
{
    Console.Write(" ");
    for (int i = 0; i < cols; i++)
    {
        Console.Write(" "+ (i));
        
    }
    Console.WriteLine();

    for (int i = 0; i < rows; i++)
    {
        Console.Write((i) + " ");

        for (int j = 0; j < cols; j++)
        {
            Console.Write(p_array[i, j]);
            Console.Write(" ");
        }
        Console.Write("\n");
    }
}