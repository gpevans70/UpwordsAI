using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpwordsAI.AI;
using UpwordsAI.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UpwordsAI.Tests.Dictionary
{
    [TestClass]
    public class DictionaryTest
    {

        UpwordsAI.Dictionary.Dictionary _dictionary;

        [TestInitialize]
        public void Initialize()
        {
            _dictionary = new UpwordsAI.Dictionary.Dictionary();



        }


        [TestMethod]
            public void AssertThat_SetLettersProperty_SetsLettersProperty()
            {
                string dictionary = File.ReadAllText("C:/Users/Public/Documents/words.txt");

            //    String pattern = @"\b" + "[zx][abcdefgo][abcdefgo]" + @"\b";
            //    //String dictionary = "FADE FACE BAG CAG CAD BAD AD BAGFADE BAGFACE";

            //    Match m = Regex.Match(dictionary, pattern);
            //    Assert.IsTrue(m.Success);

            //    const string testString = "Hello";

            //    _pattern.Letters = testString;

            //Assert.AreEqual(testString, _pattern.Letters);
        }
    }
}
