using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nesco.Quick.LeaderBoard.Model;

namespace Nesco.Quick.LeaderBoard.Test
{
    public class DummyDatas : MonoBehaviour
    {
        public void SetDummyData(int testPlayerCount)
        {
            for (int i = 0; i < testPlayerCount; i++)
            {
                TestPlayer testPlayer = new TestPlayer();
                LeaderBoard.instance.SetNewPlayer(testPlayer.Info, testPlayer.Info.ID, testPlayer.Score);
            }
        }
    }
}
