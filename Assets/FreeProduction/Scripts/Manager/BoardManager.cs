using BlackJack.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackJack.Manager
{
    /// <summary>
    /// プレイヤーやディーラの手札を管理するクラス
    /// </summary>
    public class BoardManager : SingletonMonoBehaviour<BoardManager>
    {
        #region Properties

        public int PlayerCardNum => _playerHandNum;

        public int DealerCardNum => _dealerHandNum;

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
        private int _playerHandNum = 0;


        /// <summary>ディーラーの手札</summary>
        private List<CardData> _dealerHand = new List<CardData>();

        private int _dealerHandIndex = 0;

        /// <summary>ディーラーの手札の数字</summary>
        private int _dealerHandNum = 0;

        /// <summary>ディーラーの伏せているカードの数字</summary>
        private int _dealerHoleHandNum = 0;

        #endregion

        #region Constant

        /// <summary>これ数字になるとバースト扱いになる数字</summary>
        private const int BUST_NUM = 22;

        /// <summary>ブラックジャック扱いになる数字</summary>
        private const int BLACKJACK_NUM = 21;

        /// <summary>ディーラーがトランプをこの数字以上になるまでに引き続ける</summary>
        private const int DEALER_DRAWING_HAND_LIMIT = 17;

        /// <summary>可変であるトランプ「ACE」のそれぞれの数字の差</summary>
        private const int ACE_CARD_OFFSET = 10;

        #endregion

        #region Events

        /// <summary>変数に変動があった際に呼ばれる</summary>
        public event Action OnVariablesChange;

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            //CardManager.Instance.OnCreateEnd += StartGame;
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
            Init();
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
            _playerHandNum += _playerHand[_playerHandIndex].Num;

            Debug.Log($"プレイヤーがカードを引いた\n現在の数字は{_playerHandNum}");

            if(CheckBust(_playerHandNum) == true)
            {
                //var tempHand = _playerHand.Where(x => x.Rank == CardData.RankType.A11)
                //    .Select(y => y.ChangeRank(CardData.RankType.A11));

                //if(tempHand.Any())
                //{
                //    _playerHandNum -= ACE_CARD_OFFSET;



                //    _playerHandIndex++;
                //    return;
                //}

                print("プレイヤーがバーストした プレイヤーの負け");
                OnPlayerActionEnd();
            }
            _playerHandIndex++;
        }

        #endregion

        #region Privete Methods

        private void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:

                    _dealerHand.Add(CardManager.Instance.CurrentCard);
                    _dealerHandNum += _dealerHand[_dealerHandIndex].Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHand.Add(CardManager.Instance.CurrentCard);
                    _dealerHoleHandNum = _dealerHand[_dealerHandIndex].Num;

                    break;
            }

            Debug.Log($"ディーラーがカードを引いた\n現在のアップカードは{_dealerHandNum}" +
                $"ホールカードは{_dealerHoleHandNum}");

            _dealerHandIndex++;
        }

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

            if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true
                && CheckBlackJack(_playerHandNum) == true)
            {
                print("両者がブラックジャック 引き分け");
                Init();
            }
            else if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true)
            {
                print("ディーラーがブラックジャック ディーラーの勝ち");
                Init();
            }
            else if (CheckBlackJack(_playerHandNum) == true)
            {
                print("プレイヤーがブラックジャック プレイヤーの勝ち");
                Init();
            }
        }

        /// <summary>プレイヤーのアクションが終わった際のカードを引く処理</summary>
        IEnumerator OnEndDrawing()
        {
            ShowHoleCard();

            // ディーラーはハンドが17以上になるまで引き続ける
            while(_dealerHandNum < DEALER_DRAWING_HAND_LIMIT)
            {
                DrawDealerCard(DealerCardType.Up);
                yield return new WaitForSeconds(_drawDuration);
            }

            // ディーラがハンドを引き終えたら
            if (_dealerHandNum >= DEALER_DRAWING_HAND_LIMIT)
            {
                // バーストの状況を確認して勝ち負けを確定させる
                if (CheckBust(_dealerHandNum) == true
                    && CheckBust(_playerHandNum) == true)
                {
                    print("ディーラーがバーストした しかしプレイヤーはすでにバーストしている");
                    Init();
                    yield break;
                }
                else if (CheckBust(_dealerHandNum) == true)
                {
                    print("ディーラーがバーストした プレイヤーの勝ち");
                    Init();
                    yield break;
                }
                else if(CheckBust(_playerHandNum) == true)
                {
                    print("ディーラはバーストしなかった ディーラーの勝ち");
                    Init();
                    yield break;
                }

                // 両者バーストしていなかったら数字で勝敗を確定させる
                if (_playerHandNum > _dealerHandNum)
                {
                    print($"プレイヤーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                    Init();
                }
                else if(_playerHandNum < _dealerHandNum)
                {
                    print($"ディーラーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                    Init();
                }
                else
                {
                    print("引き分け\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                    Init();
                }
            }
        }

        private void ShowHoleCard()
        {
            _dealerHandNum += _dealerHoleHandNum;
            _dealerHoleHandNum = 0;
            print($"ディーラーがホールカードを公開した\n現在の数字は{_dealerHandNum}");
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
            _playerHand = new List<CardData>();
            _playerHandNum = 0;
            _playerHandIndex = 0;

            _dealerHand = new List<CardData>();
            _dealerHandNum = 0;
            _dealerHandIndex = 0;
            _dealerHoleHandNum = 0;
        }

        #endregion
    }
}