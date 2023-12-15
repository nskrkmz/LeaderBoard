using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nesco.Quick.LeaderBoard;
using Nesco.Quick.LeaderBoard.DBCore;

using static Nesco.Quick.LeaderBoard.DBCore.DBConfiguration;

namespace Nesco.Quick.LeaderBoard.DBCore
{
    [ExecuteAlways]
    [RequireComponent(typeof(LeaderBoard))]
    public class DBManager : MonoBehaviour
    {
        public static DBManager instance;

        [SerializeField] private DBConfiguration _testDB;
        [SerializeField] private DBConfiguration _mainDB;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                instance = this;
            }
        }

        public DBConfig GetDB()
        {
            #if UNITY_EDITOR
             return _testDB.GetData();
            #endif

            #if !UNITY_EDITOR
                return _mainDB.GetData();
            #endif
        }
    }
}
