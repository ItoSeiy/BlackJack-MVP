using BlackJack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackJack.Model
{
    /// <summary>
    /// �J�[�h�̃f�[�^�𐶐��ێ�����N���X(���f��)
    /// </summary>
    public class CardStackModel : SingletonMonoBehaviour<CardStackModel>
    {
        #region Properties

        /// <summary>
        /// �R�D�̈�ԏ�̃g�����v
        /// </summary>
        public CardData CurrentCard
        {
            get
            {
                if(_cardStack != null)
                {
                    // �����l�� -1 �̂��ߐ�ɃC���N�������g���\
                    _cardStackIndex++;
                    //print($"Index{_cardStackIndex}�̃J�[�h�������܂�");

                    // �J�E���e�B���O(�s��)�h�~�̂��߂ɗ]�T�������ăg�����v���Đ�������
                    if(_cardStackIndex > _cardStack.Count - CARD_NUM)
                    {
                        print("�R�D���c��1�f�b�L�ɂȂ�܂��� �ēx�R�D�𐶐����܂�");
                        CreateCards();
                        _cardStackIndex = 0;
                    }

                    return _cardStack[_cardStackIndex];
                }
                else
                {
                    Debug.Log($"�f�b�L���쐬����Ă��Ȃ��܂��͋�̉\��������܂�");
                    return default;
                }
            }
        }

        /// <summary>
        /// �R�D 
        /// ���łɈ����ꂽ�J�[�h�͏��O����Ă��Ȃ�
        /// </summary>
        public List<CardData> CardStack
        {
            get
            {
                if(_cardStack != null)
                {
                    return _cardStack;
                }
                else
                {
                    Debug.Log($"�f�b�L���쐬����Ă��Ȃ��܂��͋�̉\��������܂�");
                    return null;
                }
            }
        }

        /// <summary>�R�D��Index�̎Q��</summary>
        public int CardStackIndex => _cardStackIndex;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("�g�����v�̃f�b�L�������p�ӂ��邩")]
        private int _deckNum = 3;

        #endregion

        #region Member Variables

        /// <summary>
        /// �g�����v�̎R�D
        /// �R�D�͉�(�V���b�t����)�ł��邽��List�ɂ���
        /// </summary>
        private List<CardData> _cardStack = null;
        
        /// <summary>�R�D����g�����v�������ۂɎg�p����Index</summary>
        private int _cardStackIndex = -1;

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];

        #endregion

        #region Constant

        /// <summary>�g�����v1�f�b�L�ɑ΂���g�����v�̖���</summary>
        private const int CARD_NUM = 52;

        /// <summary>�X�v���C�g�̔z��ɂ�����N���[�o�[��Index�͈̔�</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_CLUB
            = new RangeValue<int>(0, 12);

        /// <summary>�X�v���C�g�̔z��ɂ�����_�C����Index�͈̔�</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_DIAMOND
            = new RangeValue<int>(13, 25);

        /// <summary>�X�v���C�g�̔z��ɂ�����n�[�g��Index�͈̔�</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_HEART
            = new RangeValue<int>(26, 38);

        /// <summary>�X�v���C�g�̔z��ɂ������Index�͈̔�</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_SPADE
            = new RangeValue<int>(39, 51);

        #endregion

        #region Events

        /// <summary>�R�D�̐������I�������ɌĂяo�����</summary>
        public event Action OnCreateEnd;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            LoadSprite();
            CreateCards();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Privete Methods

        private void LoadSprite()
        {
            _cardSprites = Resources.LoadAll<Sprite>("Images/Cards");
        }

        private void CreateCards()
        {
            _cardStack = new List<CardData>();

            int tempIndex = 0;

            for (int cardIndex = 0; cardIndex < _deckNum * CARD_NUM; cardIndex++)
            {
                if(tempIndex == CARD_NUM -1)
                {
                    tempIndex = 0;
                }

                int num = GetNum(_cardSprites[tempIndex].name);

                CardData.RankType rank = GetRank(_cardSprites[tempIndex].name);

                CardData.SuitType suit = GetSuit(tempIndex);

                _cardStack.Add(new CardData(num, rank, suit, _cardSprites[tempIndex]));

                tempIndex++;
            }
            // �f�b�L�̃V���b�t��
            _cardStack = _cardStack.OrderBy(_ => Guid.NewGuid()).ToList();

            OnCreateEnd?.Invoke();
        }

        /// <summary>
        /// �J�[�h�̎�ނ��X�v���C�g�̖��O����擾����
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        private CardData.RankType GetRank(string spriteName)
        {
            if (int.TryParse(spriteName.Split('_')[1], out int num))
            {
                switch (num)
                {
                    case 0:
                        return CardData.RankType.A11;

                    case 10:
                        return CardData.RankType.J;

                    case 11:
                        return CardData.RankType.Q;
                    
                    case 12:
                        return CardData.RankType.K;

                    default : return CardData.RankType.None;
                }
            }
            else
            {
                Debug.LogError($"�J�[�h�̎�ނ��擾�ł��܂���ł���" +
                               $"\n�X�v���C�g�̖��O��{spriteName}�ł�");
                return CardData.RankType.None;
            }
        }

        /// <summary>
        /// �J�[�h�̃X�[�g��Index����擾����
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        private CardData.SuitType GetSuit(int cardIndex)
        {
            if (cardIndex <= SUIT_INDEX_RANGE_CLUB.End)
            {
                return CardData.SuitType.Club;
            }
            else if(cardIndex <= SUIT_INDEX_RANGE_DIAMOND.End)
            {
                return CardData.SuitType.Diamond;
            }
            else if(cardIndex <= SUIT_INDEX_RANGE_HEART.End)
            {
                return CardData.SuitType.Heart;
            }
            else if(cardIndex <= SUIT_INDEX_RANGE_SPADE.End)
            {
                return CardData.SuitType.Spade;
            }
            else
            {
                Debug.LogError($"�g�����v�̐������ɃX�[�g�����o�ł��܂���ł���" +
                          �@�@$"\n{cardIndex} �͑��݂��܂���");
                return CardData.SuitType.Spade;
            }
        }

        /// <summary>
        /// �J�[�h�̔ԍ����X�v���C�g�̖��O����擾����
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        private int GetNum(string spriteName)
        {
            if (int.TryParse(spriteName.Split('_')[1], out int num))
            {
                return num + 1;
            }
            else
            {
                Debug.LogError($"�J�[�h�ԍ����擾�ł��܂���ł���" +
                               $"\n�X�v���C�g�̖��O��{spriteName}�ł�");
                return 0;
            }
        }

        #endregion
    }
}