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

        // ------------------------------------ゲーム進行

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
        /// ブラックジャックが開始しているかどうかのフラグ
        /// true -> 開始
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        // ------------------------------------------------プレイヤー

        /// <summary>
        /// 最新のプレイヤーのカード
        /// 監視可能, プレイヤーの手札の更新時にイベントを発行する
        /// </summary>
        public IObservable<CardData> ObservableLatestPlayerCard => _latestPlayerCard;

        /// <summary>
        /// プレイヤーの手札の数字 
        /// 監視可能, 手札の数字更新時にイベントを発行する
        ///</summary>
        public IObservable<int> ObservablePlayerCardNum => _playerHandNum;

        /// <summary>プレイヤーの手札の数字 </summary>
        public int PlayerCardNum => _playerHandNum.Value;

        // ----------------------------------------------ディーラー

        /// <summary>
        /// 最新のプレイヤーのカード
        /// 監視可能, ディーラーの手札の更新時にイベントを発行する
        /// </summary>
        public IObservable<CardData> ObservableLatestDealerCard => _latestDealerCard;

        public IObservable<int> ObservableDealerCardNum => _dealerHandNum;

        /// <summary>ディーラーの手札の数字 </summary>
        public int DealerCardNum => _dealerHandNum.Value;

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

        /// <summary>プレイヤーの手札のIndex</summary>
        private int _playerHandIndex = 0;

        /// <summary>プレイヤーの手札の数字</summary>
        private ReactiveProperty<int> _playerHandNum = new ReactiveProperty<int>();



        /// <summary>最新のディーラーのカード</summary>
        private ReactiveProperty<CardData> _latestDealerCard = new ReactiveProperty<CardData>();

        /// <summary>ディーラーの手札</summary>
        private List<CardData> _dealerHand = new List<CardData>();

        /// <summary>ディーラーの手札のIndex</summary>
        private int _dealerHandIndex = 0;

        /// <summary>ディーラーの手札の数字 数字</summary>
        private ReactiveProperty<int> _dealerHandNum = new ReactiveProperty<int>();

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
        public void DrawPlayerCard(bool checkBlackJack = true)
        {
            if (IsStarted == false) return;

            _setActiveSelectAction.OnNext(false);

            _playerHand.Add(CardStackModel.Instance.CurrentCard);
            _playerHandNum.Value += _playerHand[_playerHandIndex].Num;
            _latestPlayerCard.Value = _playerHand[_playerHandIndex];

            Debug.Log($"プレイヤーがカードを引いた 引いた数字は{_playerHand[_playerHandIndex].Num}" +
                $"\n現在の数字は{_playerHandNum}");

            if(CheckBust(_playerHandNum.Value) == true && CheckPlayerA11() == false)// A11が存在せずバーストしていたら
            {
                EndAction();
                return;
            }

            if (checkBlackJack == true && CheckBlackJack(_playerHandNum.Value) == true)
            {
                EndAction();
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
                    _dealerHandNum.Value += _dealerHand[_dealerHandIndex].Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHand.Add(CardStackModel.Instance.CurrentCard);
                    _dealerHoleHandNum = _dealerHand[_dealerHandIndex].Num;

                    break;
            }

            _latestDealerCard.Value = _dealerHand[_dealerHandIndex];

            Debug.Log($"ディーラーがカードを引いた 引いた数字は{_dealerHand[_dealerHandIndex].Num}" +
                $"\n現在のアップカードは{_dealerHandNum}ホールカードは{_dealerHoleHandNum}");

            if (CheckBust(_dealerHandNum.Value) == true)
            {
                CheckDealerA11();
            }

            _dealerHandIndex++;
        }

        /// <summary>ゲームスタート時のカードを引く処理</summary>
        IEnumerator OnStartDrawing()
        {
            DrawPlayerCard(false);

            yield return new WaitForSeconds(_drawDuration);

            DrawDealerCard(DealerCardType.Up);

            yield return new WaitForSeconds(_drawDuration);

            DrawPlayerCard(false);

            yield return new WaitForSeconds(_drawDuration);

            DrawDealerCard(DealerCardType.Hole);
            OnOpenUpCard?.Invoke();

            if (CheckBlackJack(_dealerHandNum.Value + _dealerHoleHandNum) == true
                || CheckBlackJack(_playerHandNum.Value) == true)
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
            while (_dealerHandNum.Value < DEALER_DRAWING_HAND_LIMIT)
            {
                yield return new WaitForSeconds(_drawDuration);
                DrawDealerCard(DealerCardType.Up);
            }

            // ディーラがハンドを引き終えたら最終的な勝敗の判別を行う
            if (_dealerHandNum.Value >= DEALER_DRAWING_HAND_LIMIT)
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
            if (CheckBust(_dealerHandNum.Value) == true
                && CheckBust(_playerHandNum.Value) == true)
            {
                print("ディーラーがバーストした しかしプレイヤーはすでにバーストしている" +
                    "\n プレイヤーの負け");

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                Init();
                _judgeResult.OnNext(ResultType.Lose);

                return;
            }
            else if (CheckBust(_playerHandNum.Value) == true)
            {
                print("ディーラはバーストしなかった ディーラーの勝ち");

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                Init();
                _judgeResult.OnNext(ResultType.Lose);

                return;
            }
            else if (CheckBust(_dealerHandNum.Value) == true)
            {
                print("ディーラーがバーストした プレイヤーの勝ち");

                BetModel.Instance.ReturnBetValue(ResultType.NormalWin);
                Init();
                _judgeResult.OnNext(ResultType.NormalWin);

                return;
            }

            // 両者バーストしていなかったら数字で勝敗を確定させる
            if (_playerHandNum.Value > _dealerHandNum.Value)
            {
                print($"プレイヤーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");

                BetModel.Instance.ReturnBetValue(ResultType.NormalWin);
                Init();
                _judgeResult.OnNext(ResultType.NormalWin);
            }
            else if (_playerHandNum.Value < _dealerHandNum.Value)
            {
                print($"ディーラーの勝ち\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");

                BetModel.Instance.ReturnBetValue(ResultType.Lose);
                Init();
                _judgeResult.OnNext(ResultType.Lose);
            }
            else
            {
                print($"引き分け\nプレイヤー{_playerHandNum} ディーラー{_dealerHandNum}");
                BetModel.Instance.ReturnBetValue(ResultType.Draw);
                Init();
                _judgeResult.OnNext(ResultType.Draw);
            }
        }

        private void JudgeBlackJack()
        {
            if (CheckBlackJack(_dealerHandNum.Value + _dealerHoleHandNum) == true
                && CheckBlackJack(_playerHandNum.Value) == true)
            {
                print("両者がブラックジャック 引き分け");

                OpenHoleCard();
                BetModel.Instance.ReturnBetValue(ResultType.Draw);

                Init();
                _judgeResult.OnNext(ResultType.Draw);
            }
            else if (CheckBlackJack(_dealerHandNum.Value + _dealerHoleHandNum) == true)
            {
                print("ディーラーがブラックジャック ディーラーの勝ち");

                OpenHoleCard();
                BetModel.Instance.ReturnBetValue(ResultType.Lose);

                Init();
                _judgeResult.OnNext(ResultType.Lose);
            }
            else if (CheckBlackJack(_playerHandNum.Value) == true)
            {
                print("プレイヤーがブラックジャック プレイヤーの勝ち");

                EndAction();

                BetModel.Instance.ReturnBetValue(ResultType.BlackJack);

                Init();
                _judgeResult.OnNext(ResultType.BlackJack);
            }
        }

        /// <summary>
        /// ディーラーが伏せているカード(ホールカード)を公開する
        /// </summary>
        private void OpenHoleCard()
        {
            _dealerHandNum.Value += _dealerHoleHandNum;
            _dealerHoleHandNum = 0;
            OnOpenHoleCard?.Invoke();
            print($"ディーラーがホールカードを公開した\n現在の数字は{_dealerHandNum}");

            if (CheckBust(_dealerHandNum.Value) == true)
            {
                CheckDealerA11();
            }
        }

        /// <summary>
        /// プレイヤーがバーストしていた際に呼び出される
        /// A11を探しA1に戻す関数
        /// </summary>
        /// <returns>true -> A11が存在した false -> A11が存在せずバーストした</returns>
        private bool CheckPlayerA11()
        {
            bool existsA11 = false;

            _playerHand = _playerHand.Select(x =>
            {
                // バーストした際にカードにACE(11)が含まれていたらACE(1)として返す
                // ※ACEはソフトハンドと呼ばれて11とも1とも認識できる
                if (x.Rank == CardData.RankType.A11 && existsA11 == false)
                {
                    existsA11 = true;
                    _playerHandNum.Value -= ACE_CARD_OFFSET;

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

                return true; // A11が存在しバーストしていない
            }
            else
            {
                // A11が存在せずバーストバースト
                return false;
            }
        }

        /// <summary>
        /// ディーラーがバーストしていた際に呼び出される
        /// A11を探しA1に戻す関数
        /// </summary>
        private void CheckDealerA11()
        {
            bool existsA11 = false;

            _dealerHand = _dealerHand.Select(x =>
            {
                // バーストした際にカードにACE(11)が含まれていたらACE(1)として返す
                // ※ACEはソフトハンドと呼ばれて11とも1とも認識できる
                if (x.Rank == CardData.RankType.A11 && existsA11 == false)
                {
                    existsA11 = true;
                    _dealerHandNum.Value -= ACE_CARD_OFFSET;

                    return x.ChangeRank(CardData.RankType.A1);
                }
                else
                {
                    return x;
                }
            }).ToList();

            if (existsA11 == true)
            {
                Debug.Log($"21を超えたがACE(11)が含まれていたためディーラーのハンドの数字が変更された" +
                    $"\n現在の数字は{_dealerHandNum}");
            }
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

            _setActiveSelectAction.OnCompleted();
            _setActiveSelectAction.Dispose();
            _setActiveSelectAction = new Subject<bool>();

            _judgeResult.OnCompleted();
            _judgeResult.Dispose();
            _judgeResult = new Subject<ResultType>();

            // プレイヤーに関する初期化
            _latestPlayerCard.Dispose();
            _latestPlayerCard = new ReactiveProperty<CardData>();

            _playerHandNum.Dispose();
            _playerHandNum = new ReactiveProperty<int>();

            _playerHand = new List<CardData>();
            _playerHandIndex = 0;


            // ディーラーに関する初期化
            _latestDealerCard.Dispose();
            _latestDealerCard = new ReactiveProperty<CardData>();

            _dealerHandNum.Dispose();
            _dealerHandNum = new ReactiveProperty<int>();

            _dealerHand = new List<CardData>();
            _dealerHandIndex = 0;
            _dealerHoleHandNum = 0;

            OnInitialize?.Invoke();
        }

        #endregion
    }
}
