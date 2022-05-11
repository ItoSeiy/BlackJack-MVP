using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Manager
{
    /// <summary>
    /// プレイヤーやディーラの手札を管理するクラス
    /// </summary>
    public class BoardManager : SingletonMonoBehaviour<BoardManager>
    {
        private int _dealerCardNum;
        private int _playerCardNum;

        [ContextMenu("Player")]
        public void DrawPlayerCard()
        {
            _playerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"プレイヤーがカードを引いた\n現在の数字は{_playerCardNum}");

        }

        [ContextMenu("Dealer")]
        public void DrawDealerCard()
        {
            _dealerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"ディーラーがカードを引いた\n現在の数字は{_dealerCardNum}");
        }

        public enum Person
        {
            Player,
            Dealer
        }
    }
}