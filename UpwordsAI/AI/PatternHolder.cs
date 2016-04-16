using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpwordsAI.AI
{
    internal class PatternHolder
    {
        internal List<Pattern> ThePattern { get; set; }
        internal int XIn { get; set; } // Range 1 to 10
        internal int YIn { get; set; } // Range 1 to 10
        internal int TheLocationX { get; set; } // Range 1 to 10
        internal int TheLocationY { get; set; } // Range 1 to 10
        internal Directions TheDirection { get; set; }
        internal int LengthIn { get; set; }
        internal int TheLength { get; set; }
        internal int TheMaxTiles { get; set; }
        internal int TheMinTiles { get; set; }
        internal int MaxScore { get; set; }
        internal string BestScoringWord { get; set; }
        internal int BestScore { get; set; }
    }
}
