using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpwordsAI.AI
{
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
                if (patternCount > 0) maxScore += p.Score;

                // Track the lowest score of the '+' intersecting words
                if (patternCount > 1 && patterns[1].Letters.ElementAt(patternCount-1) == '+') minScore = Math.Min( p.Score, minScore);

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
            // If there's more than one exisitng board letter, then we have covered a whole existing word and we have to make a downward adjustment for not doing that
            int boardLettersCount = 0;
            int intersectingBoardLettersCount = 0;
            for (int i=0; i < patterns[0].Letters.Length; i++)
            {
                if (patterns[0].Letters.ElementAt(i) != '*') // In patterns[0] if it's not * (empty space) it's an existing letter
                {
                    boardLettersCount++;
                    if (patterns[1].Letters.ElementAt(i) == '+') intersectingBoardLettersCount++;
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
    }
}
