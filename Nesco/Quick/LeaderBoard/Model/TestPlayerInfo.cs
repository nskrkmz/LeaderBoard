using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Nesco.Quick.LeaderBoard.Model
{
    public class TestPlayerInfo
    {
        public string ID { get; set; }
        public string NickName { get; set; }
        public string Country { get; set; }
        public string LastGameDate { get; set; }

        public TestPlayerInfo()
        {
            //var rand = new Random();
            //ID = rand.Next(1000000, 9999999).ToString();
            //NickName = RandomString(7);
            //Country = RandomString(2);
            //LastGameDate = RandomDay().ToString();

        }


        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
