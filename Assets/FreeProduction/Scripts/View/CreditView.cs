using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

namespace BlackJack.View
{
    /// <summary>
    /// �N���W�b�g�S�ʂ�View
    /// </summary>
    public class CreditView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("�������z�̃e�L�X�g")]
        private Text _creditText;

        [SerializeField]
        [Header("�x�b�g���z�̃e�L�X�g")]
        private Text _betValueText;

        [SerializeField]
        [Header("�����z�̃e�L�X�g")]
        private Text _winBetText;


        [SerializeField]
        [Header("�����̃e�L�X�g���o�͂���Ƃ��̃A�j���[�V�����̎���")]
        private float _numTextAnimDuration = 0.5f;

        [SerializeField]
        [Header("����������������܂ł̎���(�~���b)")]
        private int _timeToDoInitText = 3000;

        #endregion

        #region Member Variables

        private int _lastCredit = 0;

        private int _lastBetValue = 0;

        private int _lastWinBet = 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// ��������UI���X�V����
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
        /// �|������UI���X�V����
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
        /// �����z��UI���X�V����
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
        /// ����������
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