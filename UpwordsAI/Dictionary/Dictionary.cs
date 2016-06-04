using System;
using System.IO;
using System.Reflection;

namespace UpwordsAI.Dictionary
{
    class Dictionary
    {
        public string WordList { get; }

        public Dictionary()
        {
            WordList = ReadDictionary();
        }

        private string ReadDictionary()
        {
            //If there is a words.txt file in the appropriate place, use that. Otherwise use the built in dictionary
            if (File.Exists("Dictionary/words.txt"))
            {
                return File.ReadAllText("Dictionary/words.txt");
            }
            else
            {
                //Open a manifest resource stream. This accesses the words.txt file which has it's built setting as "embedded resource"
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UpwordsAI.Dictionary.words.txt"))
                {
                    if (stream == null)
                        throw new InvalidOperationException("Default dictionary not found!");

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public bool LookupWord (string find)
        {
            return WordList.Contains(" " + find + " ");
        }
    }
}
