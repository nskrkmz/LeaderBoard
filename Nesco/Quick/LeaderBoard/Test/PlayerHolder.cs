using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Nesco.Quick.LeaderBoard.Model;
using UnityEngine.UI;

namespace Nesco.Quick.LeaderBoard.Test
{
    public class PlayerHolder : MonoBehaviour
    {
        [SerializeField] TMP_Text _nickNameField;
        [SerializeField] TMP_Text _scoreField;
        [SerializeField] TMP_Text _rankField;
        [SerializeField] GameObject _playerDetailsPrefab;

        private TestPlayer _player;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenPlayerDatails);
        }

        public void SetDatas(TestPlayer player)
        {
            _player = player;

            _nickNameField.text = _player.Info.NickName;
            _scoreField.text = _player.Score.ToString();
            _rankField.text = "#" + _player.Rank;
        }

        public void OpenPlayerDatails()
        {
            var detailsPnl = Instantiate(_playerDetailsPrefab, transform.parent);
            detailsPnl.GetComponent<PlayerDetails>().SetDatas(_player);
        }

    }
}
