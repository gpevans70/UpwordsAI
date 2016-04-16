using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UpwordsAI.AI
{
    public class AIWorker
    {
        public string FindMove(string boardRack, string[,] boardTiles, int[,] boardHeights) // These are zero based arrays
        {
            List<PatternHolder> patternHolders = new List<PatternHolder>();

            Console.WriteLine("");
            Console.WriteLine("AIworker starting");
            Console.WriteLine("");

            char[,] softTiles = new char[12, 12]; // As we will often want to check around a location,... 
            int[,] softHeights = new int[12, 12]; // ...work as thought the array was one based (but don't throw an error on zero or 10+1) 

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    softTiles[i, j] = ' ';
                    softHeights[i, j] = 0;
                }
            }

            for (int i=0; i<10; i++)
            {
                for (int j=0; j<10; j++) 
                {
                    softTiles[i + 1, j + 1] = boardTiles[i, j][0];
                    softHeights[i + 1, j + 1] = boardHeights[i, j];
                }
            }

            char[] Rack = boardRack.ToCharArray();
            Array.Sort(Rack);


            //          internal static PatternHolder WhatCanIPlay(char[,] softTiles, int[,] softHeights, int locationX, int locationY, 
            //                                                     Directions direction, int length, int maxTiles, int minTiles)

            //          public static bool CheckRack(char[] Rack, char[] TilesNeeded)

            //          public static int ComputePatternMaxScore(List<Pattern> patterns)

            for (int i=1; i<11; i++) // Working with 1 based arrays with soft edges (i.e. indexes zero and eleven are legal and return empty/zero)
            {
                for (int j=1; j<11; j++)
                {
                    PatternHolder temp;
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.XChanging, 1, 1, 1);
                    if (temp != null)
                    {
                        temp.MaxScore = AIHelpers.ComputePatternMaxScore(temp.ThePattern);
                        patternHolders.Add(temp);
                    }
                }
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    PatternHolder temp;
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.YChanging, 1, 1, 1);
                    if (temp != null)
                    {
                        temp.MaxScore = AIHelpers.ComputePatternMaxScore(temp.ThePattern);
                        patternHolders.Add(temp);
                    }
                }
            }

            int bestScore = 0;
            int thresholdAdjustment = 0; // This will be non-zero when we start trying to factor in the quality of the rack remaining after a move

            Dictionary.Dictionary TheDictionary;
            TheDictionary = new Dictionary.Dictionary();

            foreach (PatternHolder p in patternHolders)
            {
            if (p.MaxScore < bestScore - thresholdAdjustment)
                {
                    Debug.Print($"Discard {p.ThePattern[0]}. Max score is {p.MaxScore} and criterion is {bestScore - thresholdAdjustment}");
                    break;
                }


                string regexLookup = AIHelpers.GenerateLookup(p.ThePattern,Rack);
                //Console.WriteLine("Lookup..." + regexLookup);
                Debug.Print("Lookup..." + regexLookup);
                //String dictionary = "FADE FACE BAG CAG CAD BAD PARE PARC AD BAGFADE BAGFACE BATED RATED GATED BATEE EPARE CARE AR PA PE TAR";
                string dictionary = TheDictionary.wordList;

                MatchCollection mm = Regex.Matches(dictionary, regexLookup);
                foreach (Match m in mm)
                {
                    if (m.Success)
                    {
                        // Need to take the board letters out before checking against the rack
                        char[] foundWord = m.Value.ToCharArray();
                        char[] existingTiles = p.ThePattern[0].Letters.ToCharArray();
                        char[] tilesNeeded = new char[foundWord.Length+1];

                        int tilesNeededIndex = 0;

                        for (int i=0; i< foundWord.Length; i++)
                        {
                            if (existingTiles[i] != foundWord[i])
                            {
                                tilesNeeded[tilesNeededIndex++] = foundWord[i];
                            }
                        }
                        Array.Resize(ref tilesNeeded, tilesNeededIndex);

                        if (!AIHelpers.CheckRack(Rack, tilesNeeded))
                        {
                            Debug.Print($">> Word found >> {m.Value} but {new string(tilesNeeded)} not achievable from rack {boardRack}");
                        }
                        else
                        {
                            Debug.Print(">>>>>> Usable Word found >>" + m.Value);
                        }
                    }
                }


            }





            Console.WriteLine("AIworker ending");
            Console.WriteLine("");

            boardTiles[1, 1] = "x";
            boardTiles[1, 2] = "y";
            boardTiles[1, 3] = "z";

            return "XYZ";

        }

    }
}
