using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpwordsAI.AI
{
    public enum Directions
    {
        XChanging,
        YChanging,
    }


    public static class AIHelpers
    {
        public static int ComputePatternMaxScore(List<Pattern> patterns)

// ************** Not yet testing for flat words x2 bonus ******************************

        {
            int maxScore = 0;
            int minScore = 1000;
            int patternCount = 0;

            foreach (Pattern p in patterns)
            {
                // p.Score is the score for the existing tiles with no letters added
                if (patternCount >= 1) maxScore += p.Score;

                // Track the lowest score of the '+' intersecting words
                if (patternCount >= 2 && patterns[1].Letters.ElementAt(patternCount-2) == '+') minScore = Math.Min( p.Score, minScore);

                patternCount++;
            }          
            foreach (var c in patterns[1].Letters) // Work out the score contribution from letters being played
            {
                switch (c)
                {
                    case '*': maxScore += 1; break; // Play on an empty space, non-intersecting - one point for the new tile
                    case '&': maxScore += 1; break; // Play on an existing tile, non intersecting - one point for the new tile 
                    case '!': maxScore += 2; break; // Play on an empty space, interlocking - two points for the new tile (one point in each word)
                    case '+': maxScore += 2; break; // Play on an existing tile, interlocking - two points for the new tile (one point in each word)
                    default: break;
                }
            }

            // The maxScore so far is based on doing as much as possible.
            // If there's more than one existing board letter, then we have covered a whole existing word and we have to make a downward adjustment for not doing that
            // This logic is not perfect - there are some patterns which will fool it (with internal spaces) but this is not the end of the world as this is only filtering
            // the available moves. The actual scoring will be done when the tiles are played onto the board with more robust logic.

            int boardLettersCount = 0;
            int intersectingBoardLettersCount = 0;
            for (int i=0; i < patterns[0].Letters.Length; i++)
            {
                if (patterns[0].Letters.ElementAt(i) != '*') // In patterns[0] if it's not * (empty space) it's an existing letter
                {
                    boardLettersCount++;
                    if (patterns[1].Letters.ElementAt(i) == '+') intersectingBoardLettersCount++;
                }
                if (patterns[1].Letters.ElementAt(i) >= 'A' && patterns[1].Letters.ElementAt(i) <= 'Z') // If there's a letter in pattern 1, then 
                {                                                                                       // this will not be covered, so not adjustment is required                                                                    
                    boardLettersCount=-99; // As boardLettersCount > 1 is the test for an adjustment, this will ensure that there is not adjustment
                    break;                 // No need to keep counting                             
                }
            }
            if (boardLettersCount > 1) // If there's more than one existing board letter, then there's a word we have covered in computing maxScore so far
            {
                // If the counts are equal, then all the covered board letters are intersections - adjust accordingly
                if ( boardLettersCount == intersectingBoardLettersCount) 
                {
                    maxScore -= minScore; // Deduct for the lowest intersecting word
                    maxScore -= 2;        // Deduct for the intersecting tile played from the rack
                }
                else // There's a non-intersecting letter we can not cover
                {
                    maxScore -= 1;        // Deduct for the intersecting tile played from the rack
                }
            }
            return maxScore;
        }

        public static bool CheckRack(char[] Rack, char[] TilesNeeded, out char[] tilesLeftOnRack)
        {
            char[] disposableRack = new char[Rack.Length];
            Rack.CopyTo(disposableRack, 0);

            foreach (char c in TilesNeeded)
            {
                int pos = Array.FindIndex(disposableRack, x => x == c);
                if (pos < 0)
                {
                    tilesLeftOnRack = null;
                    return false;
                }
                else
                {
                    disposableRack[pos] = ' ';
                }
            }
            var tiles =     from c in disposableRack
                            where c != ' '
                            select c;
            tilesLeftOnRack = tiles.ToArray();
            return true;
        }

        internal static PatternHolder WhatCanIPlay(char[,] softTiles, int[,] softHeights, int locationX, int locationY, Directions direction, int length, int maxTiles, int minTiles)        
        {
            int WorkingLength, WorkingX, WorkingY;

            if (    locationX < 1 ||
                    locationX > 10 ||
                    locationY < 1 ||
                    locationY > 10 ||
                    locationX + length > 11 ||
                    locationY + length > 11) return null;

            if (length == 1)
            {
                if (direction == Directions.XChanging)
                {
                    WorkingX = locationX - CountAdjacenciesXBefore(softTiles, locationX, locationY);
                    WorkingY = locationY;
                    WorkingLength = locationX - WorkingX // CountAdjacenciesXBefore(softTiles, locationX, locationY)
                                  + 1
                                  + CountAdjacenciesXAfter(softTiles, locationX, locationY);
                }
                else
                {
                    if (CountAdjacenciesXBefore(softTiles, locationX, locationY) > 0 || CountAdjacenciesXAfter(softTiles, locationX, locationY) > 0)
                        return null; // If something in both directions, return null for YChanging (already dealt with under XChanging)
                    WorkingX = locationX;
                    WorkingY = locationY - CountAdjacenciesYBefore(softTiles, locationX, locationY);
                    WorkingLength = locationY - WorkingY // CountAdjacenciesYBefore(softTiles, locationX, locationY)
                                  + 1
                                  + CountAdjacenciesYAfter(softTiles, locationX, locationY);
                }
                if (WorkingLength == 1) return null; // If nothing in this direction, return null
            }
            else
            {
                WorkingLength = length;
                WorkingX = locationX;
                WorkingY = locationY;

                // Check for tiles adjacent to where this X and Y and direction and length is wanting to play
                // Adjacent tiles at either end cause an exit because that is a different length

                if (direction==Directions.XChanging)
                {
                    // Check at each end
                    if (softHeights[WorkingX-1, WorkingY] > 0) { return null; }
                    if (softHeights[WorkingX + WorkingLength, WorkingY] > 0) { return null; }

                    bool canplayhere = false;
                    // Look along the length of the proposed place to play and the orthogonally adjacent squares for each place
                    for (int i = WorkingX; i < WorkingX + WorkingLength; i++)
                    {
                        if (softHeights[i, WorkingY - 1] > 0) { canplayhere = true; break; }
                        if (softHeights[i, WorkingY    ] > 0) { canplayhere = true; break; }
                        if (softHeights[i, WorkingY + 1] > 0) { canplayhere = true; break; }
                    }
                    if (!canplayhere) return null;
                }
                else
                {
                    // Check at each end
                    if (softHeights[WorkingX, WorkingY-1] > 0) { return null; }
                    if (softHeights[WorkingX, WorkingY+WorkingLength] > 0) { return null; }

                    bool canplayhere = false;
                    // Look along the length of the proposed place to play and the orthogonally adjacent squares for each place
                    for (int i = WorkingY; i < WorkingY + WorkingLength; i++)
                    {
                        if (softHeights[WorkingX - 1, i] > 0) { canplayhere = true; break; }
                        if (softHeights[WorkingX, i] > 0) { canplayhere = true; break; }
                        if (softHeights[WorkingX + 1, i] > 0) { canplayhere = true; break; }
                    }
                    if (!canplayhere) return null;
                }
            }

            PatternHolder ThePatternHolder = new PatternHolder();
            ThePatternHolder.ThePattern = new List<Pattern>();
            ThePatternHolder.XIn = locationX;
            ThePatternHolder.YIn = locationY;
            ThePatternHolder.TheLocationX = WorkingX;
            ThePatternHolder.TheLocationY = WorkingY;
            ThePatternHolder.TheDirection = direction;
            ThePatternHolder.LengthIn = length;
            ThePatternHolder.TheLength = WorkingLength;
            ThePatternHolder.TheMaxTiles = maxTiles;
            ThePatternHolder.TheMinTiles = minTiles;

            // Calculate the first pattern (what is presently there)
            string buildPattern = "";
            int score = 0;
            if (direction == Directions.XChanging)
            {
                for (int i= WorkingX; i < WorkingX+WorkingLength; i++)
                {
                    if (softTiles[i, WorkingY] == ' ')
                    {
                        buildPattern += '*';
                    }
                    else
                    {
                        buildPattern += softTiles[i, WorkingY];
                        score += softHeights[i, WorkingY];
                    }
                }
            }
            else
            {
                for (int i = WorkingY; i < WorkingY + WorkingLength; i++)
                {
                    if (softTiles[WorkingX, i] == ' ')
                    {
                        buildPattern += '*';
                    }
                    else
                    {
                        buildPattern += softTiles[WorkingX,i];
                        score += softHeights[WorkingX,i];
                    }
                }
            }
            ThePatternHolder.ThePattern.Add(new Pattern(buildPattern, score));

            // Calculate the second pattern (intersections along what is presently there)
            // Don't need to recalculate the score
            buildPattern = "";
            if (direction == Directions.XChanging)
            {
                for (int i = WorkingX; i < WorkingX + WorkingLength; i++)
                {
                    if (softHeights[i,WorkingY]> 4 // If the stack is already at maximum height
                        || (length == 1 && i!=locationX)) // Or this is length one and we're not at the input location
                    {
                        buildPattern += softTiles[i, WorkingY];
                        continue;
                    }

                    if (softTiles[i, WorkingY+1] == ' ' && softTiles[i, WorkingY-1] == ' ')
                    {                                                                     // Both adjacent squares are empty so no interlocking
                        if (softTiles[i, WorkingY] == ' ')
                            buildPattern += '*'; // Empty square, not interlocking
                        else
                            buildPattern += '&'; // Cover existing tile, not interlocking
                    }
                    else
                    {                                                                     // One of the adjacent squares in not empty so interlocking
                        if (softTiles[i, WorkingY] == ' ')
                            buildPattern += '!'; // Empty square, interlocking
                        else
                            buildPattern += '+'; // Cover existing tile, interlocking
                    }
                }
            }
            else
            {
                for (int i = WorkingY; i < WorkingY + WorkingLength; i++)
                {
                    if (softHeights[WorkingX,i] > 4  // If the stack is already at maximum height
                        || (length == 1 && i != locationY)) // Or this is length one and we're not at the input location
                    {
                        buildPattern += softTiles[WorkingX,i];
                        continue;
                    }

                    if (softTiles[WorkingX + 1,i] == ' ' && softTiles[WorkingX - 1,i] == ' ')
                    {                                                                     // Both adjacent squares are empty so no interlocking
                        if (softTiles[WorkingX,i] == ' ')
                            buildPattern += '*'; // Empty square, not interlocking
                        else
                            buildPattern += '&'; // Cover existing tile, not interlocking
                    }
                    else
                    {                                                                     // One of the adjacent squares in not empty so interlocking
                        if (softTiles[WorkingX,i] == ' ')
                            buildPattern += '!'; // Empty square, interlocking
                        else
                            buildPattern += '+'; // Cover existing tile, interlocking
                    }
                }
            }

            ThePatternHolder.ThePattern.Add(new Pattern(buildPattern, score));

            // Calculate the third and subsequent patterns, if any (intersections  patterns)

            if (direction == Directions.XChanging)
            {
                for (int i = WorkingX; i < WorkingX + WorkingLength; i++)
                {
                    char c = ThePatternHolder.ThePattern[1].Letters[i - WorkingX];
                    if (c == '!' || c == '+') // The two interlocking codes
                    {
                        buildPattern = "";
                        score = 0;
                        int before = CountAdjacenciesYBefore(softTiles, i, WorkingY);
                        int after = CountAdjacenciesYAfter(softTiles, i, WorkingY);
                        for (int j = WorkingY - before; j< WorkingY; j++)
                        {
                            buildPattern += softTiles[i, j];
                            score += softHeights[i, j];
                        }
                        buildPattern += c;
                        score += softHeights[i, WorkingY];

                        for (int j = WorkingY + 1 ; j < WorkingY + after + 1; j++)
                        {
                            buildPattern += softTiles[i, j];
                            score += softHeights[i, j];
                        }
                        ThePatternHolder.ThePattern.Add(new Pattern(buildPattern, score));
                    }
                }
            }
            else
            {
                for (int i = WorkingY; i < WorkingY + WorkingLength; i++)
                {
                    char c = ThePatternHolder.ThePattern[1].Letters[i - WorkingY];
                    if (c == '!' || c == '+')
                    {
                        buildPattern = "";
                        score = 0;
                        int before = CountAdjacenciesXBefore(softTiles, WorkingX, i);
                        int after = CountAdjacenciesXAfter(softTiles, WorkingX, i);
                        for (int j = WorkingX - before; j < WorkingX; j++)
                        {
                            buildPattern += softTiles[j, i];
                            score += softHeights[j, i];
                        }
                        buildPattern += c;
                        score += softHeights[WorkingX, i];

                        for (int j = WorkingX + 1; j < WorkingX + after + 1; j++)
                        {
                            buildPattern += softTiles[j, i];
                            score += softHeights[j, i];
                        }
                        ThePatternHolder.ThePattern.Add(new Pattern(buildPattern, score));
                    }
                }
            }
            ThePatternHolder.TheLength = WorkingLength;
            ThePatternHolder.TheLocationX = WorkingX;
            ThePatternHolder.TheLocationY = WorkingY;
            return ThePatternHolder;
        }

        internal static string GenerateLookup (List<Pattern> ThePattern, char[] TheRack)
        {
            char[] deduplicatedRack = new char[TheRack.Length];

            // Generate the deduplicated rack and trim to only used positions
            foreach (char c in TheRack) 
            {
                int pos = Array.FindIndex(deduplicatedRack, x => x == c);
                if (pos < 0)
                {
                    pos = Array.FindIndex(deduplicatedRack, x => x == '\0');
                    deduplicatedRack[pos] = c;
                }
            }
            string RackString = "[" +  (new string(deduplicatedRack)).Trim('\0');

            // Check how many non-letters are in pattern[1] - if it's only one (i.e. only one rack letter to be used) we need to do something different later

            int maxRackLetters = 0;
            for (int i=0; i < ThePattern[1].Letters.Length; i++)
            {
                if (ThePattern[1].Letters[i] < 'A') // This covers all the non-letter characters
                {
                    maxRackLetters++;
                }
            }

            string result = "";
            for (int i = 0; i < ThePattern[1].Letters.Length; i++)
            {
                if (ThePattern[1].Letters[i] >= 'A' && ThePattern[1].Letters[i] <= 'Z') // If it's a letter in pattern 1, add it to the pattern
                {
                    result += ThePattern[1].Letters[i];
                }
                else // If it's not a letter in pattern 1, then it could be a rack letter of (if applicable) a board letter
                {
                    result += RackString;
                    if (ThePattern[0].Letters[i] >= 'A' && ThePattern[1].Letters[i] <= 'Z' && maxRackLetters > 1)
                    {
                        result += ThePattern[0].Letters[i];
                    }
                    result += "]";
                }
            }
            return @"\b" + result + @"\b";
        }

        internal static int CountAdjacenciesXBefore(char [,] position, int X, int Y)
        {
            int value = 0;
            for (int i = X - 1; i >= 0; i--)
            {
                char neighbour = position[i, Y];
                if (neighbour >= 'A' && neighbour <= 'Z')
                    value++;
                else
                    break;
            }
            return value;
        }

        internal static int CountAdjacenciesXAfter(char[,] position, int X, int Y)
        {
            int value = 0;
            for (int i = X + 1; i < 12; i++)
            {
                char neighbour = position[i, Y];
                if (neighbour >= 'A' && neighbour <= 'Z')
                    value++;
                else
                    break;
            }
            return value;
        }

        internal static int CountAdjacenciesYBefore(char[,] position, int X, int Y)
        {
            int value = 0;
            for (int i = Y - 1; i >= 0; i--)
            {
                char neighbour = position[X, i];
                if (neighbour >= 'A' && neighbour <= 'Z')
                    value++;
                else
                    break;
            }
            return value;
        }

        internal static int CountAdjacenciesYAfter(char[,] position, int X, int Y)
        {
            int value = 0;
            for (int i = Y + 1; i < 12; i++)
            {
                char neighbour = position[X, i];
                if (neighbour >= 'A' && neighbour <= 'Z')
                    value++;
                else
                    break;
            }
            return value;
        }


    }
}
