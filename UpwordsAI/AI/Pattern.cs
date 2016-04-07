using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpwordsAI.AI
{
    public class Pattern
    {
        internal Pattern (string TheLetters, int TheScore)
        {
            Letters = TheLetters;
            Score = TheScore;
        }

        public string Letters { get; internal set;}
        public int Score { get; internal set; }
    }
}
