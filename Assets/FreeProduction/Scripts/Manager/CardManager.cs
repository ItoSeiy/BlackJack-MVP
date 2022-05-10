using BlackJack.Data;
using BlackJack.Extension;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack.Manager
{
    /// <summary>
    /// �J�[�h�̃f�[�^�𐶐��ێ�����N���X
    /// </summary>
    public class CardManager : SingletonMonoBehaviour<CardManager>
    {
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

        [SerializeField]
        [Header("�g�����v�̃f�b�L�������p�ӂ��邩")]
        private int _deckNum = 3;

        /// <summary>
        /// �g�����v�̎R�D
        /// �R�D�͉ςł��邽��List���g�p
        /// </summary>
        private List<CardData> _cardStack = new List<CardData>();

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];
        
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

        protected override void Awake()
        {
            base.Awake();
            LoadSprite();
            CreateCards();
        }

        private void LoadSprite()
        {
            _cardSprites = Resources.LoadAll<Sprite>("Images/Cards");
        }

        private void CreateCards()
        {
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
    }
}