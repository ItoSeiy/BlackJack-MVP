using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.View;
using UniRx;
using BlackJack.Model;
using BlackJack.Manager;

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
        }

        #endregion

        #region Private Methods

        private void Subscribe()
        {
            _inputView.ObservableGameStart.Subscribe(OnGameStart);
            _inputView.ObservableHitButton.Subscribe(_ => OnHitButton());
            _inputView.ObservableStayButton.Subscribe(_ => OnStayButton());

            BoardModel.Instance.OnInitialize += OnInit;

            SetSelectAction();
        }

        private void OnInit()
        {
            _inputView.Init();

            SetSelectAction();
        }   

        /// <summary>
        /// アクションの選択ボタンの表示,非表示を管理するイベントを購読する
        /// </summary>
        private void SetSelectAction()
        {
            // 初回の2回は初回のドローなのでスキップする 
            // スキップ後はボタンを表示する
            BoardModel.Instance.SetSelectAction
                .Skip(4)
                .Subscribe(_inputView.SetActionButton);
        }

        private void OnGameStart(int betValue)
        {
            BoardModel.Instance.StartGame();

            BetModel.Instance.SetBetValue(betValue);

            CreditDataManager.Instance.UpdateCreditData
                (new Data.CreditData(CreditDataManager.Instance.CreditData.Credit - betValue));
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