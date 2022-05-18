using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.Calculation;
using UniRx;
using System;

namespace BlackJack.Model
{
    /// <summary>
    /// ä|ÇØã‡ÇÃä«óùÇÇ∑ÇÈModel
    /// </summary>
    public class BetModel : SingletonMonoBehaviour<BetModel>
    {
        #region Properties

        public IObservable<int> ObservableReturnBetValue => _onReturnBetValue;

        public IObservable<int> ObservableSetBetValue => _onSetBetValue;

        #endregion

        #region Member Variables

        private int _betValue = 0;

        #endregion

        #region Events

        Subject<int> _onReturnBetValue = new Subject<int>();

        Subject<int> _onSetBetValue = new Subject<int>();

        #endregion

        #region Public Methods

        public void SetBetValue(int betValue)
        {
            _betValue = betValue;
            _onSetBetValue.OnNext(betValue);
        }

        public void ReturnBetValue(BoardModel.ResultType winType)
        {
            switch(winType)
            {
                case BoardModel.ResultType.NormalWin:

                    _onReturnBetValue.OnNext(Calculator.NormalWin(_betValue));
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.BlackJack:

                    _onReturnBetValue.OnNext(Calculator.BlackJack(_betValue));
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.Draw:

                    _onReturnBetValue.OnNext(_betValue);
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.Lose:

                    _betValue = 0;

                    break;
            }
        }

        #endregion
    }
}