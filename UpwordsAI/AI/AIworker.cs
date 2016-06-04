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
            PatternHolder bestScoringPatternHolder = null;

            int bestPatternScore = 0;
            int thresholdAdjustment = 0; // This will be non-zero when we start trying to factor in the quality of the rack remaining after a move

            Dictionary.Dictionary TheDictionary;
            TheDictionary = new Dictionary.Dictionary();

            string bestWord = "";

            //int bestWordScore = 0;

            char[,] softTiles = new char[12, 12]; // As we will often want to check around a location,... 
            int[,] softHeights = new int[12, 12]; // ...work as thought the array was one based (but don't throw an error on zero or 10+1) 

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    softTiles[i, j] = ' ';
                    softHeights[i, j] = 0;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    softTiles[i + 1, j + 1] = boardTiles[i, j][0];
                    softHeights[i + 1, j + 1] = boardHeights[i, j];
                }
            }

            char[] Rack = boardRack.ToCharArray();
            Array.Sort(Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 1, softTiles, softHeights, Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 2, softTiles, softHeights, Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 3, softTiles, softHeights, Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 4, softTiles, softHeights, Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 5, softTiles, softHeights, Rack);

            CalculateOneLength(ref bestScoringPatternHolder, ref bestPatternScore, thresholdAdjustment, TheDictionary, ref bestWord, 6, softTiles, softHeights, Rack);

            PatternHolder ph = bestScoringPatternHolder;

            Debug.Print($"Best pattern: {ph.BestScoringWord} gives new best word score {ph.BestScore}");

            Console.WriteLine("");
            Console.WriteLine($"Best pattern: {ph.BestScoringWord} gives new best word score {ph.BestScore}");
            Console.WriteLine("");

            foreach (Pattern p in ph.ThePattern)
            {
                Console.WriteLine($"Pattern: {p.Letters}");
            }

            char[] charBestScoringWord = ph.BestScoringWord.ToCharArray();
            char[] charBestExistingTiles = ph.ThePattern[0].Letters.ToCharArray();

            for (int i = 0; i < ph.BestScoringWord.Length; i++)
            {
                if (charBestScoringWord[i] != charBestExistingTiles[i])
                {
                    int thisX;
                    int thisY;

                    if (ph.TheDirection == Directions.XChanging)
                    {
                        thisX = ph.TheLocationX + i;
                        thisY = ph.TheLocationY;
                    }
                    else
                    {
                        thisX = ph.TheLocationX;
                        thisY = ph.TheLocationY + i;
                    }
                    Console.WriteLine($"Play tile {charBestScoringWord[i]} at {thisX} down and {thisY} across");
                    boardTiles[thisX - 1, thisY - 1] = charBestScoringWord[i].ToString().ToLower();
                }
            }
            return $"Word is {ph.BestScoringWord}, score is {ph.BestScore}";
        }



        //

        private static void CalculateOneLength(
            ref PatternHolder bestScoringPatternHolder, ref int bestPatternScore, int thresholdAdjustment,
            Dictionary.Dictionary TheDictionary, ref string bestWord,
            int lengthToCheck, char[,] softTiles, int[,] softHeights, char[] Rack)
        {
            Debug.WriteLine($"Starting length {lengthToCheck}");

            List<PatternHolder> patternHolders = new List<PatternHolder>();
            //int bestWordScore = 0;

            for (int i = 1; i < (12 - lengthToCheck); i++) // Working with 1 based arrays with soft edges (i.e. indexes zero and eleven are legal and return empty/zero)
            {
                for (int j = 1; j < 11; j++)
                {
                    PatternHolder temp;
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.XChanging, lengthToCheck, 1, 1);
                    if (temp != null)
                    {
                        temp.MaxScore = AIHelpers.ComputePatternMaxScore(temp.ThePattern);
                        patternHolders.Add(temp);
                    }
                }
            }

            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < (12 - lengthToCheck); j++)
                {
                    PatternHolder temp;
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.YChanging, lengthToCheck, 1, 1);
                    if (temp != null)
                    {
                        temp.MaxScore = AIHelpers.ComputePatternMaxScore(temp.ThePattern);
                        patternHolders.Add(temp);
                    }
                }
            }

            foreach (PatternHolder ph in patternHolders)
            {
                int bestWordScore = 0;
                string bestWordTilesRemaining = "";

                if (ph.MaxScore <= bestPatternScore - thresholdAdjustment)
                {
                    if (lengthToCheck > 2) Debug.Print($"  ** Pattern eliminated {ph.ThePattern[0].Letters} at [{ph.TheLocationX},{ph.TheLocationY}]. Max score is {ph.MaxScore} and criterion is {bestPatternScore - thresholdAdjustment}");
                }
                else
                {
                    //Debug.Print($"  >> Evaluate pattern {ph.ThePattern[0].Letters} at [{ph.TheLocationX},{ph.TheLocationY}]. Max score is {ph.MaxScore} and criterion is {bestPatternScore - thresholdAdjustment}");

                    string regexLookup = AIHelpers.GenerateLookup(ph.ThePattern, Rack);
                    string dictionary = TheDictionary.WordList;

                    MatchCollection mm = Regex.Matches(dictionary, regexLookup);
                    if (mm.Count == 0)
                    {
                        //Debug.WriteLine("  No words found");
                    }

                    foreach (Match m in mm) // Don't need to embed this in an else from mm.Count == 0 as the foreach will not happen in that case
                    {
                        // Check for found word same as existing word
                        if (m.Value == ph.ThePattern[0].Letters)
                        {
                            continue; //Go to the next iteration of the foreach (Match m in mm), i.e. the next word
                        }

                        // Check the required letters against the rack

                        // Need to take the board letters out before checking against the rack
                        char[] foundWord = m.Value.ToCharArray();
                        char[] existingTiles = ph.ThePattern[0].Letters.ToCharArray();
                        char[] intersectionsPattern = ph.ThePattern[1].Letters.ToCharArray();
                        char[] tilesNeeded = new char[foundWord.Length + 1];
                        char[] tilesLeftOnRack;

                        int tilesNeededCount = 0; // Also used later for scoring as it is the number of new tiles played
                        for (int i = 0; i < foundWord.Length; i++)
                        {
                            if (existingTiles[i] != foundWord[i])
                            {
                                tilesNeeded[tilesNeededCount++] = foundWord[i]; // Works as the index because we post increment
                            }
                        }
                        Array.Resize(ref tilesNeeded, tilesNeededCount);

                        if (!AIHelpers.CheckRack(Rack, tilesNeeded, out tilesLeftOnRack))
                        {
                            continue; //Go to the next iteration of the foreach (Match m in mm), i.e. the next word
                        }

                        //Debug.WriteLine($"Rack = {new string (Rack)}, Word = {new string (foundWord)}, Tiles needed = {new string (tilesNeeded)}, Tiles left = {new string (tilesLeftOnRack)}");
                        // End of checking against the rack

                        bool completeWordCovered = false;

                        bool inWord = false;
                        int letterCount = 0;
                        int coverCount = 0;

                        for (int i = 0; i < existingTiles.Length; i++)
                        {
                            if (existingTiles[i] == '*') //Empty space where we are playing the word
                            {
                                if (inWord) // We were previously in a word so we've just finished one
                                {
                                    if (letterCount > 1 && letterCount == coverCount)
                                    {
                                        completeWordCovered = true;
                                        break;
                                    }
                                    else
                                    {
                                        inWord = false;
                                        letterCount = 0;
                                        coverCount = 0;
                                    }
                                }
                            }
                            else // It's an existing letter
                            {
                                inWord = true;
                                letterCount++;
                                if (existingTiles[i] != foundWord[i]) coverCount++;
                            }
                        }

                        if (completeWordCovered || (letterCount > 1 && letterCount == coverCount))
                        {
                            //Debug.WriteLine($"{new string(foundWord)} covers a complete word in {new string(existingTiles)}");
                            continue;
                        }
                        // End of check for covering whole words

                        // Check intersecting words
                        int intersectingPattern = 2;
                        int scoreForIntersectingWords = 0;
                        bool intersectionCheckOK = true;

                        for (int i = 0; i < ph.ThePattern[1].Letters.Length; i++)
                        {
                            char c = intersectionsPattern[i];
                            if (c == '!' || c == '+')
                            {
                                if (ph.ThePattern[0].Letters[i] != foundWord[i])
                                {
                                    char[] intersectingWord = ph.ThePattern[intersectingPattern].Letters.ToCharArray();
                                    int pos = Array.FindIndex(intersectingWord, x => x == c);
                                    intersectingWord[pos] = foundWord[i];
                                    string lookup = @"\b" + new string(intersectingWord) + @"\b";

                                    Match mx = Regex.Match(dictionary, lookup);
                                    if (!mx.Success)
                                    {
                                        //Debug.WriteLine($"  **  Fails intersection test");
                                        intersectionCheckOK = false;
                                        break; // Exit the immediate for loop checking all the interesections
                                    }
                                    else
                                    {
                                        scoreForIntersectingWords += (ph.ThePattern[intersectingPattern].Score + 1);
                                        // Check for flat word bonus - only possible if the intersection is of type "!" (Intersection on a empty square)
                                        if (c == '!' && ph.ThePattern[intersectingPattern].Score == ph.ThePattern[intersectingPattern].Letters.Length - 1)
                                        {
                                            scoreForIntersectingWords += (ph.ThePattern[intersectingPattern].Score + 1);
                                            //Debug.WriteLine($"  >>  Intersecting word {new string(intersectingWord)} scores flat word bonus");
                                        }
                                        //Debug.WriteLine($"  >>  Passes intersections test ({mx.Value}). Score of intersecting words now {scoreForIntersectingWords}");
                                    }
                                }
                                intersectingPattern++;
                            }
                        }

                        if (!intersectionCheckOK) continue; //Go to the next iteration of the foreach (Match m in mm), i.e. the next word
                        // End of checking intersecting words

                        // Now to work out the score

                        int wordScore = ph.ThePattern[1].Score + tilesNeededCount;
                        if (wordScore == ph.ThePattern[1].Letters.Length)
                        {
                            wordScore *= 2;
                            //Debug.WriteLine($"  >>  Main word {new string(foundWord)} scores flat word bonus");
                        }
                        //Debug.WriteLine($"  >>  Main word score {wordScore} + intersecting words {scoreForIntersectingWords} = {wordScore + scoreForIntersectingWords}");
                        wordScore += scoreForIntersectingWords;
                        if (wordScore > bestWordScore)
                        {
                            bestWordScore = wordScore;
                            bestWord = new String(foundWord);
                            bestWordTilesRemaining = new String(tilesLeftOnRack);
                            Debug.WriteLine($"  >>  {bestWord} gives new best word score {bestWordScore} for this pattern");
                        }
                    }

                    if (bestWordScore > (bestScoringPatternHolder?.BestScore ?? 0))
                    {
                        ph.BestScoringWord = bestWord;
                        ph.BestScore = bestWordScore;
                        ph.TilesLeftOnRack = bestWordTilesRemaining;

                        bestPatternScore = bestWordScore;
                        bestScoringPatternHolder = ph;
                        Console.WriteLine($"  >>  New best pattern: {bestScoringPatternHolder.BestScoringWord} gives new best word score {bestScoringPatternHolder.BestScore} and {bestScoringPatternHolder.TilesLeftOnRack} tiles left");

                        //Debug.WriteLine($"  >>  New best pattern: {bestPatternHolder.BestScoringWord} gives new best word score {bestPatternHolder.BestScore}");
                    }
                }
            }
        }
    }
}