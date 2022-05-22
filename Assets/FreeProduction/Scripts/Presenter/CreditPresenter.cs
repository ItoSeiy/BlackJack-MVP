using BlackJack.Data;
using BlackJack.Manager;
using BlackJack.Model;
using BlackJack.View;
using UniRx;
using UnityEngine;
using System.Threading.Tasks;

namespace BlackJack.Presenter
{
    /// <summary>
    /// �N���W�b�g�S�ʂ�Presenter
    /// </summary>
    public class CreditPresenter : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private CreditView _creditView;

        [SerializeField]
        [Header("�e�L�X�g������������܂ł̎���(�~���b)")]
        private int _timeToDoInitText = 1500;

        #endregion

        #region Member Variables

        /// <summary>�ۑ��p�̕ϐ�</summary>
        private int _initialTimeToDoInitText;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // �ۑ��p�̕ϐ��ɏ����l��ۑ����l��0�ɂ���
            // �������邱�Ƃŏ���̏������͒x���Ȃ������������
            _initialTimeToDoInitText = _timeToDoInitText;
            _timeToDoInitText = 0;
            Subscribe();
            Init();
            SetEvent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// IObservable�ŃC�x���g���w��
        /// </summary>
        private void Subscribe()
        {
            BetModel.Instance.ObservableReturnBetValue.Subscribe(x =>
            { 
                _creditView.SetWinBetText(x);

                CreditDataManager.Instance.UpdateCreditData
                    (new CreditData(CreditDataManager.Instance.Data.Credit + x));
            });

            BetModel.Instance.ObservableSetBetValue.Subscribe(x => _creditView.SetBetValue(x));

            CreditDataManager.Instance.ObservableCreditDataChange.Subscribe(x => _creditView.SetCreditText(x));
        }

        private void SetEvent()
        {
            BoardModel.Instance.OnInitialize += Init;
        }

        private async void Init()
        {
            _creditView.SetCreditText(CreditDataManager.Instance.Data.Credit);
            CreditDataManager.Instance.CreateCreditData();
            await Task.Delay(_timeToDoInitText);
            _creditView.Init();
            // ����̏�����������������̏����l�ɖ߂�
            _timeToDoInitText = _initialTimeToDoInitText;
        }

        #endregion
    }
}