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
            PatternHolder bestScoringPatternHolder=null;



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

            int bestPatternScore = 0;

            int thresholdAdjustment = 0; // This will be non-zero when we start trying to factor in the quality of the rack remaining after a move

            Dictionary.Dictionary TheDictionary;
            TheDictionary = new Dictionary.Dictionary();

            string bestWord = "";
            int bestWordScore = 0;

            foreach (PatternHolder p in patternHolders)
            {
                //Debug.WriteLine("New pattern");

                if (p.MaxScore < bestPatternScore - thresholdAdjustment)
                {
                    //Debug.Print($"  ** Pattern eliminated {p.ThePattern[0].Letters}. Max score is {p.MaxScore} and criterion is {bestPatternScore - thresholdAdjustment}");
                }

                else
                {
                    string regexLookup = AIHelpers.GenerateLookup(p.ThePattern,Rack);
//Console.WriteLine("Lookup..." + regexLookup);
                                    //String dictionary = "FADE FACE BAG CAG CAD BAD PARE PARC AD BAGFADE BAGFACE BATED RATED GATED BATEE EPARE CARE AR PA PE TAR";
                    string dictionary = TheDictionary.wordList;

                    MatchCollection mm = Regex.Matches(dictionary, regexLookup);
                    if (mm.Count == 0)
                    {
                        //Debug.WriteLine("  No words found");
                    }

                    foreach (Match m in mm) // Don't need to embed this in an else from mm.Count == 0 as the foreach will not happen in that case
                    {
                        //Debug.WriteLine($"  Word found >> {m.Value}");

                        // Check for found word same as exiting word
                        if (m.Value == p.ThePattern[0].Letters)
                        {
                            //Debug.WriteLine($"  **  Word eliminated - regex word {m.Value} and board word {p.ThePattern[0].Letters} are the same");
                            break; //Exit the foreach (Match m in mm)
                        }

                        // Check the required letters against the rack

                        // Need to take the board letters out before checking against the rack
                        char[] foundWord = m.Value.ToCharArray();
                        char[] existingTiles = p.ThePattern[0].Letters.ToCharArray();
                        char[] intersectionsPattern = p.ThePattern[1].Letters.ToCharArray();
                        char[] tilesNeeded = new char[foundWord.Length + 1];

                        int tilesNeededCount = 0; // Also used later for scoring as it is the number of new tiles played
                        for (int i = 0; i < foundWord.Length; i++)
                        {
                            if (existingTiles[i] != foundWord[i])
                            {
                                tilesNeeded[tilesNeededCount++] = foundWord[i]; // Works as the index because we post increment
                            }
                        }
                        Array.Resize(ref tilesNeeded, tilesNeededCount);

                        if (!AIHelpers.CheckRack(Rack, tilesNeeded))
                        {
                            //Debug.WriteLine($"  **  Word eliminated - tiles {new string(tilesNeeded)} not achievable from rack {boardRack}");
                            break; //Exit the foreach (Match m in mm)
                        }

                        //Debug.WriteLine($"  >>  Passes rack test");

                        // Not yet checking for covering whole words

                        // Check intersecting words
                        int intersectingPattern = 2;
                        int scoreForIntersectingWords = 0;

                        for (int i = 0; i < p.ThePattern[1].Letters.Length; i++)
                        {
                            char c = intersectionsPattern[i];
                            if (c == '!' || c == '+' && p.ThePattern[0].Letters[i] != foundWord[i])
                            {
                                char[] intersectingWord = p.ThePattern[intersectingPattern].Letters.ToCharArray();
                                int pos = Array.FindIndex(intersectingWord, x => x == c);
                                intersectingWord[pos] = foundWord[i];
                                string lookup = @"\b" + new string(intersectingWord)+ @"\b";

                                Match mx = Regex.Match(dictionary, lookup);
                                if (!mx.Success)
                                {
                                    //Debug.WriteLine($"  **  Fails intersection test");
                                    break;
                                }
                                else
                                {
                                    scoreForIntersectingWords += (p.ThePattern[intersectingPattern].Score + 1);
                                    // Check for flat word bonus - only possible if the intersection is of type "!" (Intersection on a empty square)
                                    if (c == '!' && p.ThePattern[intersectingPattern].Score == p.ThePattern[intersectingPattern].Letters.Length - 1)
                                    {
                                        scoreForIntersectingWords += (p.ThePattern[intersectingPattern].Score + 1);
                                        //Debug.WriteLine($"  >>  Intersecting word {new string(intersectingWord)} scores flat word bonus");
                                    }
                                    intersectingPattern++;
                                    //Debug.WriteLine($"  >>  Passes intersections test ({mx.Value}). Score of intersecting words now {scoreForIntersectingWords}");
                                }
                            }
                        }
                        //if (intersectingPattern == 2) Debug.WriteLine($"  >>  No intersections to check");

                        // End of checking intersecting words

                        // Now to work out the score

                        int wordScore = p.ThePattern[1].Score + tilesNeededCount;
                        if (wordScore == p.ThePattern[1].Letters.Length)
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
                            //Debug.WriteLine($"  >>  {bestWord} gives new best word score {bestWordScore} for this pattern");
                        }
                    }
                    //Debug.WriteLine("End of pattern");

                    if (bestWordScore > bestPatternScore) // Don't use the bestScoringPatternHolder here as it might be null
                    {
                        p.BestScoringWord = bestWord;
                        bestPatternScore = bestWordScore;
                        p.BestScore = bestWordScore;
                        bestScoringPatternHolder = p;
                        Console.WriteLine($"  >>  New best pattern: {bestScoringPatternHolder.BestScoringWord} gives new best word score {bestScoringPatternHolder.BestScore}");

                        //Debug.WriteLine($"  >>  New best pattern: {bestScoringPatternHolder.BestScoringWord} gives new best word score {bestScoringPatternHolder.BestScore}");
                    }
                }
            }

            //Debug.WriteLine("Starting length 2");
            Console.WriteLine("Starting length 2");

            for (int i = 1; i < 11; i++) // Working with 1 based arrays with soft edges (i.e. indexes zero and eleven are legal and return empty/zero)
            {
                for (int j = 1; j < 11; j++)
                {
                    PatternHolder temp;
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.XChanging, 2, 2, 1);
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
                    temp = AIHelpers.WhatCanIPlay(softTiles, softHeights, i, j, Directions.YChanging, 2, 2, 1);
                    if (temp != null)
                    {
                        temp.MaxScore = AIHelpers.ComputePatternMaxScore(temp.ThePattern);
                        patternHolders.Add(temp);
                    }
                }
            }

            foreach (PatternHolder p in patternHolders)
            {
                //Debug.WriteLine("New pattern");

                if (p.MaxScore < bestPatternScore - thresholdAdjustment)
                {
                    //Debug.Print($"  ** Pattern eliminated {p.ThePattern[0].Letters}. Max score is {p.MaxScore} and criterion is {bestPatternScore - thresholdAdjustment}");
                }

                else
                {
                    string regexLookup = AIHelpers.GenerateLookup(p.ThePattern, Rack);
//Console.WriteLine("Lookup..." + regexLookup);
                    //String dictionary = "FADE FACE BAG CAG CAD BAD PARE PARC AD BAGFADE BAGFACE BATED RATED GATED BATEE EPARE CARE AR PA PE TAR";
                    string dictionary = TheDictionary.wordList;

                    MatchCollection mm = Regex.Matches(dictionary, regexLookup);
                    if (mm.Count == 0)
                    {
                        //Debug.WriteLine("  No words found");
                    }

                    foreach (Match m in mm) // Don't need to embed this in an else from mm.Count == 0 as the foreach will not happen in that case
                    {
                        //Debug.WriteLine($"  Word found >> {m.Value}");

                        // Check for found word same as exiting word
                        if (m.Value == p.ThePattern[0].Letters)
                        {
                            //Debug.WriteLine($"  **  Word eliminated - regex word {m.Value} and board word {p.ThePattern[0].Letters} are the same");
                            break; //Exit the foreach (Match m in mm)
                        }

                        // Check the required letters against the rack

                        // Need to take the board letters out before checking against the rack
                        char[] foundWord = m.Value.ToCharArray();
                        char[] existingTiles = p.ThePattern[0].Letters.ToCharArray();
                        char[] intersectionsPattern = p.ThePattern[1].Letters.ToCharArray();
                        char[] tilesNeeded = new char[foundWord.Length + 1];

                        int tilesNeededCount = 0; // Also used later for scoring as it is the number of new tiles played
                        for (int i = 0; i < foundWord.Length; i++)
                        {
                            if (existingTiles[i] != foundWord[i])
                            {
                                tilesNeeded[tilesNeededCount++] = foundWord[i]; // Works as the index because we post increment
                            }
                        }
                        Array.Resize(ref tilesNeeded, tilesNeededCount);

                        if (!AIHelpers.CheckRack(Rack, tilesNeeded))
                        {
                            //Debug.WriteLine($"  **  Word eliminated - tiles {new string(tilesNeeded)} not achievable from rack {boardRack}");
                            break; //Exit the foreach (Match m in mm)
                        }

                        //Debug.WriteLine($"  >>  Passes rack test");

                        // Not yet checking for covering whole words

                        // Check intersecting words
                        int intersectingPattern = 2;
                        int scoreForIntersectingWords = 0;

                        for (int i = 0; i < p.ThePattern[1].Letters.Length; i++)
                        {
                            char c = intersectionsPattern[i];
                            if (c == '!' || c == '+' && p.ThePattern[0].Letters[i] != foundWord[i])
                            {
                                char[] intersectingWord = p.ThePattern[intersectingPattern].Letters.ToCharArray();
                                int pos = Array.FindIndex(intersectingWord, x => x == c);
                                intersectingWord[pos] = foundWord[i];
                                string lookup = @"\b" + new string(intersectingWord) + @"\b";
                                Match mx = Regex.Match(dictionary, lookup);
                                if (!mx.Success)
                                {
                                    //Debug.WriteLine($"  **  Fails intersection test");
                                    break;
                                }
                                else
                                {
                                    scoreForIntersectingWords += (p.ThePattern[intersectingPattern].Score + 1);
                                    // Check for flat word bonus - only possible if the intersection is of type "!" (Intersection on a empty square)
                                    if (c == '!' && p.ThePattern[intersectingPattern].Score == p.ThePattern[intersectingPattern].Letters.Length - 1)
                                    {
                                        scoreForIntersectingWords += (p.ThePattern[intersectingPattern].Score + 1);
                                        //Debug.WriteLine($"  >>  Intersecting word {new string(intersectingWord)} scores flat word bonus");
                                    }
                                    intersectingPattern++;
                                    //Debug.WriteLine($"  >>  Passes intersections test ({mx.Value}). Score of intersecting words now {scoreForIntersectingWords}");
                                }
                            }
                        }
                        //if (intersectingPattern == 2) Debug.WriteLine($"  >>  No intersections to check");

                        // End of checking intersecting words

                        // Now to work out the score

                        //Main word
                        //if (new string(foundWord) == "ONES")
                        //{
                        //    Debug.WriteLine($"ONES: p.ThePattern[1].Letters.Length =  {p.ThePattern[1].Letters.Length}");
                        //    Debug.WriteLine($"ONES: tilesNeededCount =  {tilesNeededCount}");
                        //}

                        int wordScore = p.ThePattern[1].Score + tilesNeededCount;
                        if (wordScore == p.ThePattern[1].Letters.Length)
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
                            //Debug.WriteLine($"  >>  {bestWord} gives new best word score {bestWordScore} for this pattern");
                        }
                    }
                    //Debug.WriteLine("End of pattern");

                    if (bestWordScore > bestPatternScore) // Don't use the bestScoringPatternHolder here as it might be null
                    {
                        p.BestScoringWord = bestWord;
                        bestPatternScore = bestWordScore;
                        p.BestScore = bestWordScore;
                        bestScoringPatternHolder = p;
                        Console.WriteLine($"  >>  New best pattern: {bestScoringPatternHolder.BestScoringWord} gives new best word score {bestScoringPatternHolder.BestScore}");

                        //Debug.WriteLine($"  >>  New best pattern: {bestScoringPatternHolder.BestScoringWord} gives new best word score {bestScoringPatternHolder.BestScore}");
                    }
                }
            }
            PatternHolder ph = bestScoringPatternHolder;

            Console.WriteLine("");
            Console.WriteLine($"Best pattern: {ph.BestScoringWord} gives new best word score {ph.BestScore}");
            Console.WriteLine("");

            foreach (Pattern p in ph.ThePattern)
            {
                Console.WriteLine($"Pattern: {p.Letters}");
            }

            char[] charBestScoringWord = ph.BestScoringWord.ToCharArray();
            char[] charBestExistingTiles = ph.ThePattern[0].Letters.ToCharArray();
          
            for (int i=0; i< ph.BestScoringWord.Length; i++)
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
                    boardTiles[thisX-1,thisY-1] = charBestScoringWord[i].ToString().ToLower();
                }
            }

            Console.WriteLine("");
            Console.WriteLine("AIworker ending");
            Console.WriteLine("");

            return ph.BestScoringWord;
        }
    }
}
