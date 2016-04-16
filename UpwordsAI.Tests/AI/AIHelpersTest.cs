using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpwordsAI.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpwordsAI.Tests.AI
{
    [TestClass]
    public class AIHelpersTest
    {
        List<Pattern> _patternList;
        public char[,] _position1;
        public int[,] _position1Heights;

        [TestInitialize]
        public void Initialize()
        {
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };

            _position1Heights = new int[12, 12]
            { //  0  1  2  3  4  5  6  7  8  9 10 11    
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 0
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 1
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,  },  // 2
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,  },  // 3
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,  },  // 4
                { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0,  },  // 5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 7
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 8
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 9
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  //10
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  //11
            };
        }


        [TestMethod]
        public void AssertThat_GenerateLookupReturnsCorrectAnswer1()
        {

            char[] _rack = new char[] { 'A', 'B', 'D', 'D', 'E', 'F', 'G' };

            _patternList = new List<Pattern> // Pattern List 1 - simple crossing of existing word
            {
                {new Pattern ("RAT**", 0) },
                {new Pattern ("*A+**", 1) }, // Heights 0,0,1,0,0 (before any new tiles played)
                {new Pattern ("RS+UV", 5) }, // Heights 1,1,1,1,1 (before any new tiles played)
            };

             Assert.AreEqual(@"\b[ABDEFGR]A[ABDEFGT][ABDEFG][ABDEFG]\b", AIHelpers.GenerateLookup(_patternList, _rack));
        }

        [TestMethod]
        public void AssertThat_GenerateLookupReturnsCorrectAnswer2() // NB Board letter not included in [ABDEFG] as only one tile can be played
        {

            char[] _rack = new char[] { 'A', 'B', 'D', 'D', 'E', 'F', 'G' };

            _patternList = new List<Pattern> // Pattern List 1 - simple crossing of existing word
            {
                {new Pattern ("PARE", 0) },
                {new Pattern ("PA+E", 1) }, // Heights 0,0,1,0,0 (before any new tiles played)
                {new Pattern ("+AP", 5) }, // Heights 1,1,1,1,1 (before any new tiles played)
            };

            Assert.AreEqual(@"\bPA[ABDEFG]E\b", AIHelpers.GenerateLookup(_patternList, _rack));
        }

        [TestMethod]
        public void AssertThat_GenerateLookupReturnsCorrectAnswer3() // NB Board letter not included in [ABDEFG] as only one tile can be played
        {

            char[] _rack = new char[] { 'A', 'B', 'D', 'D', 'E', 'F', 'G' };

            _patternList = new List<Pattern> // Pattern List 1 - simple crossing of existing word
            {
                {new Pattern ("PARE", 0) },
                {new Pattern ("PA++", 1) }, // Heights 0,0,1,0,0 (before any new tiles played)
                {new Pattern ("+AP", 5) }, // Heights 1,1,1,1,1 (before any new tiles played)
                {new Pattern ("+AP", 5) }, // Heights 1,1,1,1,1 (before any new tiles played)
            };

            Assert.AreEqual(@"\bPA[ABDEFGR][ABDEFGE]\b", AIHelpers.GenerateLookup(_patternList, _rack));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList1returns12()
        {
            _patternList = new List<Pattern> // Pattern List 1 - simple crossing of existing word
            {
                {new Pattern ("**T**", 0) },
                {new Pattern ("**+**", 1) }, // Heights 0,0,1,0,0 (before any new tiles played)
                {new Pattern ("RS+UV", 5) }, // Heights 1,1,1,1,1 (before any new tiles played)
            };
            Assert.AreEqual(12, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList2returns10()
        {
            _patternList = new List<Pattern> // Pattern List 2 - simple crossing past end of existing word
            {
                {new Pattern ("******", 0) },
                {new Pattern ("**!***", 0) }, // Heights 0,0,0,0,0,0 (before any new tiles played)
                {new Pattern ("ABC!", 3)   }, // Heights 1,1,1,0 (before any new tiles played)
            };
            Assert.AreEqual(10, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList3returns13()
        {
            _patternList = new List<Pattern> // Pattern List 3 - new word overlays existing two letter word
            {
                {new Pattern ("*FZ**", 0)  },
                {new Pattern ("*++**", 2)  }, // Heights 0,1,1,0,0 (before any new tiles played)
                {new Pattern ("ABCDE+", 6) }, // Heights 1,1,1,1,1,1 (before any new tiles played)
                {new Pattern ("XY+", 3)    }, // Heights 1,1,1 (before any new tiles played)
            };
            Assert.AreEqual(13, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList4returns16()
        {
            _patternList = new List<Pattern> // Pattern List 4 - new word overlays existing two letter word (unequal heights)
            {
                {new Pattern ("*EZ**", 0)  }, 
                {new Pattern ("*++**", 5)  }, // Heights 0,4,1,0,0 (before any new tiles played)
                {new Pattern ("ABCD+", 5)  }, // Heights all one
                {new Pattern ("XY+", 6)    }, // Heights 1,1,4 (before any new tiles played)
            };
            Assert.AreEqual(16, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList4areturns17()
        {
            _patternList = new List<Pattern> // Pattern List 4a - new word overlays existing three letter word
            {
                {new Pattern ("*ATE*", 0)   },
                {new Pattern ("*+++*", 3)   }, // Heights 0,1,1,1,0 (before any new tiles played)
                {new Pattern ("SUG+R", 5)   }, // Heights 1,1,1,1,1 (before any new tiles played)
                {new Pattern ("FA+", 3)     }, // Heights 1,1,1 (before any new tiles played)
                {new Pattern ("B+", 1)      }, // Heights 1,1 (before any new tiles played)
            };
            Assert.AreEqual(17, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList5returns13()
        {
            _patternList = new List<Pattern> // Pattern List 5 - new word overlays existing two letter word (unequal heights but not at the crossing point)
            {
                {new Pattern ("*EZ**", 0)  }, 
                {new Pattern ("*++**", 2)  }, // Heights 0,1,1,0,0 (before any new tiles played)
                {new Pattern ("ABCD+", 5)  }, // Heights all one
                {new Pattern ("XY+", 6)    }, // Heights 4,1,1 (before any new tiles played)
            };
            Assert.AreEqual(13, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList6returns14()
        {
            _patternList = new List<Pattern> // Pattern List 6 - complex crossing and intersecting
            {
                {new Pattern ("*AT**", 0)   },
                {new Pattern ("*++!*", 2)   }, // Heights 0,1,1,0,0 (before any new tiles played)
                {new Pattern ("SUG+R", 5)   }, // Heights 1,1,1,1,1 (before any new tiles played)
                {new Pattern ("FA+", 3)     }, // Heights 1,1,1 (before any new tiles played)
                {new Pattern ("B!", 1)      }, // Heights 1,0 (before any new tiles played)
            };
            Assert.AreEqual(14, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList7returns15()
        {
            _patternList = new List<Pattern> // Pattern List 7 - another complex example
            {
                {new Pattern ("*R***", 0) },
                {new Pattern ("*+!**", 2) }, // Heights 0,1,0,0,0 (before any new tiles played)
                {new Pattern ("CA+", 3)   }, // Heights 1,1,1 (before any new tiles played)
                {new Pattern ("FAT!", 3)  }, // Heights 1,1,1,0 (before any new tiles played)
            };
            Assert.AreEqual(15, AIHelpers.ComputePatternMaxScore(_patternList));
        }

        [TestMethod]
        public void AssertThat_ComputePatternMaxScoreof_patternList8returns14()
        {
            _patternList = new List<Pattern> // Pattern List 8 - another complex example
            {
                {new Pattern ("PARE", 7) },
                {new Pattern ("PAR+", 7) }, // Heights 1,4,1,1 (before any new tiles played)
                {new Pattern ("FAC+S", 5)   }, // Heights 1,1,1,1,1 (before any new tiles played)
            };
            Assert.AreEqual(14, AIHelpers.ComputePatternMaxScore(_patternList));
        }


        [TestMethod]
        public void AssertThat_RackDoesNotChangeRack()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            char[] _startrack = _rack;

            char[] _tilesneeded = new char[] { 'A', 'B', 'C' };

            AIHelpers.CheckRack(_rack, _tilesneeded);

            Assert.AreEqual(_startrack, _rack);
        }

        [TestMethod]
        public void AssertThat_RackHandlesOKSingles()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'A' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'C' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'D' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'E' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'F' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_RackHandlesNotOKSingles()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'H' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'T' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'Z' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _rack = new char[] { 'T', 'U', 'W', 'V', 'X', 'Y', 'Z' };

            _tilesneeded = new char[] { 'A' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'M' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'S' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _rack = new char[] { 'B', 'F', 'J', 'N', 'Q', 'T', 'Y' };

            _tilesneeded = new char[] { 'A' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'M' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'Z' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_RackHandlesOKDoubles()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'A', 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'A' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'A', 'D' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'D', 'A' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'A', 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'F' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'F', 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'C', 'G' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'C' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'A', 'G' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'A' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_RackHandlesNotOKDoubles()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'K', 'B' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'K' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'S', 'G' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'X' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'A', 'A' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'E', 'E' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'G' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_RackHandlesOKLongerLengths()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'A', 'B', 'C' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'A', 'F' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'A', 'D' , 'F', 'C' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'D', 'A', 'G', 'E', 'F' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'C' , 'F', 'E', 'D', 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'G', 'C', 'F', 'E', 'D', 'A', 'B' };
            Assert.AreEqual(true, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_RackHandlesNotOKLongerLengths()
        {
            char[] _rack = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

            var _tilesneeded = new char[] { 'B', 'B' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'K', 'A' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'C', 'F', 'S', 'G' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));

            _tilesneeded = new char[] { 'B', 'C', 'F', 'G', 'B', 'A' };
            Assert.AreEqual(false, AIHelpers.CheckRack(_rack, _tilesneeded));
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesXBeforeWorksCorrectlyAroundEmptySpaces()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 5, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 10, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 10, 6));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 10, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 7, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 7));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 2, 9));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 9, 2));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 8, 8));
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesYBeforeWorksCorrectlyAroundEmptySpaces()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 1, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 10, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 10, 6));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 10, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 7, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 1, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 1, 7));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 2, 9));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 9, 2));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 8, 8));
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesXAfterWorksCorrectlyAroundEmptySpaces()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 5, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 10, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 10, 6));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 10, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 7, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 7));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 2, 9));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 9, 2));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 8, 8));
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesYAfterWorksCorrectlyAroundEmptySpaces()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 1, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 10, 1));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 10, 6));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 10, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 7, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 1, 10));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 1, 7));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 2, 9));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 9, 2));
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 8, 8));
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesXBeforeWorksCorrectlyAroundExistingTiles()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 6)); // One up from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 2, 6)); // P of "PARE" counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesXBefore(_position1, 3, 6)); // A of "PARE" counting P
            Assert.AreEqual(2, AIHelpers.CountAdjacenciesXBefore(_position1, 4, 6)); // R of "PARE" counting PA
            Assert.AreEqual(3, AIHelpers.CountAdjacenciesXBefore(_position1, 5, 6)); // E of "PARE" counting PAR
            Assert.AreEqual(4, AIHelpers.CountAdjacenciesXBefore(_position1, 6, 6)); // After E of "PARE" counting PARE
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 7, 6)); // 2 after E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 5)); // One up and one left from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 2, 5)); // One left from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 3, 5)); // One left from A of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 4, 5)); // One left from R of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 5, 5)); // One left from E of "PARE" (C of "FACE") counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesXBefore(_position1, 6, 5)); // One down and one left from E of "PARE" counting C of "FACE"
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 7, 5)); // Two down and one left E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 1, 7)); // One up and one right from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 2, 7)); // One right from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 3, 7)); // One right from A of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 4, 7)); // One right from R of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 5, 7)); // One right from E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 6, 7)); // One down and one right from E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXBefore(_position1, 7, 7)); // Two down and one right E of "PARE" counting nothing
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesYBeforeWorksCorrectlyAroundExistingTiles()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 2)); // One left from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 3)); // F of "FACE" counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 4)); // A of "FACE" counting F
            Assert.AreEqual(2, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 5)); // C of "FACE" counting FA
            Assert.AreEqual(3, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 6)); // E of "FACE" counting FAC
            Assert.AreEqual(4, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 7)); // After E of "FACE" counting FACE
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 5, 8)); // 2 after E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 2)); // One up and one left from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 3)); // One up from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 4)); // One up from A of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 5)); // One up from C of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 6)); // One up from E of "FACE" (R of "PARE") counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 7)); // One up and one right from E of "FACE" counting R of "PARE"
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 4, 8)); // One up and two right E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 2)); // One down and one left from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 3)); // One down from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 4)); // One down from A of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 5)); // One down from C of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 6)); // One down from E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 7)); // One down and one right from E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYBefore(_position1, 6, 8)); // One down and two right E of "FACE" counting nothing
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesXAfterWorksCorrectlyAroundExistingTiles()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(4, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 6)); // One up from P of "PARE" counting PARE
            Assert.AreEqual(3, AIHelpers.CountAdjacenciesXAfter(_position1, 2, 6)); // P of "PARE" counting ARE
            Assert.AreEqual(2, AIHelpers.CountAdjacenciesXAfter(_position1, 3, 6)); // A of "PARE" counting RE
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesXAfter(_position1, 4, 6)); // R of "PARE" counting E
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 5, 6)); // E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 6, 6)); // After E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 5)); // One up and one left from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 2, 5)); // One left from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 3, 5)); // One left from A of "PARE" counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesXAfter(_position1, 4, 5)); // One left from R of "PARE" counting one (C of "FACE")
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 5, 5)); // One left from E of "PARE" (C of "FACE") counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 6, 5)); // One down and one left from E of "PARE"
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 1, 7)); // One up and one right from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 2, 7)); // One right from P of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 3, 7)); // One right from A of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 4, 7)); // One right from R of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 5, 7)); // One right from E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 6, 7)); // One down and one right from E of "PARE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesXAfter(_position1, 7, 7)); // Two down and one right E of "PARE" counting nothing
        }

        [TestMethod]
        public void AssertThat_CountAdjacenciesYAfterWorksCorrectlyAroundExistingTiles()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 1)); // Two left from F of "FACE" counting nothing
            Assert.AreEqual(4, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 2)); // One left from F of "FACE" counting FACE
            Assert.AreEqual(3, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 3)); // F of "FACE" counting ACE
            Assert.AreEqual(2, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 4)); // A of "FACE" counting CE
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 5)); // C of "FACE" counting E
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 6)); // E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 7)); // After E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 5, 8)); // 2 after E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 2)); // One up and one left from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 3)); // One up from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 4)); // One up from A of "FACE" counting nothing
            Assert.AreEqual(1, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 5)); // One up from C of "FACE" counting R of "PARE"
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 6)); // One up from E of "FACE" (R of "PARE") counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 7)); // One up and one right from E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 4, 8)); // One up and two right E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 2)); // One down and one left from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 3)); // One down from F of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 4)); // One down from A of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 5)); // One down from C of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 6)); // One down from E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 7)); // One down and one right from E of "FACE" counting nothing
            Assert.AreEqual(0, AIHelpers.CountAdjacenciesYAfter(_position1, 6, 8)); // One down and two right E of "FACE" counting nothing
        }

        [TestMethod]
        public void AssertThat_WCIPIsCheckedForLengthOne()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)
              // EDGE                                          EDGE  
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 [EDGE] (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11 [EDGE]
            };
            */

            PatternHolder _PH;

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 1, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 2, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 3, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 4, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 5, Directions.XChanging, 1, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 6, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 7, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 8, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 9, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 10, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 1, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 2, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 3, Directions.YChanging, 1, 1, 1));
//
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 4, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 5, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 6, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 7, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 8, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 9, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 10, Directions.YChanging, 1, 1, 1));

            _PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.XChanging, 1, 1, 1);
            Assert.AreEqual(5, _PH.TheLength);
            Assert.AreEqual(2, _PH.TheLocationX);
            Assert.AreEqual(6, _PH.TheLocationY);
            Assert.AreEqual("PARE*", _PH.ThePattern[0].Letters);
            Assert.AreEqual(4, _PH.ThePattern[0].Score);
            Assert.AreEqual("PARE*", _PH.ThePattern[1].Letters);
            Assert.AreEqual(2, _PH.ThePattern.Count);

            _PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 6, Directions.XChanging, 1, 1, 1);
            Assert.AreEqual(4, _PH.TheLength);
            Assert.AreEqual(2, _PH.TheLocationX);
            Assert.AreEqual(6, _PH.TheLocationY);
            Assert.AreEqual("PARE", _PH.ThePattern[0].Letters);
            Assert.AreEqual(4, _PH.ThePattern[0].Score);
            Assert.AreEqual("P&RE", _PH.ThePattern[1].Letters);
            Assert.AreEqual(2, _PH.ThePattern.Count);

            _PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 6, Directions.XChanging, 1, 1, 1);
            Assert.AreEqual(4, _PH.TheLength);
            Assert.AreEqual(2, _PH.TheLocationX);
            Assert.AreEqual(6, _PH.TheLocationY);
            Assert.AreEqual("PARE", _PH.ThePattern[0].Letters);
            Assert.AreEqual(4, _PH.ThePattern[0].Score);
            Assert.AreEqual("PAR+", _PH.ThePattern[1].Letters);
            Assert.AreEqual("FAC+", _PH.ThePattern[2].Letters);
            Assert.AreEqual(4, _PH.ThePattern[2].Score);

            _PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 5, Directions.XChanging, 1, 1, 1);
            Assert.AreEqual(2, _PH.TheLength);
            Assert.AreEqual(4, _PH.TheLocationX);
            Assert.AreEqual(5, _PH.TheLocationY);
            Assert.AreEqual("*C", _PH.ThePattern[0].Letters);
            Assert.AreEqual(1, _PH.ThePattern[0].Score);
            Assert.AreEqual("!C", _PH.ThePattern[1].Letters);

            _position1[5, 8] = 'X';
            _position1Heights[5, 8] = 1;

            _PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 7, Directions.YChanging, 1, 1, 1);
            Assert.AreEqual(6, _PH.TheLength);
            Assert.AreEqual(5, _PH.TheLocationX);
            Assert.AreEqual(3, _PH.TheLocationY);
            Assert.AreEqual("FACE*X", _PH.ThePattern[0].Letters);
            Assert.AreEqual(5, _PH.ThePattern[0].Score);


            //_PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 2, Directions.XChanging, 1, 1, 1);
            //Assert.AreEqual(1, _PH.TheLength);

            //_PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 9, Directions.YChanging, 1, 1, 1);
            //Assert.AreEqual(1, _PH.TheLength);
        }

        [TestMethod]
        public void AssertThat_WCIPIsCheckedForLengthTwo()
        {
            /*
            _position1 = new char[12, 12]
            { //   0    1   2   3   4   5   6   7   8   9   10  11 (second index)    
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 0 (first index)
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ',' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ',' ',' ',' ',' ','A',' ',' ',' ',' ',' ', }, // 3
                { ' ', ' ',' ',' ',' ',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ',' ','F','A','C','E',' ',' ',' ',' ',' ', }, // 5 
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 6
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 7
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //11
            };
            */

            //PatternHolder _PH;

            // NB all items below containing a 10 fail on bounds checking

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 1, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 1, Directions.XChanging, 2, 1, 1)); 
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 1, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 2, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 2, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 3, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 3, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 4, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 4, Directions.XChanging, 2, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 5, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 5, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 6, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 6, Directions.XChanging, 2, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 7, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 7, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 8, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 8, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 9, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 9, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 10, Directions.XChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 10, Directions.XChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 1, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 1, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 2, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 2, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 3, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 3, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 4, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 4, Directions.YChanging, 2, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 5, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 5, Directions.YChanging, 2, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 6, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 6, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 7, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 7, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 8, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 8, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 9, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 9, Directions.YChanging, 2, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 1, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 2, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 3, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 4, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 5, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 7, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 8, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 9, 10, Directions.YChanging, 2, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position1, _position1Heights, 10, 10, Directions.YChanging, 2, 1, 1));


            //_PH = AIHelpers.WhatCanIPlay(_position1, _position1Heights, 6, 6, Directions.XChanging, 1, 1, 1);
            //Assert.AreEqual(5, _PH.TheLength);
            //Assert.AreEqual(2, _PH.TheLocationX);
            //Assert.AreEqual(6, _PH.TheLocationY);
            //Assert.AreEqual("PARE*", _PH.ThePattern[0].Letters);
            //Assert.AreEqual(4, _PH.ThePattern[0].Score);
            //Assert.AreEqual("PARE*", _PH.ThePattern[1].Letters);
            //Assert.AreEqual(2, _PH.ThePattern.Count);
        }

        [TestMethod]
        public void AssertThat_WCIPHandlesComplexPositionForLengthOne()
        {

           char[,] _position2 = new char[12, 12]
            { //        1   2   3   4   5   6   7   8   9   10  
                { ' ', ' ',' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ', }, 
                { ' ', ' ',' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 1
                { ' ', ' ', ' ',' ',' ',' ','P',' ',' ',' ',' ',' ', }, // 2
                { ' ', ' ', ' ',' ',' ',' ','A','R','T',' ',' ',' ', }, // 3
                { ' ', ' ', ' ',' ','P',' ','R',' ',' ',' ',' ',' ', }, // 4
                { ' ', ' ', ' ','F','A','C','E','S',' ',' ',' ',' ', }, // 5
                { ' ', ' ', 'H','I',' ',' ',' ','O','N','E',' ',' ', }, // 6
                { ' ', ' ', ' ','N',' ',' ',' ','N',' ','H',' ',' ', }, // 7
                { ' ', ' ', 'H','E',' ',' ',' ',' ',' ',' ',' ',' ', }, // 8
                { ' ', ' ',' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ', }, // 9
                { ' ', ' ',' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ', }, //10
                { ' ', ' ',' ', ' ',' ',' ',' ',' ',' ',' ',' ',' ', }, 
};

            int[,] _position2Heights = new int[12, 12]
            { //     1  2  3  4  5  6  7  8  9 10    
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 1
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,  },  // 2
                { 0, 0, 0, 0, 0, 0, 4, 1, 1, 0, 0, 0,  },  // 3
                { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0,  },  // 4
                { 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0,  },  // 5
                { 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0,  },  // 6
                { 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0,  },  // 7
                { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 8
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 9
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  //10
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },
            };

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 1, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 1, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 2, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 2, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 3, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 3, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 4, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 4, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 5, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 5, Directions.XChanging, 1, 1, 1));

            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 6, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 6, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 7, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 7, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 8, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 8, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 9, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 9, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 10, Directions.XChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 10, Directions.XChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 1, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 1, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 2, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 2, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 3, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 3, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 4, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 4, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 5, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 5, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 6, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 6, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 7, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 7, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 8, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 8, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 9, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 9, Directions.YChanging, 1, 1, 1));

            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 1, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 2, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 3, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 4, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 5, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 6, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNotNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 7, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 8, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 9, 10, Directions.YChanging, 1, 1, 1));
            Assert.IsNull(AIHelpers.WhatCanIPlay(_position2, _position2Heights, 10, 10, Directions.YChanging, 1, 1, 1));

            //Assert.AreEqual(5, _PH.TheLength);
            //Assert.AreEqual(2, _PH.TheLocationX);
            //Assert.AreEqual(6, _PH.TheLocationY);
            //Assert.AreEqual("PARE*", _PH.ThePattern[0].Letters);
            //Assert.AreEqual(4, _PH.ThePattern[0].Score);
            //Assert.AreEqual("PARE*", _PH.ThePattern[1].Letters);
            //Assert.AreEqual(2, _PH.ThePattern.Count);
        }
    }
}

