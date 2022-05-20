using BlackJack.Manager;
using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

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
        private int _timeToStartButtonSelectable = 3000;

        [SerializeField]
        [Header("�A�N�V�����̃{�^����I���ォ�牟����悤�ɂȂ�܂ł̎���(�~���b)")]
        private int _timeToSelectableActionButton = 1000;

        [SerializeField]
        [Header("�ʒm�̃e�L�X�g���o���b��")]
        private float _notificationDuration = 2;

        [SerializeField]
        private Text _notificationText;

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

        #region Public Methods

        /// <summary>�Q�[���̐i�s�����Z�b�g���ꂽ�ۂɌĂяo�����</summary>
        public async void Init()
        {
            SetActionButton(false);
            await Task.Delay(_timeToStartButtonSelectable);
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
            if (active == true)
            {
                _actionCanvasGroup.interactable = true;
                _actionCanvasGroup.blocksRaycasts = true;
                _actionCanvasGroup.DOFade(1, _fadeDuration);
            }
            else
            {
                _actionCanvasGroup.interactable = false;
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
                if (string.IsNullOrWhiteSpace(x) == false)
                {
                    SetBetValue(int.Parse(x));
                }
                else
                {
                    SetBetValue(0);
                }
            });

            _startButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(_timeToSelectableActionButton))
                .Subscribe(_ => OnStartButton());

            _hitButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(_timeToSelectableActionButton))
                .Subscribe(_ => OnHitButton());

            _stayButton
                .OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(_timeToSelectableActionButton))
                .Subscribe(_ => OnStayButton());
        }

        private void OnStartButton()
        {
            if (_betValue <= 0)
            {
                StartCoroutine
                    (OutputNotificationText("�x�b�g���z����͂��Ă�������"));
                return;
            }
            else if(CreditDataManager.Instance.Data.Credit < _betValue)
            {
                StartCoroutine
                    (OutputNotificationText("������������܂���"));
                return;
            }

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

        private IEnumerator OutputNotificationText(object message)
        {
            _notificationText.text = message.ToString();
            yield return new WaitForSeconds(_notificationDuration);
            _notificationText.text = "";
        }

        #endregion
    }
}