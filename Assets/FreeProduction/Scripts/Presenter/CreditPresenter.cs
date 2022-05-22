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
    /// クレジット全般のPresenter
    /// </summary>
    public class CreditPresenter : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private CreditView _creditView;

        [SerializeField]
        [Header("数字を初期化するまでの時間(ミリ秒)")]
        private int _timeToDoInitText = 1500;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Subscribe();
            Init();
            SetEvent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// IObservableでイベントを購読
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
        }

        #endregion
    }
}