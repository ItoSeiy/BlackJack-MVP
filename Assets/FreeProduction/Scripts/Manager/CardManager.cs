using UnityEngine;
using BlackJack.Data;

namespace BlackJack.Manager
{
    public class CardManager : SingletonMonoBehaviour<CardManager>
    {
        [SerializeField]
        [Header("�g�����v�̃f�b�L�������p�ӂ��邩")]
        private int _deckNum = 3;

        [SerializeField]
        private Card _cardPrefab;

        private Card[,] _cards;

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];

        private const int CARD_NUM = 52;

        private readonly RangeValue<int> SUIT_INDEX_RANGE_CLUB
            = new RangeValue<int>(0, 12);

        private readonly RangeValue<int> SUIT_INDEX_RANGE_DIAMOND
            = new RangeValue<int>(13, 25);

        private readonly RangeValue<int> SUIT_INDEX_RANGE_HEART
            = new RangeValue<int>(26, 38);

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
            _cards = new Card[_deckNum, CARD_NUM];


            for(int deckIndex = 0; deckIndex < _deckNum; deckIndex++)
            {
                for(int cardIndex = 0; cardIndex < CARD_NUM; cardIndex++)
                {
                    CardData.RankType rank = GetRank(cardIndex);
                    CardData.SuitType suit = GetSuit(cardIndex);

                    int num = GetNum(_cardSprites[cardIndex].name);

                    Instantiate(_cards[deckIndex, cardIndex]);
                }
            }
        }

        /// <summary>
        /// �J�[�h�̎�ނ�Index����擾����
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        private CardData.RankType GetRank(int cardIndex)
        {
            switch (cardIndex)
            {
                case  0: return CardData.RankType.A11;

                case 10: return CardData.RankType.J;

                case 11: return CardData.RankType.Q;

                case 12: return CardData.RankType.K;

                default: return CardData.RankType.DefaultNum;
            }
        }

        /// <summary>
        /// �J�[�h�̃X�[�g��Index����擾����
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        private CardData.SuitType GetSuit(int cardIndex)
        {
            if (cardIndex < SUIT_INDEX_RANGE_CLUB.End)
            {
                return CardData.SuitType.Club;
            }
            else if(cardIndex < SUIT_INDEX_RANGE_DIAMOND.End)
            {
                return CardData.SuitType.Diamond;
            }
            else if(cardIndex < SUIT_INDEX_RANGE_HEART.End)
            {
                return CardData.SuitType.Heart;
            }
            else if(cardIndex < SUIT_INDEX_RANGE_SPADE.End)
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
            var spriteIndex = int.Parse(spriteName.Split('_')[1]);

            // ACE�̏ꍇ�̔ԍ�
            if(spriteIndex == 0)
            {
                return 11;
            }
            // J,Q,K�̏ꍇ�̔ԍ�
            else if(spriteIndex > 10)
            {
                return 10;
            }
            // ���̑�,�ʏ�̃g�����v�̏ꍇ�̔ԍ�
            else
            {
                return spriteIndex + 1;
            }

        }
    }
}