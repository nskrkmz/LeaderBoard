using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Nesco.Quick.LeaderBoard.Model;
using Nesco.Quick.LeaderBoard.DBCore;
using Nesco.Quick.LeaderBoard.Test;

using static Nesco.Quick.LeaderBoard.DBCore.DBConfiguration;

namespace Nesco.Quick.LeaderBoard
{
    public class LeaderBoard : MonoBehaviour
    {
        public static LeaderBoard instance;
        private DBConfig _dBConfig;
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
            _dBConfig = DBManager.instance.GetDB();

            if (gameObject.GetComponent<DummyDatas>())
            {
                gameObject.GetComponent<DummyDatas>().SetDummyData(38);
            }
            
        }

        public void SetNewPlayer<T>(T currentPlayer, string playerId, int startingScore) => StartCoroutine(SetNewPlayerCo(currentPlayer, playerId, startingScore));
        public void UpdatePlayerScore(string playerId, int scoreIncrement) => StartCoroutine(UpdatePlayerScoreCo(playerId, scoreIncrement));
        public void GetTopPlayers(int playerCount, Action<string> callback) => StartCoroutine(GetTopPlayersCo(playerCount, callback));
        public void GetPlayers(int startingIndex, int endingIndex, Action<string> callback) => StartCoroutine(GetPlayersCo(startingIndex, endingIndex,callback));
        public void GetPlayerRank(string playerId, Action<int> callback) => StartCoroutine(GetPlayerRankCo(playerId, callback));
        public void GetPlayerRevRank(string playerId, Action<int> callback) => StartCoroutine(GetPlayerRevRankCo(playerId, callback));
        public void GetPlayerScore(string playerId, Action<int> callback) => StartCoroutine(GetPlayerScoreCo(playerId, callback));
        public void GetPlayerInfo(string playerId, Action<string> callback) => StartCoroutine(GetPlayerInfoCo(playerId, callback));
        public void ClearAll() => StartCoroutine(ClearAllCo());
        

        IEnumerator SetNewPlayerCo<T>(T currentPlayer, string playerID, int startingScore)
        {
            var serializeObj = JsonConvert.SerializeObject(currentPlayer);

            using (UnityWebRequest scoreSetRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zadd/{_dBConfig.ScoreTableName}/{startingScore}/{playerID}"))
            {
                scoreSetRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return scoreSetRequest.SendWebRequest();

                if (scoreSetRequest.result != UnityWebRequest.Result.Success)
                {
                    LogError(" scoreSetRequest => "+scoreSetRequest.error);
                }
            }
            using (UnityWebRequest playerSetRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/hset/{_dBConfig.InfoTableName}/{playerID}/{serializeObj}"))
            {
                playerSetRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return playerSetRequest.SendWebRequest();

                if (playerSetRequest.result != UnityWebRequest.Result.Success)
                {
                    LogError(" playerSetRequest => " + playerSetRequest.error);
                }
            }
        }

        IEnumerator UpdatePlayerScoreCo(string currentPlayerID, int scoreIncrement)
        {
            using (UnityWebRequest updatePlayerReq = UnityWebRequest.Get(_dBConfig.RestURL + $"/zincrby/{_dBConfig.ScoreTableName}/{scoreIncrement}/{currentPlayerID}"))
            {
                updatePlayerReq.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return updatePlayerReq.SendWebRequest();

                if (updatePlayerReq.result != UnityWebRequest.Result.Success)
                {
                    LogError(" updatePlayerReq => " + updatePlayerReq.error);
                }
            }
        }
        IEnumerator GetTopPlayersCo(int playerCount, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zrevrange/{_dBConfig.ScoreTableName}/0/{playerCount - 1}/withscores"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string ids = webRequest.downloadHandler.text;
                    callback(ids);
                }
                else
                {
                    Debug.LogError("Get Top Players request failed: " + webRequest.error);
                    Debug.LogError("Get Top Players request failed: " + webRequest.error);
                }
            }
        }
        IEnumerator GetPlayersCo(int startingIndex, int endingIndex, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zrevrange/{_dBConfig.ScoreTableName}/{startingIndex}/{endingIndex}/withscores"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string ids = webRequest.downloadHandler.text;
                    callback(ids);
                }
                else
                {
                    Debug.LogError("Get Top Players request failed: " + webRequest.error);
                }
            }
        }
        IEnumerator GetPlayerInfoCo(string playerID, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/hget/{_dBConfig.InfoTableName}/{playerID}"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string info = webRequest.downloadHandler.text;
                    Debug.Log(info);
                    callback(info);
                    
                }
                else
                {
                    Debug.LogError("Get Top Players request failed: " + webRequest.error);
                }
            }
        }
        IEnumerator GetPlayerRankCo(string playerID, Action<int> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zrank/{_dBConfig.ScoreTableName}/{playerID}"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Baþarýlý GET isteði: " + webRequest.downloadHandler.text);
                    int revRank = int.Parse(webRequest.downloadHandler.text);
                    callback(revRank);
                    // Ýstek baþarýlýysa, yanýtý webRequest.downloadHandler.text ile alabilirsiniz.
                }
                else
                {
                    Debug.LogError("GET isteði baþarýsýz: " + webRequest.error);
                    // Ýstek baþarýsýzsa, hata mesajýný webRequest.error ile alabilirsiniz.
                }
            }
        }

        //Verilere Gore Asil index sirasi doner (+1 yaparak kacinci oldugu ogrenilebilir)
        IEnumerator GetPlayerRevRankCo(string playerID, Action<int> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zrevrank/{_dBConfig.ScoreTableName}/{playerID}"))
            {
                //string authorizationHeader = " Bearer " + _dBConfig.RestToken;
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Baþarýlý GET isteði: " + webRequest.downloadHandler.text);
                    int rank = int.Parse(webRequest.downloadHandler.text);
                    callback(rank);
                    // Ýstek baþarýlýysa, yanýtý webRequest.downloadHandler.text ile alabilirsiniz.
                }
                else
                {
                    Debug.LogError("GET isteði baþarýsýz: " + webRequest.error);
                    // Ýstek baþarýsýzsa, hata mesajýný webRequest.error ile alabilirsiniz.
                }
            }
        }

        IEnumerator GetPlayerScoreCo(string playerID, Action<int> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/zscore/{_dBConfig.ScoreTableName}/{playerID}"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Baþarýlý GET isteði: " + webRequest.downloadHandler.text);
                    int score = int.Parse(webRequest.downloadHandler.text);
                    callback(score);
                    // Ýstek baþarýlýysa, yanýtý webRequest.downloadHandler.text ile alabilirsiniz.
                }
                else
                {
                    Debug.LogError("GET isteði baþarýsýz: " + webRequest.error);
                    // Ýstek baþarýsýzsa, hata mesajýný webRequest.error ile alabilirsiniz.
                }
            }
        }

        IEnumerator ClearAllCo()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(_dBConfig.RestURL + $"/flushall"))
            {
                webRequest.SetRequestHeader(_dBConfig.AuthorizationHeaderName, _dBConfig.AuthorizationHeaderValue);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Baþarýlý GET isteði: " + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("GET isteði baþarýsýz: " + webRequest.error);
                }
            }
        }

        void LogError(string msg)
        {
            Debug.Log("request Failed :" + msg);
        }
       
    }
}
