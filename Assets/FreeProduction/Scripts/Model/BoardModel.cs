using BlackJack.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

namespace BlackJack.Model
{
    /// <summary>
    /// �v���C���[��f�B�[���̎�D���Ǘ�����N���X(���f��)
    /// </summary>
    public class BoardModel : SingletonMonoBehaviour<BoardModel>
    {
        #region Properties

        /// <summary>
        /// �ŐV�̃v���C���[�̃J�[�h
        /// �Ď��\, �v���C���[�̎�D�̍X�V���ɃC�x���g�𔭐�����
        /// </summary>
        public IObservable<CardData> ObservableLatestPlayerCard => _latestPlayerCard;

        public int PlayerCardNum => _playerHandNum;

        /// <summary>
        /// �ŐV�̃v���C���[�̃J�[�h
        /// �Ď��\, �f�B�[���[�̎�D�̍X�V���ɃC�x���g�𔭐�����
        /// </summary>
        public IObservable<CardData> ObservableLatestDealerCard => _latestDealerCard;

        public int DealerCardNum => _dealerHandNum;

        /// <summary>
        /// �u���b�N�W���b�N���J�n���Ă��邩�ǂ����̃t���O
        /// true -> �J�n
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("�J�[�h���������x�̊Ԋu")]
        private float _drawDuration = 1f;

        #endregion

        #region Member Variables

        /// <summary>�ŐV�̃v���C���[�̃J�[�h</summary>
        private ReactiveProperty<CardData> _latestPlayerCard = new ReactiveProperty<CardData>();

        /// <summary>�v���C���[�̎�D</summary>
        private List<CardData> _playerHand = new List<CardData>();

        private int _playerHandIndex = 0;

        /// <summary>�v���C���[�̐���</summary>
        private int _playerHandNum = 0;

        /// <summary>�ŐV�̃f�B�[���[�̃J�[�h</summary>
        private ReactiveProperty<CardData> _latestDealerCard = new ReactiveProperty<CardData>();

        /// <summary>�f�B�[���[�̎�D</summary>
        private List<CardData> _dealerHand = new List<CardData>();

        private int _dealerHandIndex = 0;

        /// <summary>�f�B�[���[�̎�D�̐���</summary>
        private int _dealerHandNum = 0;

        /// <summary>�f�B�[���[�̕����Ă���J�[�h�̐���</summary>
        private int _dealerHoleHandNum = 0;

        #endregion

        #region Constant

        /// <summary>���ꐔ���ɂȂ�ƃo�[�X�g�����ɂȂ鐔��</summary>
        private const int BUST_NUM = 22;

        /// <summary>�u���b�N�W���b�N�����ɂȂ鐔��</summary>
        private const int BLACKJACK_NUM = 21;

        /// <summary>�f�B�[���[���g�����v�����̐����ȏ�ɂȂ�܂łɈ���������</summary>
        private const int DEALER_DRAWING_HAND_LIMIT = 17;

        /// <summary>�ςł���g�����v�uACE�v�̂��ꂼ��̐����̍�</summary>
        private const int ACE_CARD_OFFSET = 10;

        #endregion

        #region Events

        /// <summary>�{�[�h�̏������̍ۂɎ��s</summary>
        public event Action OnInitialize;

        /// <summary>�A�b�v�J�[�h���߂�����ۂɌĂяo�����</summary>
        public event Action OnOpenUpCard;

        /// <summary>�z�[���J�[�h���߂�����ۂɌĂяo�����</summary>
        public event Action OnOpenHoleCard;

        /// <summary>�v���C���[���A�N�V������I���ł���悤�ɂȂ����ۂɌĂяo�����</summary>
        public event Action OnCanSelectAction;

        #endregion

        #region Enums

        public enum Person { Player, Dealer }

        public enum DealerCardType
        {
            /// <summary>�\�����̃g�����v</summary>
            Up,
            /// <summary>�����Ă���g�����v</summary>
            Hole
        }

        #endregion

        #region Public Methods

        [ContextMenu("StartGame")]
        public void StartGame()
        {
            if (IsStarted == true) return;

            IsStarted = true;
            StartCoroutine(OnStartDrawing());
        }
        
        [ContextMenu("EndAction")]
        public void OnPlayerActionEnd()
        {
            if(IsStarted == false) return;

            StartCoroutine(OnEndDrawing());
        }

        [ContextMenu("Draw")]
        public void DrawPlayerCard()
        {
            if (IsStarted == false) return;

            _playerHand.Add(CardStackModel.Instance.CurrentCard);
            _playerHandNum += _playerHand[_playerHandIndex].Num;
            _latestPlayerCard.Value = _playerHand[_playerHandIndex];

            Debug.Log($"�v���C���[���J�[�h�������� ������������{_playerHand[_playerHandIndex].Num}"+
                $"\n���݂̐�����{_playerHandNum}");

            if(CheckBust(_playerHandNum) == true)
            {
                bool existsA11 = false;

                _playerHand = _playerHand.Select(x =>
                {
                    // �o�[�X�g�����ۂɃJ�[�h��ACE(11)���܂܂�Ă�����ACE(1)�Ƃ��ĕԂ�
                    // ��ACE�̓\�t�g�n���h�ƌĂ΂��11�Ƃ�1�Ƃ��F���ł���
                    if(x.Rank == CardData.RankType.A11)
                    {
                        existsA11 = true;
                        _playerHandNum -= ACE_CARD_OFFSET;
                        return x.ChangeRank(CardData.RankType.A1);
                    }
                    else
                    {
                        return x;
                    }
                }).ToList();

                if(existsA11 == true)
                {
                    Debug.Log($"21�𒴂�����ACE(11)���܂܂�Ă������߃n���h�̐������ύX���ꂽ" +
                        $"\n���݂̐�����{_playerHandNum}");
                    _playerHandIndex++;
                    return;
                }

                print("�v���C���[���o�[�X�g���� �v���C���[�̕���");
                OnPlayerActionEnd();
            }
            _playerHandIndex++;
        }

        #endregion

        #region Privete Methods

        private void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:

                    _dealerHand.Add(CardStackModel.Instance.CurrentCard);
                    _dealerHandNum += _dealerHand[_dealerHandIndex].Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHand.Add(CardStackModel.Instance.CurrentCard);
                    _dealerHoleHandNum = _dealerHand[_dealerHandIndex].Num;

                    break;
            }

            _latestDealerCard.Value = _dealerHand[_dealerHandIndex];

            Debug.Log($"�f�B�[���[���J�[�h�������� ������������{_dealerHand[_dealerHandIndex].Num}" +
                $"\n���݂̃A�b�v�J�[�h��{_dealerHandNum}�z�[���J�[�h��{_dealerHoleHandNum}");

            _dealerHandIndex++;
        }

        /// <summary>�Q�[���X�^�[�g���̃J�[�h����������</summary>
        IEnumerator OnStartDrawing()
        {
            DrawPlayerCard();

            yield return new WaitForSeconds(_drawDuration);

            DrawDealerCard(DealerCardType.Up);

            yield return new WaitForSeconds(_drawDuration);

            DrawPlayerCard();

            yield return new WaitForSeconds(_drawDuration);

            DrawDealerCard(DealerCardType.Hole);
            OnOpenUpCard?.Invoke();

            if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true
                && CheckBlackJack(_playerHandNum) == true)
            {
                OpenHoleCard();
                print("���҂��u���b�N�W���b�N ��������");
                Init();
                yield break;
            }
            else if (CheckBlackJack(_dealerHandNum + _dealerHoleHandNum) == true)
            {
                OpenHoleCard();
                print("�f�B�[���[���u���b�N�W���b�N �f�B�[���[�̏���");
                Init();
                yield break;
            }
            else if (CheckBlackJack(_playerHandNum) == true)
            {
                OpenHoleCard();
                print("�v���C���[���u���b�N�W���b�N �v���C���[�̏���");
                Init();
                yield break;
            }

            OnCanSelectAction?.Invoke();
        }

        /// <summary>�v���C���[�̃A�N�V�������I������ۂ̃J�[�h����������</summary>
        IEnumerator OnEndDrawing()
        {
            yield return new WaitForSeconds(_drawDuration);

            OpenHoleCard();

            // �f�B�[���[�̓n���h��17�ȏ�ɂȂ�܂ň���������
            while (_dealerHandNum < DEALER_DRAWING_HAND_LIMIT)
            {
                yield return new WaitForSeconds(_drawDuration);
                DrawDealerCard(DealerCardType.Up);
            }

            // �f�B�[�����n���h�������I������ŏI�I�ȏ��s�̔��ʂ��s��
            if (_dealerHandNum >= DEALER_DRAWING_HAND_LIMIT)
            {
                yield return new WaitForSeconds(_drawDuration);
                Judge();
            }
        }

        /// <summary>
        /// �ŏI�I�ȏ��s�̔��ʂ��s��
        /// </summary>
        private void Judge()
        {
            // �o�[�X�g�̏󋵂��m�F���ď����������m�肳����
            if (CheckBust(_dealerHandNum) == true
                && CheckBust(_playerHandNum) == true)
            {
                print("�f�B�[���[���o�[�X�g���� �������v���C���[�͂��łɃo�[�X�g���Ă���");
                Init();
                return;
            }
            else if (CheckBust(_dealerHandNum) == true)
            {
                print("�f�B�[���[���o�[�X�g���� �v���C���[�̏���");
                Init();
                return;
            }
            else if (CheckBust(_playerHandNum) == true)
            {
                print("�f�B�[���̓o�[�X�g���Ȃ����� �f�B�[���[�̏���");
                Init();
                return;
            }

            // ���҃o�[�X�g���Ă��Ȃ������琔���ŏ��s���m�肳����
            if (_playerHandNum > _dealerHandNum)
            {
                print($"�v���C���[�̏���\n�v���C���[{_playerHandNum} �f�B�[���[{_dealerHandNum}");
                Init();
            }
            else if (_playerHandNum < _dealerHandNum)
            {
                print($"�f�B�[���[�̏���\n�v���C���[{_playerHandNum} �f�B�[���[{_dealerHandNum}");
                Init();
            }
            else
            {
                print($"��������\n�v���C���[{_playerHandNum} �f�B�[���[{_dealerHandNum}");
                Init();
            }
        }

        /// <summary>
        /// �f�B�[���[�������Ă���J�[�h(�z�[���J�[�h)�����J����
        /// </summary>
        private void OpenHoleCard()
        {
            _dealerHandNum += _dealerHoleHandNum;
            _dealerHoleHandNum = 0;
            OnOpenHoleCard?.Invoke();
            print($"�f�B�[���[���z�[���J�[�h�����J����\n���݂̐�����{_dealerHandNum}");
        }

        private bool CheckBlackJack(int num)
        {
            if(num == BLACKJACK_NUM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckBust(int num)
        {
            if(num >= BUST_NUM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void Init()
        {
            // �Q�[���i�s�Ɋւ��鏉����
            IsStarted = false;

            // �v���C���[�Ɋւ��鏉����
            _latestPlayerCard.Dispose();
            _latestPlayerCard = new ReactiveProperty<CardData>();

            _playerHand = new List<CardData>();
            _playerHandNum = 0;
            _playerHandIndex = 0;


            // �f�B�[���[�Ɋւ��鏉����
            _latestDealerCard.Dispose();
            _latestDealerCard = new ReactiveProperty<CardData>();

            _dealerHand = new List<CardData>();
            _dealerHandNum = 0;
            _dealerHandIndex = 0;
            _dealerHoleHandNum = 0;

            OnInitialize?.Invoke();
        }

        #endregion
    }
}