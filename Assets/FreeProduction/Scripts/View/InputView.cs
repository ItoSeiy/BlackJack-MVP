using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace BlackJack.View
{
    /// <summary>
    /// �Q�[���i�s�Ɋւ�����͂��󂯕t����View
    /// </summary>
    public class InputView : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// �Q�[���̊J�n�̃C�x���g�𔭍s����
        /// �Ď��\
        /// int -> �x�b�g���z
        /// </summary>
        public IObservable<int> ObservableGameStart => _onStartButton;

        #endregion

        #region Inspector Variables

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        [Header("�x�b�e�B���O�̋��z�̓���")]
        private InputField _betInput;

        #endregion

        #region Member Variables

        private int _betValue = 0;

        #endregion

        #region Constant
        #endregion

        #region Events

        /// <summary>
        /// �X�^�[�g�{�^���������ꂽ�Ƃ��̏���
        /// int -> �x�b�e�B���O���z
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