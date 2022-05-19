using BlackJack.Model;
using UnityEngine;
using UnityEngine.UI;

namespace BlackJack.View
{
    public class BoardResultView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("勝敗, 引き分けを出力するテキスト")]
        private Text _resultText;

        [SerializeField]
        private Text _playerHandNumText;

        [SerializeField]
        private Text _dealerHandNumText;

        #endregion

        #region Public Methods
        
        public void SetResultText(BoardModel.ResultType resultType)
        {
            switch (resultType)
            {
                case BoardModel.ResultType.NormalWin:

                    _resultText.text = "Win";

                    break;

                case BoardModel.ResultType.Lose:

                    _resultText.text = "Lose";

                    break;

                case BoardModel.ResultType.Draw:

                    _resultText.text = "Draw";

                    break;

                case BoardModel.ResultType.BlackJack:

                    _resultText.text = "BlackJack!!";

                    break;
            }
        }

        public void SetPlayerHandTextNum(int num)
        {
            _playerHandNumText.text = num.ToString();
        }

        public void SetDealerHandTextNum(int num)
        {
            _dealerHandNumText.text = num.ToString();
        }

        #endregion
    }
}