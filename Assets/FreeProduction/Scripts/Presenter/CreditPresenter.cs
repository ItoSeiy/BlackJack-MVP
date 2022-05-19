using BlackJack.Data;
using BlackJack.Manager;
using BlackJack.Model;
using BlackJack.View;
using UniRx;
using UnityEngine;

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

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Subscribe();
            SetUp();
            BoardModel.Instance.OnInitialize += SetUp;
        }

        #endregion
        
        #region Private Methods

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

        private void SetUp()
        {
            _creditView.SetCreditText(CreditDataManager.Instance.Data.Credit);
            _creditView.Init();
        }

        #endregion
    }
}