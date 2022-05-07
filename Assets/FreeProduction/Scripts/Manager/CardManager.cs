using UnityEngine;
using BlackJack.Data;

namespace BlackJack.Manager
{
    public class CardManager : SingletonMonoBehaviour<CardManager>
    {
        [SerializeField]
        [Header("トランプのデッキをいくつ用意するか")]
        private int _deckNum = 3;

        private Card[,] _cards;

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];

        private const int CARD_NUM = 52;

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
                    CardData.RankType rankType = GetRank(cardIndex);
                }
            }
        }

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
        
        //private CardData.SuitType GetSuit(int cardIndex)
        //{
        //}
    }
}