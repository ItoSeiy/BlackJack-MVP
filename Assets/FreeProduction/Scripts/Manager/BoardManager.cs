using BlackJack.Data;
using System;
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
        #region Properties

        public int PlayerCardNum => _playerCardNum;

        public int DealerCardNum => _dealerCardNum;

        #endregion

        #region Inspector Variables

        [SerializeField]
        private float _drawDuration = 0.4f;

        #endregion

        #region Member Variables

        /// <summary>プレイヤーの手札</summary>
        private List<CardData> _playerHand = new List<CardData>();

        private int _playerHandIndex = 0;

        /// <summary>プレイヤーの数字</summary>
        private int _playerCardNum = 0;


        /// <summary>ディーラーの手札</summary>
        private List<CardData> _dealerHand = new List<CardData>();

        private int _dealerHandIndex = 0;

        /// <summary>ディーラーの数字</summary>
        private int _dealerCardNum = 0;

        /// <summary>ディーラーの伏せているカードの数字</summary>
        private int _dealerHoleCardNum = 0;

        #endregion

        #region Constant

        /// <summary>これ数字になるとバースト扱いになる数字</summary>
        private const int BUST_NUM = 22;

        /// <summary>ブラックジャック扱いになる数字</summary>
        private const int BLACKJACK_NUM = 21;

        /// <summary>ディーラーがトランプをこの数字以上になるまでに引き続ける</summary>
        private const int DEALER_DRAWING_HAND_LIMIT = 17;

        #endregion

        #region Events

        /// <summary>変数に変動があった際に呼ばれる</summary>
        public event Action OnVariablesChange;

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            CardManager.Instance.OnCreateEnd += StartGame;
        }

        #endregion

        #region Enums

        public enum Person { Player, Dealer }

        public enum DealerCardType
        {
            /// <summary>表向きのトランプ</summary>
            Up,
            /// <summary>伏せているトランプ</summary>
            Hole
        }

        #endregion

        #region Public Methods

        [ContextMenu("StartGame")]
        public void StartGame()
        {
            StartCoroutine(OnStartDrawing());
        }
        
        [ContextMenu("EndAction")]
        public void OnPlayerActionEnd()
        {
            StartCoroutine(OnEndDrawing());
        }

        [ContextMenu("Draw")]
        public void DrawPlayerCard()
        {
            _playerHand.Add(CardManager.Instance.CurrentCard);

            Debug.Log($"プレイヤーがカードを引いた\n現在の数字は{_playerCardNum}");

            if(CheckBust(_playerCardNum) == true)
            {
                print("プレイヤーがバーストした プレイヤーの負け");
                OnPlayerActionEnd();
            }
            _playerHandIndex++;
        }

        #endregion

        #region Privete Methods

        /// <summary>ゲームスタート時のカードを引く処理</summary>
        IEnumerator OnStartDrawing()
        {
            DrawPlayerCard();

            yield return new WaitForSeconds(_drawDuration);

            DrawDealerCard(DealerCardType.Up);

            yield return new WaitForSeconds(_drawDuration);

            DrawPlayerCard();

            yield return new WaitForSeconds(_drawDuration);


            DrawDealerCard(DealerCardType.Hole);

            if (CheckBlackJack(_dealerCardNum + _dealerHoleCardNum) == true
                && CheckBlackJack(_playerCardNum) == true)
            {
                print("両者がブラックジャック 引き分け");
                Init();
            }
            else if (CheckBlackJack(_dealerCardNum + _dealerHoleCardNum) == true)
            {
                print("ディーラーがブラックジャック ディーラーの勝ち");
                Init();
            }
            else if (CheckBlackJack(_playerCardNum) == true)
            {
                print("プレイヤーがブラックジャック プレイヤーの勝ち");
                Init();
            }
        }

        /// <summary>プレイヤーのアクションが終わった際のカードを引く処理</summary>
        IEnumerator OnEndDrawing()
        {
            while(_dealerCardNum >= DEALER_DRAWING_HAND_LIMIT)
            {
                DrawDealerCard(DealerCardType.Up);
                yield return new WaitForSeconds(_drawDuration);
            }
        }

        private void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:

                    _dealerCardNum += CardManager.Instance.CurrentCard.Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHoleCardNum = CardManager.Instance.CurrentCard.Num;

                    break;
            }

            Debug.Log($"ディーラーがカードを引いた\n現在のアップカードは{_dealerCardNum}" +
                $"ホールカードは{_dealerHoleCardNum}");

            if(CheckBust(_dealerCardNum) == true 
                && CheckBust(_playerCardNum) == true)
            {
                print("ディーラーがバーストした しかしプレイヤーはすでにバーストしている");
                Init();
            }
            else if(CheckBust(_dealerCardNum) == true)
            {
                print("ディーラーがバーストした プレイヤーの勝ち");
                Init();
            }
        }

        private void ShowHoleCard()
        {
            _dealerCardNum += _dealerHoleCardNum;
            print($"ディーラーがホールカードを公開した\n現在の数字は{_dealerCardNum}");
        }


        private bool CheckBlackJack(int num)
        {
            if(num == BLACKJACK_NUM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckBust(int num)
        {
            if(num >= BUST_NUM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void Init()
        {
            _playerCardNum = 0;
            _dealerCardNum = 0;
            _dealerHoleCardNum = 0;
        }

        #endregion

    }
}