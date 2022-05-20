using BlackJack.Manager;
using BlackJack.Model;
using BlackJack.View;
using UniRx;
using UnityEngine;

namespace BlackJack.Presenter
{
    public class InputPresenter : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private InputView _inputView;

        #endregion

        #region Unity Methods

        private void Start()
        {
            Subscribe();
            SetEvent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// IObservable�ŃC�x���g���w��
        /// </summary>
        private void Subscribe()
        {
            _inputView.ObservableGameStart.Subscribe(OnGameStart);
            _inputView.ObservableHitButton.Subscribe(_ => OnHitButton());
            _inputView.ObservableStayButton.Subscribe(_ => OnStayButton());

            SetSelectAction();
        }

        /// <summary>
        /// �f���Q�[�g�Ɋ֐���o�^
        /// </summary>
        private void SetEvent()
        {
            BoardModel.Instance.OnInitialize += OnInit;
        }

        private void OnInit()
        {
            _inputView.Init();

            SetSelectAction();
        }   

        /// <summary>
        /// �A�N�V�����̑I���{�^���̕\��,��\�����Ǘ�����C�x���g���w�ǂ���
        /// </summary>
        private void SetSelectAction()
        {
            // �����2��͏���̃h���[�Ȃ̂ŃX�L�b�v���� 
            // �X�L�b�v��̓{�^����\������
            BoardModel.Instance.ObservableSetSelectAction
                .Skip(4)
                .Subscribe(_inputView.SetActionButton);
        }

        private void OnGameStart(int betValue)
        {
            BoardModel.Instance.StartGame();

            BetModel.Instance.SetBetValue(betValue);

            CreditDataManager.Instance.UpdateCreditData
                (new Data.CreditData(CreditDataManager.Instance.Data.Credit - betValue));
        }

        private void OnHitButton()
        {
            BoardModel.Instance.DrawPlayerCard();
        }

        private void OnStayButton()
        {
            BoardModel.Instance.EndAction();
        }

        #endregion
    }
}