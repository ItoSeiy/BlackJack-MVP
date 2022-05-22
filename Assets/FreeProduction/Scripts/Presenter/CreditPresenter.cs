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
        [Header("テキストを初期化するまでの時間(ミリ秒)")]
        private int _timeToDoInitText = 1500;

        #endregion

        #region Member Variables

        /// <summary>保存用の変数</summary>
        private int _initialTimeToDoInitText;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // 保存用の変数に初期値を保存し値を0にする
            // そうすることで初回の初期化は遅延なく初期化される
            _initialTimeToDoInitText = _timeToDoInitText;
            _timeToDoInitText = 0;
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
            // 初回の初期化が完了したらの初期値に戻す
            _timeToDoInitText = _initialTimeToDoInitText;
        }

        #endregion
    }
}