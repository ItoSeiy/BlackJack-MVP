using BlackJack.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

namespace BlackJack.Model
{
    /// <summary>
    /// プレイヤーやディーラの手札を管理するクラス(モデル)
    /// </summary>
    public class BoardModel : SingletonMonoBehaviour<BoardModel>
    {
        #region Properties

        /// <summary>
        /// 勝敗を判定する際にイベントを発行する
        /// 監視可能
        /// </summary>
        public IObservable<ResultType> ObservableJudgeResult => _judgeResult;

        /// <summary>
        /// アクションの選択ボタンの表示,非表示を管理するイベントを発行する
        /// 監視可能
        /// </summary>
        public IObservable<bool> ObservableSetSelectAction => _setActiveSelectAction;

        /// <summary>
        /// 最新のプレイヤーのカード
        /// 監視可能, プレイヤーの手札の更新時にイベントを発生する
        /// </summary>
        public IObservable<CardData> ObservableLatestPlayerCard => _latestPlayerCard;

        public int PlayerCardNum => _playerHandNum;

        /// <summary>
        /// 最新のプレイヤーのカード
        /// 監視可能, ディーラーの手札の更新時にイベントを発生する
        /// </summary>
        public IObservable<CardData> ObservableLatestDealerCard => _latestDealerCard;

        public int DealerCardNum => _dealerHandNum;

        /// <summary>
        /// ブラックジャックが開始しているかどうかのフラグ
        /// true -> 開始
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("カードを引く速度の間隔")]
        private float _drawDuration = 1f;

        #endregion

        #region Member Variables

        /// <summary>最新のプレイヤーのカード</summary>
        private ReactiveProperty<CardData> _latestPlayerCard = new ReactiveProperty<CardData>();

        /// <summary>プレイヤーの手札</summary>
        private List<CardData> _playerHand = new List<CardData>();

        private int _playerHandIndex = 0;

        /// <summary>プレイヤーの数字</summary>
        private int _playerHandNum = 0;

        /// <summary>最新のディーラーのカード</summary>
        private ReactiveProperty<CardData> _latestDealerCard = new ReactiveProperty<CardData>();

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

        /// <summary>
        /// 勝敗を判定する際にイベントを発行する
        /// </summary>
        private Subject<ResultType> _judgeResult = new Subject<ResultType>();

        /// <summary>
        /// アクションの選択ボタンの表示,非表示を管理するイベントを発行する
        /// </summary>
        private Subject<bool> _setActiveSelectAction = new Subject<bool>();

        /// <summary>ボードの初期化の際に実行</summary>
        public event Action OnInitialize;

        /// <summary>アップカードがめくられる際に呼び出される</summary>
        public event Action OnOpenUpCard;

        /// <summary>ホールカードがめくられる際に呼び出される</summary>
        public event Action OnOpenHoleCard;

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

        /// <summary>勝利方法</summary>
        public enum ResultType
        {
            /// <summary>通常勝利</summary>
            NormalWin,
            /// <summary>ブラックジャックによる勝利</summary>
            BlackJack,
            /// <summary>引き分け</summary>
            Draw,
            /// <summary>負け</summary>
            Lose,
        }


        #endregion

        #region Public Methods

        [ContextMenu("StartGame")]
        public void StartGame()
        {
            if (IsStarted == true) return;

            IsStarted = true;
            StartCoroutine(OnStartDrawing());
        }

        [ContextMenu("EndAction")]
        public void EndAction()
        {
            if (IsStarted == false) return;

            StartCoroutine(OnEndDrawing());
        }

        [ContextMenu("Draw")]
        public void DrawPlayerCard()
        {
            if (IsStarted == false) return;

            _setActiveSelectAction.OnNext(false);

            _playerHand.Add(CardStackModel.Instance.CurrentCard);
            _playerHandNum += _playerHand[_playerHandIndex].Num;
            _latestPlayerCard.Value = _playerHand[_playerHandIndex];

            Debug.Log($"プレイヤーがカードを引いた 引いた数字は{_playerHand[_playerHandIndex].Num}" +
                $"\n現在の数字は{_playerHandNum}");

            if (CheckBlackJack(_playerHandNum) == true)
            {
                EndAction();
                return;
            }

            if (CheckBust(_playerHandNum) == true)
            {
                bool existsA11 = false;

                _playerHand = _playerHand.Select(x =>
                {
                    // バーストした際にカードにACE(11)が含まれていたらACE(1)として返す
                    // ※ACEはソフトハンドと呼ばれて11とも1とも認識できる
                    if (x.Rank == CardData.RankType.A11 && existsA11 == false)
                    {
                        existsA11 = true;
                        _playerHandNum -= ACE_CARD_OFFSET;

                        return x.ChangeRank(CardData.RankType.A1);
                    }
                    else
                    {
                        return x;
                    }
                }).ToList();

                if (existsA11 == true)
                {
                    Debug.Log($"21を超えたがACE(11)が含まれていたためプレイヤーのハンドの数字が変更された" +
                        $"\n現在の数字は{_playerHandNum}");

                    // 変更した結果を適用 イベント発行
                    _latestPlayerCard.Value = _playerHand[_playerHandIndex];

                    if (CheckBlackJack(_playerHandNum) == true)
                    {
                        EndAction();
                        return;
                    }
                    else
                    {
                        _setActiveSelectAction.OnNext(true);
                        _playerHandIndex++;
                        return; //バーストはしていない
                    }
                }

                // ここまで来たらバースト
                EndAction();
                _playerHandIndex++;
                return;
            }

            _setActiveSelectAction.OnNext(true);

            _playerHandIndex++;
        }

        #endregion

        #region Privete Methods

        private void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:

                    _dealerHand.Add(CardStackModel.Instance.CurrentCard);
                    _dealerHandNum += _dealerHand[_dealerHandIndex].Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHand.Add(CardStackModel.Instance.CurrentCard);
                    _dealerHoleHandNum = _dealerHand[_dealerHandIndex].Num;

                    break;
            }

            _latestDealerCard.Value = _dealerHand[_dealerHandIndex];

            Debug.Log($"ディーラーがカードを引いた 引いた数字は{_dealerHand[_dealerHandIndex].Num}" +
                $"\n現在のアップカードは{_dealerHandNum}ホールカードは{_dealerHoleHandNum}");

            if(CheckBust(_dealerHandNum) == true)
            {
                bool existsA11 = false;

                _dealerHand = _dealerHand.Select(x =>
                {
                    // バーストした際にカードにACE(11)が含まれていたらACE(1)として返す
                    // ※ACEはソフトハンドと呼ばれて11とも1とも認識できる
                    if(x.Rank == CardData.RankType.A11 && existsA11 == false)
                    {
                        existsA11 = true;
                        _dealerHandNum -= ACE_CARD_OFFSET;

                        return x.ChangeRank(CardData.RankType.A1);
                    }
                    else
                    {
                        return x;
                    }
                }).ToList();

                if(existsA11 == true)
                {
                    Debug.Log($"21を超えたがACE(11)が含まれていたためディーラーのハンドの数字が変更された" +
                        $"\n現在の数字は{_dealerHandNum}");

                    // 変更した結果を適用 イベント発行
                    _latestDealerCard.Value = _dealerHand[_dealerHandIndex];
                }
            }

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
            OnOpenUpCard?.Invoke();

            if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true
                || CheckBlackJack(_playerHandNum) == true)
            {
                JudgeBlackJack();
                yield break;
            }

            _setActiveSelectAction.OnNext(true);
        }

        /// <summary>プレイヤーのアクションが終わった際のカードを引く処理</summary>
        IEnumerator OnEndDrawing()
        {
            _setActiveSelectAction.OnNext(false);
            yield return new WaitForSeconds(_drawDuration);

            OpenHoleCard();

            // ディーラーはハンドが17以上になるまで引き続ける
            while (_dealerHandNum < DEALER_DRAWING_HAND_LIMIT)
            {
                yield return new WaitForSeconds(_drawDuration);
                DrawDealerCard(DealerCardType.Up);
            }

            // ディーラがハンドを引き終えたら最終的な勝敗の判別を行う
            if (_dealerHandNum >= DEALER_DRAWING_HAND_LIMIT)
            {
                yield return new WaitForSeconds(_drawDuration);
                Judge();
            }
        }

        /// <summary>
        /// 最終的な勝敗の判別を行う
        /// </summary>
        private void Judge()
        {
            // バーストの状況を確認して勝ち負けを確定させる
            if (CheckBust(_dealerHandNum) == true
                && CheckBust(_playerHandNum) == true)
            {
                print("ディーラーがバーストした しかしプレイヤーはすでにバーストしている" +
                    "\n プレイヤーの負け");

                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                _judgeResult.OnNext(ResultType.Lose);

                return;
            }
            else if (CheckBust(_playerHandNum) == true)
            {
                print("ディーラはバーストしなかった ディーラーの勝ち");

                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                _judgeResult.OnNext(ResultType.Lose);

                return;
            }
            else if (CheckBust(_dealerHandNum) == true)
            {
                print("ディーラーがバーストした プレイヤーの勝ち");

                Init();

                BetModel.Instance.ReturnBetValue(ResultType.NormalWin);
                _judgeResult.OnNext(ResultType.NormalWin);

                return;
            }

            // 両者バーストしていなかったら数字で勝敗を確定させる
            if (_playerHandNum > _dealerHandNum)
            {
                print($"プレイヤーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");

                Init();

                BetModel.Instance.ReturnBetValue(ResultType.NormalWin);
                _judgeResult.OnNext(ResultType.NormalWin);
            }
            else if (_playerHandNum < _dealerHandNum)
            {
                print($"ディーラーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                _judgeResult.OnNext(ResultType.Lose);
            }
            else
            {
                print($"引き分け\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Draw);
                _judgeResult.OnNext(ResultType.Draw);
            }
        }

        private void JudgeBlackJack()
        {
            if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true
                && CheckBlackJack(_playerHandNum) == true)
            {
                print("両者がブラックジャック 引き分け");

                OpenHoleCard();
                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Draw);
                _judgeResult.OnNext(ResultType.Draw);
            }
            else if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true)
            {
                print("ディーラーがブラックジャック ディーラーの勝ち");

                OpenHoleCard();
                Init();

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                _judgeResult.OnNext(ResultType.Lose);
            }
            else if (CheckBlackJack(_playerHandNum) == true)
            {
                print("プレイヤーがブラックジャック プレイヤーの勝ち");

                OpenHoleCard();
                Init();

                BetModel.Instance.ReturnBetValue(ResultType.BlackJack);
                _judgeResult.OnNext(ResultType.BlackJack);
            }
        }

        /// <summary>
        /// ディーラーが伏せているカード(ホールカード)を公開する
        /// </summary>
        private void OpenHoleCard()
        {
            _dealerHandNum += _dealerHoleHandNum;
            _dealerHoleHandNum = 0;
            OnOpenHoleCard?.Invoke();
            print($"ディーラーがホールカードを公開した\n現在の数字は{_dealerHandNum}");
        }

        private bool CheckBlackJack(int num)
        {
            if (num == BLACKJACK_NUM)
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
            if (num >= BUST_NUM)
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
            // ゲーム進行に関する初期化
            IsStarted = false;

            _setActiveSelectAction.Dispose();
            _setActiveSelectAction = new Subject<bool>();

            // プレイヤーに関する初期化
            _latestPlayerCard.Dispose();
            _latestPlayerCard = new ReactiveProperty<CardData>();

            _playerHand = new List<CardData>();
            _playerHandNum = 0;
            _playerHandIndex = 0;


            // ディーラーに関する初期化
            _latestDealerCard.Dispose();
            _latestDealerCard = new ReactiveProperty<CardData>();

            _dealerHand = new List<CardData>();
            _dealerHandNum = 0;
            _dealerHandIndex = 0;
            _dealerHoleHandNum = 0;

            OnInitialize?.Invoke();
        }

        #endregion
    }
}
