﻿using System;
using System.Diagnostics;
using System.IO;
using UpwordsAI.AI;


namespace UpwordsAIviaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect number of arguments!");
                Console.WriteLine("Call with 'RACKTILES path\\to\\board.txt'");
                Console.WriteLine("Press any key to exit");
                Console.Read();

                return;
            }

            AIWorker aiWorker = new AIWorker();

            string[,] boardTiles = new string[10, 10];
            int[,] boardHeights = new int[10, 10];
            
            var boardRack = args[0].Trim().Replace("Qu","Q").ToUpper();            

            string[] fileContents = File.ReadAllLines(args[1]);

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
                Console.WriteLine($"My tiles are: {boardRack}");
                Console.WriteLine("");
                Console.WriteLine("Press X followed by ENTER key to eXit or the ENTER key to start the AI");
                string userInput = Console.ReadLine();

                if (userInput != "X")
                {
                    string move = aiWorker.FindMove(boardRack, boardTiles, boardHeights); // These are zero based arrays

                    Console.WriteLine("");
                    Console.WriteLine(""); Console.WriteLine($"I play tiles {move}");
                    Console.WriteLine("");
                    Console.WriteLine("  +------+------+------+------+------+------+------+------+------+------+");

                    for (int i = 0; i < 10; i++)
                    {
                        Console.Write("  ");

                        for (int j = 0; j < 10; j++)
                        {
                            if (boardTiles[i, j] == "Q")
                            {
                                Console.Write("|  Qu");
                            }
                            else
                            {
                                Console.Write($"|  {boardTiles[i, j]} ");
                            }
                            Console.Write("  ");
                        }
                        Console.Write("|");
                        Console.WriteLine("");
                        Console.WriteLine("  +------+------+------+------+------+------+------+------+------+------+");
                    }
                    Console.WriteLine("");

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
