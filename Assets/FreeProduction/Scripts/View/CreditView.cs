using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackJack.View
{
    /// <summary>
    /// �N���W�b�g�S�ʂ�View
    /// </summary>
    public class CreditView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("�x�b�g���z�̃e�L�X�g")]
        private Text _betValueText;

        [SerializeField]
        [Header("�����z�̃e�L�X�g")]
        private Text _winBetText;

        [SerializeField]
        [Header("�������z�̃e�L�X�g")]
        private Text _creditText;

        #endregion

        #region Public Methods

        /// <summary>
        /// ��������UI���X�V����
        /// </summary>
        public CreditView SetCreditText(int value)
        {
            _creditText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// �����z��UI���X�V����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetWinBetText(int value)
        {
            _winBetText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// �|������UI���X�V����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreditView SetBetValue(int value)
        {
            _betValueText.text = value.ToString();
            return this;
        }

        /// <summary>
        /// ����������
        /// </summary>
        public void Init()
        {
            _winBetText.text = string.Empty;
            _betValueText.text = string.Empty;
        }

        #endregion
    }
}