using BlackJack.Model;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackJack.View
{
    public class BoardResultView : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("���s, �����������o�͂���e�L�X�g")]
        private Text _resultText;

        [SerializeField]
        [Header("ResultText���o�͂���Ƃ��̃A�j���[�V�����̃J�[�u")]
        public AnimationCurve _doTextCurve;

        [SerializeField]
        [Header("ResultText���o�͂���Ƃ��̃A�j���[�V�����̎���")]
        private float _resultTextAnimDuration = 1f;

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

                    _resultText.text = "Win!";

                    break;

                case BoardModel.ResultType.Lose:

                    _resultText.text = "Lose";

                    break;

                case BoardModel.ResultType.Draw:

                    _resultText.text = "Draw";

                    break;

                case BoardModel.ResultType.BlackJack:

                    _resultText.DOText("Black Jack!!", _resultTextAnimDuration,
                        scrambleMode: ScrambleMode.Custom,
                        scrambleChars: "Black Jack!!")
                        .SetEase(_doTextCurve);

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

        public void Init()
        {
            _resultText.text = string.Empty;
            _playerHandNumText.text = string.Empty;
            _dealerHandNumText.text= string.Empty;
        }

        #endregion
    }
}