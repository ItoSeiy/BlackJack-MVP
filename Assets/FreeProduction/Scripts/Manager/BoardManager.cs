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
        public int PlayerCardNum => _playerCardNum;

        public int DealerCardNum => _dealerCardNum;

        /// <summary>プレイヤーの数字</summary>
        private int _playerCardNum;

        /// <summary>ディーラーの数字</summary>
        private int _dealerCardNum;

        /// <summary>ディーラーの伏せているカードの数字</summary>
        private int _dealerHoleCardNum;


        public enum Person { Player, Dealer}

        public enum DealerCardType
        {
            /// <summary>表向きのトランプ</summary>
            Up,
            /// <summary>伏せているトランプ</summary>
            Hole
        }

        public void StartGame()
        {
            DrawPlayerCard();
            DrawDealerCard(DealerCardType.Up);

            DrawPlayerCard();
            DrawDealerCard(DealerCardType.Hole);
        }

        [ContextMenu("Player")]
        public void DrawPlayerCard()
        {
            _playerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"プレイヤーがカードを引いた\n現在の数字は{_playerCardNum}");
        }

        [ContextMenu("Dealer")]
        public void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:



                    break;

                case DealerCardType.Hole:

                    _dealerHoleCardNum = CardManager.Instance.CurrentCard.Num;

                    break;
            }

            _dealerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"ディーラーがカードを引いた\n現在の数字は{_dealerCardNum}");
        }
    }
}