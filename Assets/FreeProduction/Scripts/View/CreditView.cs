using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

namespace BlackJack.View
{
    /// <summary>
    /// クレジット全般のView
    /// </summary>
    public class CreditView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("所持金額のテキスト")]
        private Text _creditText;

        [SerializeField]
        [Header("ベット金額のテキスト")]
        private Text _betValueText;

        [SerializeField]
        [Header("勝ち額のテキスト")]
        private Text _winBetText;


        [SerializeField]
        [Header("数字のテキストを出力するときのアニメーションの時間")]
        private float _numTextAnimDuration = 0.5f;

        [SerializeField]
        [Header("数字を初期化するまでの時間(ミリ秒)")]
        private int _timeToDoInitText = 3000;

        #endregion

        #region Member Variables

        private int _lastCredit = 0;

        private int _lastBetValue = 0;

        private int _lastWinBet = 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// 所持金のUIを更新する
        /// </summary>
        public CreditView SetCreditText(int value)
        {
            DOTween.To(() => _lastCredit,
                x => _lastCredit = x,
                value,
                _numTextAnimDuration)
                .OnUpdate(() => _creditText.text = _lastCredit.ToString())
                .OnComplete(() =>
                {
                    _creditText.text = value.ToString();
                    _lastCredit = value;
                });
            return this;
        }

        /// <summary>
        /// 掛け金のUIを更新する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetBetValue(int value)
        {
            DOTween.To(() => _lastBetValue,
                x => _lastBetValue = x,
                value,
                _numTextAnimDuration)
                .OnUpdate(() => _betValueText.text = _lastBetValue.ToString())
                .OnComplete(() =>
                {
                    _betValueText.text = value.ToString();
                    _lastBetValue= value;
                });
            return this;
        }

        /// <summary>
        /// 勝ち額のUIを更新する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetWinBetText(int value)
        {
            DOTween.To(() => _lastWinBet,
                x => _lastWinBet = x,
                value,
                _numTextAnimDuration)
                .OnUpdate(() => _winBetText.text = _lastWinBet.ToString())
                .OnComplete(() =>
                {
                    _winBetText.text = value.ToString();
                    _lastWinBet= value;
                });
            return this;
        }


        /// <summary>
        /// 初期化する
        /// </summary>
        public async Task Init()
        {
            await Task.Delay(_timeToDoInitText);
            _winBetText.text = string.Empty;
            _betValueText.text = string.Empty;
        }

        #endregion
    }
}