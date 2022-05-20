using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackJack.View
{
    /// <summary>
    /// クレジット全般のView
    /// </summary>
    public class CreditView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("ベット金額のテキスト")]
        private Text _betValueText;

        [SerializeField]
        [Header("勝ち額のテキスト")]
        private Text _winBetText;

        [SerializeField]
        [Header("所持金額のテキスト")]
        private Text _creditText;

        #endregion

        #region Public Methods

        /// <summary>
        /// 所持金のUIを更新する
        /// </summary>
        public CreditView SetCreditText(int value)
        {
            _creditText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// 勝ち額のUIを更新する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetWinBetText(int value)
        {
            _winBetText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// 掛け金のUIを更新する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetBetValue(int value)
        {
            _betValueText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Init()
        {
            _winBetText.text = string.Empty;
            _betValueText.text = string.Empty;
        }

        #endregion
    }
}