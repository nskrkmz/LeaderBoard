using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Nesco.Quick.LeaderBoard.DBCore;

namespace Nesco.Quick.LeaderBoard.Editor
{
    public class CreateDatabaseManager : EditorWindow
    {
        [MenuItem("Nesco/Create Database Manager")]
        public static void ShowWindow()
        {
            GameObject dbManagerObj = new GameObject();
            dbManagerObj.name = "DatabaseManager";
            dbManagerObj.AddComponent<DBManager>();
        }
    }
}
