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
    public class AIWorkerTest
    {
        AIWorker _worker;
        public string[,] _position1;
        public int[,] _position1Heights;

        [TestInitialize]
        public void Initialize()
        {
            _worker = new AIWorker();

            _position1 = new string[10, 10]
            { //   1   2   3   4   5   6   7   8   9   10  

                { " ", " "," "," "," "," "," "," "," "," ", }, // 1
                { " ", " "," "," "," ","T"," "," "," "," ", }, // 2
                { " ", " "," "," "," ","A","R","T"," "," ", }, // 3
                { " ", " "," ","M"," ","R"," "," "," "," ", }, // 4
                { " ", " ","F","A","C","E","S"," "," "," ", }, // 5
                { " ", "H","I"," "," "," ","O","N","E"," ", }, // 6
                { " ", " "," "," "," "," ","N"," ","H"," ", }, // 7
                { " ", " "," "," "," "," "," "," "," "," ", }, // 8
                { " ", " "," "," "," "," "," "," "," "," ", }, // 9
                { " ", " "," "," "," "," "," "," "," "," ", }, //10

};

            _position1Heights = new int[10, 10]
            { //  1  2  3  4  5  6  7  8  9 10    

                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 1
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,  },  // 2
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0,  },  // 3
                { 0, 0, 0, 1, 0, 1, 0, 0, 0, 0,  },  // 4
                { 0, 0, 4, 1, 1, 1, 1, 0, 0, 0,  },  // 5
                { 0, 1, 4, 0, 0, 0, 5, 1, 1, 0,  },  // 6
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0,  },  // 7
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 8
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  // 9
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  },  //10

            };
        }

        [TestMethod]
        public void AssertThat_AIWorkerTest()
        {
            string boardRack = "DENOPST";
            string result;

            result = _worker.FindMove(boardRack, _position1, _position1Heights);

            Assert.AreEqual("Word is DINE, score is 22", result);
        }



    }
}
