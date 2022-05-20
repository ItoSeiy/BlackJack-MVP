using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BlackJack.View;
using System.Threading.Tasks;

namespace BlackJack.Model
{
    public class BoardResultPresenter : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private BoardResultView _boardResultView;

        [SerializeField]
        [Header("UI������������܂ł̎���(�~���b)")]
        private int _timeToDoInit = 2000; 

        #endregion

        #region Member Variables
        #endregion

        #region Unity Methods

        private void Start()
        {
            Subscribe();
            SetEvent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// IObservable�ŃC�x���g���w��
        /// </summary>
        private void Subscribe()
        {

            BoardModel.Instance.ObservableLatestPlayerCard
                .Where(x => x.Num != 0)
                .Subscribe(_ => _boardResultView.SetPlayerHandTextNum(BoardModel.Instance.PlayerCardNum));

            BoardModel.Instance.ObservableLatestDealerCard
                .Where(x => x.Num != 0)
                .Skip(1) //��x�ڂ̃C�x���g���s���̓f�B�[���[�͂܂��J�[�h�����J���Ă��Ȃ��̂Ŗ���������
                .Subscribe(_ => _boardResultView.SetDealerHandTextNum(BoardModel.Instance.DealerCardNum));

            BoardModel.Instance.ObservableJudgeResult
                .Subscribe(_boardResultView.SetResultText);
        }

        private void SetEvent()
        {
            BoardModel.Instance.OnInitialize += () =>
            {
                _ = OnInit();
                Subscribe();
            };

            // �f�B�[���[�̃A�b�v�J�[�h�����J���ꂽ�^�C���~���O�Ńe�L�X�g�ɐ����𔽉f������
            BoardModel.Instance.OnOpenUpCard += () =>
            {
                _boardResultView.SetDealerHandTextNum(BoardModel.Instance.DealerCardNum);
            };

            // [BoardModel.DealerCardNum]�ɉ��Z����Ă��Ȃ������z�[���J�[�h�����J���Ƀe�L�X�g�ɔ��f������
            BoardModel.Instance.OnOpenHoleCard += () =>
            {
                _boardResultView.SetDealerHandTextNum(BoardModel.Instance.DealerCardNum);
            };
        }

        private async Task OnInit()
        {
            await Task.Delay(_timeToDoInit);
            _boardResultView.Init();
        }

        #endregion
    }
}