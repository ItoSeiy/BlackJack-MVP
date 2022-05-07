using BlackJack.Data;
using BlackJack.Extension;
using System;
using UnityEngine;

namespace BlackJack.Manager
{
    public class CardManager : SingletonMonoBehaviour<CardManager>
    {
        [SerializeField]
        [Header("トランプのデッキをいくつ用意するか")]
        private int _deckNum = 3;

        [SerializeField]
        [Header("カードのテンプレートのプレハブ")]
        private Card _cardPrefab;
        
        /// <summary>実際に生成を行うカード</summary>
        private Card[,] _cards;

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];
        
        /// <summary>トランプ1デッキに対するトランプの枚数</summary>
        private const int CARD_NUM = 52;

        /// <summary>スプライトの配列におけるクローバーのIndexの範囲</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_CLUB
            = new RangeValue<int>(0, 12);

        /// <summary>スプライトの配列におけるダイヤのIndexの範囲</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_DIAMOND
            = new RangeValue<int>(13, 25);

        /// <summary>スプライトの配列におけるハートのIndexの範囲</summary>
        private readonly RangeValue<int> SUIT_INDEX_RANGE_HEART
            = new RangeValue<int>(26, 38);

        /// <summary>スプライトの配列におけるのIndexの範囲</summary>
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

            for (int deckIndex = 0; deckIndex < _deckNum; deckIndex++)
            {
                for(int cardIndex = 0; cardIndex < CARD_NUM; cardIndex++)
                {
                    int num = GetNum(_cardSprites[cardIndex].name);

                    CardData.RankType rank = GetRank(_cardSprites[cardIndex].name);

                    CardData.SuitType suit = GetSuit(cardIndex);

                    _cards[deckIndex, cardIndex] = _cardPrefab
                        .SetUp(new CardData(num, rank, suit), _cardSprites[cardIndex])
                        .Show();
                }
            }
        }

        /// <summary>
        /// カードの種類をスプライトの名前から取得する
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
                Debug.LogError($"カードの種類が取得できませんでした" +
                               $"\nスプライトの名前は{spriteName}です");
                return CardData.RankType.None;
            }
        }

        /// <summary>
        /// カードのスートをIndexから取得する
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
                Debug.LogError($"トランプの生成時にスートを検出できませんでした" +
                          　　$"\n{cardIndex} は存在しません");
                return CardData.SuitType.Spade;
            }
        }

        /// <summary>
        /// カードの番号をスプライトの名前から取得する
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
                Debug.LogError($"カード番号が取得できませんでした" +
                               $"\nスプライトの名前は{spriteName}です");
                return 0;
            }
        }
    }
}