using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackJack.View;
using UniRx;
using BlackJack.Model;

namespace BlackJack.Presenter
{
    public class InputPresenter : MonoBehaviour
    {
        #region Properties
        #endregion

        #region Inspector Variables

        [SerializeField]
        private InputView _inputView;

        #endregion

        #region Member Variables
        #endregion

        #region Events
        #endregion

        #region Unity Methods

        private void Start()
        {
            Subscribe();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void Subscribe()
        {
            _inputView.ObservableGameStart.Subscribe(OnGameStart);
            _inputView.ObservableHitButton.Subscribe(_ => OnHitButton());
            _inputView.ObservableStayButton.Subscribe(_ => OnStayButton());

            BoardModel.Instance.SetSelectAction
                .Skip(4)
                .Subscribe(_inputView.SetActionButton);

            BoardModel.Instance.OnInitialize += OnInit;
        }

        private void OnInit()
        {
            _inputView.Init();

            // 初回の2回は初回のドローなのでスキップする 
            // スキップ後はボタンを表示する
            BoardModel.Instance.SetSelectAction
                .Skip(4)
                .Subscribe(_inputView.SetActionButton);
        }

        private void OnGameStart(int vetValue)
        {
            BoardModel.Instance.StartGame();
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