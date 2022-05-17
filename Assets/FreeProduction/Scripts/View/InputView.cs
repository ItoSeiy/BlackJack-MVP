using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

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
        private Subject<int> _onStartButton;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetUp();
        }

        #endregion

        #region Enums
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void SetUp()
        {
            _startButton.onClick.AddListener(OnStartButton);
            _betInput.onEndEdit.AddListener(x => SetBetValue(int.Parse(x)));
        }

        private void OnStartButton()
        {
            if (_betValue == 0) return;
            
            _onStartButton.OnNext(_betValue);
            _betValue = 0;
        }

        private void SetBetValue(int value)
        {
            _betValue = value;
        }

        #endregion
    }
}