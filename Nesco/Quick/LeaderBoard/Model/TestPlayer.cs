using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nesco.Quick.LeaderBoard.Model
{
    public class TestPlayer
    {
        public int Score { get; set; }
        public int Rank { get; set; }
        public TestPlayerInfo Info;
        public TestPlayer()
        {
            Info = new TestPlayerInfo();
            //var rand = new Random();
            //Score = rand.Next(100, 9999999);
        }
    }
}
