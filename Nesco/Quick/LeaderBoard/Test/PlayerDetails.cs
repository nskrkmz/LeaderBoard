using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nesco.Quick.LeaderBoard.Model;
using TMPro;

namespace Nesco.Quick.LeaderBoard.Test
{
    public class PlayerDetails : MonoBehaviour
    {
        [SerializeField] TMP_Text _nickNameField;
        [SerializeField] TMP_Text _rankField;
        [SerializeField] TMP_Text _scoreField;
        [SerializeField] TMP_Text _countryField;
        [SerializeField] TMP_Text _dateField;

        public void SetDatas(TestPlayer player)
        {
            _nickNameField.text ="Nick Name: "+ player.Info.NickName;
            _rankField.text = "Rank: " + player.Rank.ToString();
            _scoreField.text = "Score: " + player.Score.ToString();
            _countryField.text = "Country: " + player.Info.Country;
            _dateField.text = "Lats Game Date: " + player.Info.LastGameDate;
        }

        public void ExitPanel()
        {
            Destroy(gameObject);
        }
    }
}

