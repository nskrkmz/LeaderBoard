using System;
using UnityEngine;

namespace Nesco.Quick.LeaderBoard.DBCore
{
    [CreateAssetMenu(fileName = "DBConfigurations", menuName = "Nesco/Quick/LeaderBoard/New DBConfigurations")]
    public class DBConfiguration : ScriptableObject
    {
        [SerializeField] private DBConfig _dbConfig;

        public DBConfig GetData()
        {
            try
            {
                ValidateData();
                SetDefaultDatas();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return _dbConfig;
        }

        private void ValidateData()
        {
            if (_dbConfig.RestURL == null) { throw new MissingReferenceException($"{this.name} => RestURL field is not set. Please assign a RestURL."); }
            if (_dbConfig.RestToken == null) { throw new MissingReferenceException($"{this.name} => RestToken field is not set. Please assign a RestToken."); }
        }

        private void SetDefaultDatas()
        {
            _dbConfig.DBName = this.name;
            _dbConfig.AuthorizationHeaderName = "Authorization";
            _dbConfig.AuthorizationHeaderValue = " Bearer " + _dbConfig.RestToken;

            if (_dbConfig.ScoreTableName == null || _dbConfig.ScoreTableName.Equals(""))
            {
                _dbConfig.ScoreTableName = "player:exp";
            }
            if (_dbConfig.InfoTableName == null || _dbConfig.InfoTableName.Equals(""))
            {
                _dbConfig.InfoTableName = "player:info";
            }
        }

        [System.Serializable]
        public struct DBConfig
        {
            [HideInInspector] public string DBName;
            [HideInInspector] public string AuthorizationHeaderName;
            [HideInInspector] public string AuthorizationHeaderValue;

            public string RestURL;
            public string RestToken;

            [Header("for example => player:exp, player:info")]
            public string ScoreTableName;
            public string InfoTableName;
        }
    }
}
