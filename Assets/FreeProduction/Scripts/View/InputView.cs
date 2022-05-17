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
        [Header("�Q�[���I����Ƀ{�^����������悤�ɂȂ�܂ł̎���(�~���b)")]
        private int _timeToSelectable = 3000;

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

        /// <summary>�Q�[���̐i�s�����Z�b�g���ꂽ�ۂɌĂяo�����</summary>
        public async void Init()
        {
            await Task.Delay(_timeToSelectable);
            // ���������ꂽ����͉\�ɂ���
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
            
            // �X�^�[�g��̓x�b�g���z����͕s�\�ɂ���
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