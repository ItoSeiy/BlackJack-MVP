using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using System.Threading.Tasks;
using DG.Tweening;

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

        /// <summary>
        /// Hit�̃C�x���g�𔭍s����
        /// </summary>
        public IObservable<string> ObservableHitButton => _onHitButton;

        /// <summary>
        /// Stay�̃C�x���g�𔭍s����
        /// </summary>
        public IObservable<string> ObservableStayButton => _onStayButton;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("�Q�[���I����Ƀ{�^����������悤�ɂȂ�܂ł̎���(�~���b)")]
        private int _timeToSelectable = 3000;

        [SerializeField]
        [Header("�x�b�e�B���O�̋��z�̓���")]
        private InputField _betInput;

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        [Header("Hit, Stay����{�^�����܂Ƃ߂�CanvasGroup")]
        private CanvasGroup _actionCanvasGroup;

        [SerializeField]
        [Header("�L�����o�X�O���[�v���t�F�[�h���鎞��")]
        private float _fadeDuration = 0.3f;

        [SerializeField]
        private Button _hitButton;

        [SerializeField]
        private Button _stayButton;

        #endregion

        #region Member Variables

        private int _betValue = 0;

        #endregion

        #region Events

        /// <summary>
        /// �X�^�[�g�{�^���������ꂽ�Ƃ��̏���
        /// int -> �x�b�e�B���O���z
        /// </summary>
        private Subject<int> _onStartButton = new Subject<int>();

        private Subject<string> _onHitButton = new Subject<string>();

        private Subject<string> _onStayButton = new Subject<string>();

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
            SetActionButton(false);
            await Task.Delay(_timeToSelectable);
            // ���������ꂽ����͉\�ɂ���
            _betInput.interactable = true;
            _startButton.interactable = true;
        }

        /// <summary>
        /// �{�^���̕\��,��\����؂肩����
        /// </summary>
        /// <param name="active">
        /// �{�^����\�����邩�ǂ���
        /// true -> �\��
        /// </param>
        public void SetActionButton(bool active)
        {
            if(active == true)
            {
                _actionCanvasGroup.interactable = true;
                _actionCanvasGroup.blocksRaycasts = true;
                _actionCanvasGroup.DOFade(1, _fadeDuration);
            }
            else
            {
                _actionCanvasGroup.interactable= false;
                _actionCanvasGroup.blocksRaycasts = false;
                _actionCanvasGroup.DOFade(0, _fadeDuration);
            }
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

            _hitButton.onClick.AddListener(OnHitButton);

            _stayButton.onClick.AddListener(OnStayButton);
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

        private void OnHitButton()
        {
            _onHitButton.OnNext("Hit");
        }

        private void OnStayButton()
        {
            _onStayButton.OnNext("Stay");
        }

        #endregion
    }
}