using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.Calculation;
using UniRx;
using System;
using BlackJack.Data;
using BlackJack.Manager;

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

        #region Inspector Variables

        [SerializeField]
        private string _smallBetSoundKey = "SmallBet";

        [SerializeField]
        private string _normalBetSoundKey = "NormalBet";

        [SerializeField]
        private string _bigBetSoundKey = "BigBet";

        [SerializeField]
        private string _winSoundkey = "Win";

        [SerializeField]
        private string _blackJackSoundKey = "BlackJack";

        [SerializeField]
        private string _loseSoundKey = "Lose";


        [SerializeField]
        private RangeValue<int> _smallBetRange = new RangeValue<int>(0, 99);

        [SerializeField]
        private RangeValue<int> _normalBetRange = new RangeValue<int>(100, 999);

        [SerializeField]
        private RangeValue<int> _bigBetRange = new RangeValue<int>(1000, int.MaxValue);

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
            if(betValue <= _smallBetRange.End)
            {
                SoundManager.Instance.UseSFX(_smallBetSoundKey);
            }
            else if(betValue <= _normalBetRange.End)
            {
                SoundManager.Instance.UseSFX(_normalBetSoundKey);
            }
            else if(betValue <= _bigBetRange.End)
            {
                SoundManager.Instance.UseSFX(_bigBetSoundKey);
            }

            _betValue = betValue;
            _onSetBetValue.OnNext(betValue);
        }

        public void ReturnBetValue(BoardModel.ResultType winType)
        {
            switch(winType)
            {
                case BoardModel.ResultType.NormalWin:

                    SoundManager.Instance.UseSFX(_winSoundkey);
                    _onReturnBetValue.OnNext(Calculator.NormalWin(_betValue));
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.BlackJack:

                    SoundManager.Instance.UseSFX(_blackJackSoundKey);
                    _onReturnBetValue.OnNext(Calculator.BlackJack(_betValue));
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.Draw:

                    SoundManager.Instance.UseSFX(_winSoundkey);
                    _onReturnBetValue.OnNext(_betValue);
                    _betValue = 0;

                    break;

                case BoardModel.ResultType.Lose:

                    SoundManager.Instance.UseSFX(_loseSoundKey);
                    _betValue = 0;

                    break;
            }
        }

        #endregion
    }
}