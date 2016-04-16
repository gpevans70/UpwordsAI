using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpwordsAI.AI;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace UpwordsAI.Tests.AI
{
    [TestClass]
    public class PatternTest
    {
        Pattern _pattern;

        [TestInitialize]
        public void Initialize()
        {
            _pattern = new Pattern("",0);
        }

        //[TestMethod]
        //public void AssertThat_LettersPropertyIsNull_WithUninitializedPattern()
        //{
        //    Assert.AreEqual(null, _pattern.Letters);
        //}

        [TestMethod]
        public void AssertThat_SetLettersProperty_SetsLettersProperty()
        {
            //string dictionary = File.ReadAllText("C:/Users/Public/Documents/words.txt");

            //String pattern = @"\b" + "[zx][abcdefgo][abcdefgo]" + @"\b";
            //String dictionary = "FADE FACE BAG CAG CAD BAD AD BAGFADE BAGFACE BATED RATED GATED BATEE";

            //Match m = Regex.Match(dictionary, pattern);
            //Assert.IsTrue(m.Success);

            //const string testString = "Hello";

            string testString = "ACE";

            _pattern.Letters = testString;

            Assert.AreEqual(testString, _pattern.Letters);
        }
    }
}
