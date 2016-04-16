using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UpwordsAI.AI;

namespace UpwordsAI.Dictionary
{
    class Dictionary
    {
        public string wordList;

        public Dictionary()
        {
             wordList = File.ReadAllText("C:/Users/Public/Documents/words.txt");

        }

        public bool LookupWord (string find)
        {
            return wordList.Contains(" " + find + " ");
        }




    }
}
