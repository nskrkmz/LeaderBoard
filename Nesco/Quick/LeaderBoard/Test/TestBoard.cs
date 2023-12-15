using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nesco.Quick.LeaderBoard;
using Nesco.Quick.LeaderBoard.Model;
using Nesco.Quick.LeaderBoard.Test;

namespace Nesco.Quick.LeaderBoard.Test
{
    public class TestBoard : MonoBehaviour
    {
        [SerializeField] GameObject LeaderPlayerPrefab;
        [SerializeField] GameObject SecondPlayerPrefab;
        [SerializeField] GameObject ThirdPlayerPrefab;
        [SerializeField] GameObject NthPlayerPrefab;
        [SerializeField] float itemGap;

        //private static List<BoardItem> _boardMembers = new List<BoardItem>();
        private static List<TestPlayer> _boardMembers = new List<TestPlayer>();
        private static List<TestPlayerInfo> _boardMembersInfo = new List<TestPlayerInfo>();
        private List<GameObject> _memeberUiItems = new List<GameObject>();

        [SerializeField] int _playersCountPerPage;
        private static int playersCountPerPage;
        private bool _isBoardSeted;
        private static int _numberOfPage;

        private void Start()
        {
            playersCountPerPage = _playersCountPerPage;
            _numberOfPage = 1;
            GetPlayers(_numberOfPage);
        }

        private void Update()
        {
            //GetPlayerInfos();

            if (_boardMembers.Count == _playersCountPerPage && !_isBoardSeted && _boardMembersInfo.Count == _playersCountPerPage)
            {

                //_isBoardSeted = true;
                for (int i = 1; i <= _boardMembers.Count; i++)
                {
                    if (i == 1)
                    {
                        GameObject memberUiItem = Instantiate(LeaderPlayerPrefab, transform);
                        //memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1].Info.ID, _boardMembers[i - 1].Info.NickName, _boardMembers[i - 1].Score, _boardMembers[i - 1].Rank);
                        memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1]);
                        _memeberUiItems.Add(memberUiItem);
                    }
                    else if (i == 2)
                    {
                        GameObject memberUiItem = Instantiate(SecondPlayerPrefab, transform);
                        //memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1].Info.ID, _boardMembers[i - 1].Info.NickName, _boardMembers[i - 1].Score, _boardMembers[i - 1].Rank);
                        memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1]);
                        _memeberUiItems.Add(memberUiItem);
                    }
                    else if (i == 3)
                    {
                        GameObject memberUiItem = Instantiate(ThirdPlayerPrefab, transform);
                        //memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1].Info.ID, _boardMembers[i - 1].Info.NickName, _boardMembers[i - 1].Score, _boardMembers[i - 1].Rank);
                        memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1]);
                        _memeberUiItems.Add(memberUiItem);
                    }
                    else
                    {
                        var gap = (i - 4) * itemGap;
                        GameObject memberUiItem = Instantiate(NthPlayerPrefab, transform);

                        RectTransform rectTransform = memberUiItem.GetComponent<RectTransform>();
                        Vector2 currentPosition = rectTransform.anchoredPosition;
                        currentPosition.y += gap; // Y pozisyonunu artýrma
                        rectTransform.anchoredPosition = currentPosition;

                        //memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1].Info.ID, _boardMembers[i - 1].Info.NickName, _boardMembers[i - 1].Score, _boardMembers[i - 1].Rank);
                        memberUiItem.GetComponent<PlayerHolder>().SetDatas(_boardMembers[i - 1]);
                        _memeberUiItems.Add(memberUiItem);
                    }
                }
                foreach (var item in _boardMembers)
                {
                    Debug.Log($"playerID: {item.Info.ID},     {item.Rank} ,     {item.Info.NickName}");
                }
            }


        }

        private void GetPlayers(int numberOfPage)
        {
            int startingIndex = (numberOfPage - 1) * 7;
            int endingIndex = startingIndex + _playersCountPerPage - 1;
            LeaderBoard.instance.GetPlayers(startingIndex, endingIndex, GetPlayersCallback);


        }

        private void GetPlayerInfos()
        {
            foreach (var member in _boardMembers)
            {
                LeaderBoard.instance.GetPlayerInfo(member.Info.ID, GetPlayerInfoCallback);
            }
        }
        Action<string> GetPlayersCallback = (IDs) =>
        {
            // JSON dizesini Result sýnýfýna dönüþtür
            Result result = JsonUtility.FromJson<Result>(IDs);
            List<string> ids = new List<string>();
            ids = result.result;

            int rank = 1;
            for (int i = 0; i < ids.Count; i += 2)
            {
                //SetMember(ids[i], ids[i + 1], rank.ToString());
                SetMember(ids[i], ids[i + 1], (rank + (_numberOfPage - 1) * playersCountPerPage).ToString());
                rank++;
            }
        };

        Action<string> GetPlayerInfoCallback = (info) =>
        {
            // JSON dizesini TestPlayerInfo sýnýfýna dönüþtür
            TestPlayerInfo p_info = JsonUtility.FromJson<TestPlayerInfo>(info);
            Debug.Log(info);
            for (int i = 0; i < _boardMembers.Count; i++)
            {
                if (_boardMembers[i].Info.ID == p_info.ID)
                {
                    //_boardMembers[i].Info.NickName = p_info.NickName;
                    //_boardMembers[i].Info.Country = p_info.Country;
                    //_boardMembers[i].Info.LastGameDate = p_info.LastGameDate;
                    _boardMembers[i].Info = p_info;
                    _boardMembersInfo.Add(p_info);
                }
            }
        };

        public static void SetMember(string id, string score, string rank)
        {
            TestPlayer member = new TestPlayer();
            member.Info.ID = id;
            member.Score = Int32.Parse(score);
            member.Rank = Int32.Parse(rank);
            _boardMembers.Add(member);
        }

        private void ClearBoard()
        {
            for (int i = 0; i < _memeberUiItems.Count; i++)
            {
                Destroy(_memeberUiItems[i]);
            }
            _boardMembers.Clear();
            _memeberUiItems.Clear();
            _isBoardSeted = false;
        }

        public void NextPage()
        {
            _numberOfPage++;
            ClearBoard();
            GetPlayers(_numberOfPage);
        }
        public void PreviousPage()
        {
            if (_numberOfPage != 1)
            {
                _numberOfPage--;
                ClearBoard();
                GetPlayers(_numberOfPage);
            }
        }

        public void RefreshPage()
        {
            ClearBoard();
            GetPlayers(_numberOfPage);
        }
    }

    [Serializable]
    public class Result
    {
        public List<string> result;
    }
}
