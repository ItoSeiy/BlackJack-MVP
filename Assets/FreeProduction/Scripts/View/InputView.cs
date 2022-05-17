using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using System.Threading.Tasks;

namespace BlackJack.View
{
    /// <summary>
    /// ゲーム進行に関する入力を受け付けるView
    /// </summary>
    public class InputView : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// ゲームの開始のイベントを発行する
        /// 監視可能
        /// int -> ベット金額
        /// </summary>
        public IObservable<int> ObservableGameStart => _onStartButton;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("ゲーム終了後にボタンを押せるようになるまでの時間(ミリ秒)")]
        private int _timeToSelectable = 3000;

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        [Header("ベッティングの金額の入力")]
        private InputField _betInput;

        #endregion

        #region Member Variables

        private int _betValue = 0;

        #endregion

        #region Constant
        #endregion

        #region Events

        /// <summary>
        /// スタートボタンが押されたときの処理
        /// int -> ベッティング金額
        /// </summary>
        private Subject<int> _onStartButton = new Subject<int>();

        #endregion

        #region Unity Methods

        private void Start()
        {
            SetUpInputEvent();
        }

        #endregion

        #region Enums
        #endregion

        #region Public Methods

        /// <summary>ゲームの進行がリセットされた際に呼び出される</summary>
        public async void Init()
        {
            await Task.Delay(_timeToSelectable);
            // 初期化されたら入力可能にする
            _betInput.interactable = true;
            _startButton.interactable = true;
        }

        #endregion

        #region Private Methods

        private void SetUpInputEvent()
        {
            _betInput.onEndEdit.AddListener(x =>
            {
                if(string.IsNullOrWhiteSpace(x) == false)
                {
                    SetBetValue(int.Parse(x));
                }
            });

            _startButton.onClick.AddListener(OnStartButton);
        }

        private void OnStartButton()
        {
            if (_betValue == 0) return;
            
            // スタート後はベット金額を入力不可能にする
            _betInput.interactable = false;
            _startButton.interactable = false;

            _onStartButton.OnNext(_betValue);
        }

        private void SetBetValue(int value)
        {
            _betValue = value;
        }

        #endregion
    }
}