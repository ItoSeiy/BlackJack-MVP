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

        #region Constant
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
            BoardModel.Instance.OnInitialize += () => _inputView.Init();
        }

        private void OnGameStart(int vetValue)
        {
            BoardModel.Instance.StartGame();
        }

        #endregion
    }
}