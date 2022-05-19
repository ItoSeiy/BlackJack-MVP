using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.View;
using BlackJack.Model;
using UniRx;
using BlackJack.Manager;
using BlackJack.Data;

namespace BlackJack.Presenter
{
    /// <summary>
    /// クレジット全般のPresenter
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
                    (new CreditData(CreditDataManager.Instance.CreditData.Credit + x));
            });

            BetModel.Instance.ObservableSetBetValue.Subscribe(x => _creditView.SetBetValue(x));

            CreditDataManager.Instance.ObservableCreditDataChange.Subscribe(x => _creditView.SetCreditText(x));
        }

        private void SetUp()
        {
            _creditView.SetCreditText(CreditDataManager.Instance.CreditData.Credit);
            _creditView.Init();
        }

        #endregion
    }
}