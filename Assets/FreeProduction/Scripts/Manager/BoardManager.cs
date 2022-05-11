using BlackJack.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack.Manager
{
    /// <summary>
    /// �v���C���[��f�B�[���̎�D���Ǘ�����N���X
    /// </summary>
    public class BoardManager : SingletonMonoBehaviour<BoardManager>
    {
        #region Properties

        public int PlayerCardNum => _playerCardNum;

        public int DealerCardNum => _dealerCardNum;

        #endregion

        #region Inspector Variables

        [SerializeField]
        private float _drawDuration = 0.4f;

        #endregion

        #region Member Variables

        /// <summary>�v���C���[�̎�D</summary>
        private List<CardData> _playerHand = new List<CardData>();

        private int _playerHandIndex = 0;

        /// <summary>�v���C���[�̐���</summary>
        private int _playerCardNum = 0;


        /// <summary>�f�B�[���[�̎�D</summary>
        private List<CardData> _dealerHand = new List<CardData>();

        private int _dealerHandIndex = 0;

        /// <summary>�f�B�[���[�̐���</summary>
        private int _dealerCardNum = 0;

        /// <summary>�f�B�[���[�̕����Ă���J�[�h�̐���</summary>
        private int _dealerHoleCardNum = 0;

        #endregion

        #region Constant

        /// <summary>���ꐔ���ɂȂ�ƃo�[�X�g�����ɂȂ鐔��</summary>
        private const int BUST_NUM = 22;

        /// <summary>�u���b�N�W���b�N�����ɂȂ鐔��</summary>
        private const int BLACKJACK_NUM = 21;

        /// <summary>�f�B�[���[���g�����v�����̐����ȏ�ɂȂ�܂łɈ���������</summary>
        private const int DEALER_DRAWING_HAND_LIMIT = 17;

        #endregion

        #region Events

        /// <summary>�ϐ��ɕϓ����������ۂɌĂ΂��</summary>
        public event Action OnVariablesChange;

        #endregion

        #region UnityMethods

        protected override void Awake()
        {
            base.Awake();
            CardManager.Instance.OnCreateEnd += StartGame;
        }

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
            StartCoroutine(OnStartDrawing());
        }
        
        [ContextMenu("EndAction")]
        public void OnPlayerActionEnd()
        {
            StartCoroutine(OnEndDrawing());
        }

        [ContextMenu("Draw")]
        public void DrawPlayerCard()
        {
            _playerHand.Add(CardManager.Instance.CurrentCard);

            Debug.Log($"�v���C���[���J�[�h��������\n���݂̐�����{_playerCardNum}");

            if(CheckBust(_playerCardNum) == true)
            {
                print("�v���C���[���o�[�X�g���� �v���C���[�̕���");
                OnPlayerActionEnd();
            }
            _playerHandIndex++;
        }

        #endregion

        #region Privete Methods

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

            if (CheckBlackJack(_dealerCardNum + _dealerHoleCardNum) == true
                && CheckBlackJack(_playerCardNum) == true)
            {
                print("���҂��u���b�N�W���b�N ��������");
                Init();
            }
            else if (CheckBlackJack(_dealerCardNum + _dealerHoleCardNum) == true)
            {
                print("�f�B�[���[���u���b�N�W���b�N �f�B�[���[�̏���");
                Init();
            }
            else if (CheckBlackJack(_playerCardNum) == true)
            {
                print("�v���C���[���u���b�N�W���b�N �v���C���[�̏���");
                Init();
            }
        }

        /// <summary>�v���C���[�̃A�N�V�������I������ۂ̃J�[�h����������</summary>
        IEnumerator OnEndDrawing()
        {
            while(_dealerCardNum >= DEALER_DRAWING_HAND_LIMIT)
            {
                DrawDealerCard(DealerCardType.Up);
                yield return new WaitForSeconds(_drawDuration);
            }
        }

        private void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:

                    _dealerCardNum += CardManager.Instance.CurrentCard.Num;

                    break;

                case DealerCardType.Hole:

                    _dealerHoleCardNum = CardManager.Instance.CurrentCard.Num;

                    break;
            }

            Debug.Log($"�f�B�[���[���J�[�h��������\n���݂̃A�b�v�J�[�h��{_dealerCardNum}" +
                $"�z�[���J�[�h��{_dealerHoleCardNum}");

            if(CheckBust(_dealerCardNum) == true 
                && CheckBust(_playerCardNum) == true)
            {
                print("�f�B�[���[���o�[�X�g���� �������v���C���[�͂��łɃo�[�X�g���Ă���");
                Init();
            }
            else if(CheckBust(_dealerCardNum) == true)
            {
                print("�f�B�[���[���o�[�X�g���� �v���C���[�̏���");
                Init();
            }
        }

        private void ShowHoleCard()
        {
            _dealerCardNum += _dealerHoleCardNum;
            print($"�f�B�[���[���z�[���J�[�h�����J����\n���݂̐�����{_dealerCardNum}");
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
            _playerCardNum = 0;
            _dealerCardNum = 0;
            _dealerHoleCardNum = 0;
        }

        #endregion

    }
}