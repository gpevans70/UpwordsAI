using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UpwordsAIviaConsole.AI;


namespace UpwordsAIviaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AIworker TheAI = new AIworker();

            string[,] boardTiles = new string[10, 10];
            int[,] boardHeights = new int[10, 10];

            string[] fileContents = File.ReadAllLines("C:/Users/Public/Documents/board.txt");

            int lineCounter = 0;
            while (lineCounter<10)
            {
                int rowCounter = 0;
                foreach (char c in fileContents[lineCounter])
                {
                    boardTiles[lineCounter, rowCounter] = " ";
                    if (rowCounter<10 && c >='A' && c <= 'Z')
                    {
                        boardTiles[lineCounter, rowCounter] = c.ToString();
                    }
                    rowCounter++;
                }
                lineCounter++;
            }
;
            lineCounter = 0;
            while (lineCounter < 10)
            {
                int rowCounter = 0;
                foreach (char c in fileContents[lineCounter+11])
                {
                    boardHeights[lineCounter, rowCounter] = 0;
                    if (rowCounter < 10 && c >= '0' && c <= '5')
                    {
                        int parseResult;
                        bool parseOutcome = Int32.TryParse(c.ToString(), out parseResult);
                        if (parseOutcome)
                        {
                            boardHeights[lineCounter, rowCounter] = parseResult;
                        }                     
                    }
                    rowCounter++;
                }
                lineCounter++;
            }
            Console.WriteLine("");
            Console.WriteLine("I see the board as follows:");
            Console.WriteLine("");
            Console.WriteLine("  +------+------+------+------+------+------+------+------+------+------+");

            int errorLine = 0;
            int errorRow = 0;
            string errorString = "";

            for (int i=0; i<10; i++)
            {
                Console.Write("  ");

                for (int j=0; j<10; j++)
                {
                    if (boardTiles[i, j] == "Q")
                    {
                        Console.Write("| Qu");
                    }
                    else
                    {
                        Console.Write($"| {boardTiles[i, j]} ");
                    }
                    
                    if (boardHeights[i, j] >0)
                    {
                        Console.Write($" {boardHeights[i, j]} ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    if (boardTiles[i, j] == " " && boardHeights[i, j] > 0)
                    {
                        errorLine = i;
                        errorRow = j;
                        errorString = "Height but no tile at";
                    }
                    if (boardTiles[i, j] != " " && boardHeights[i, j] == 0)
                    {
                        errorLine = i;
                        errorRow = j;
                        errorString = "Tile but no height at";
                    }
                }
                Console.Write("|");
                Console.WriteLine("");
                Console.WriteLine("  +------+------+------+------+------+------+------+------+------+------+");
            }
            Console.WriteLine("");

            if (errorString == "")
            {
                Console.WriteLine($"My tiles are: {args[0]}");
                Console.WriteLine("");
                Console.WriteLine("Press X followed by ENTER key to eXit or the ENTER key to start the AI");
                string UserInput = Console.ReadLine();

                if (UserInput != "X")
                {
                    TheAI.DoStuff();
                    Console.WriteLine("Press the ENTER key to exit");
                    Console.ReadLine();
                }


            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine($"{errorString} at line index {errorLine} and row index {errorRow}");
                Console.WriteLine("");
                Console.WriteLine("Press the ENTER key to exit");
                Console.ReadLine();

            }

        }
    }
}
