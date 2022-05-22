using BlackJack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackJack.Model
{
    /// <summary>
    /// カードのデータを生成保持するクラス(モデル)
    /// </summary>
    public class CardStackModel : SingletonMonoBehaviour<CardStackModel>
    {
        #region Properties

        /// <summary>
        /// 山札の一番上のトランプ
        /// </summary>
        public CardData CurrentCard
        {
            get
            {
                if(_cardStack != null)
                {
                    // 初期値が -1 のため先にインクリメントが可能
                    _cardStackIndex++;
                    //print($"Index{_cardStackIndex}のカードを引きます");

                    // カウンティング(不正)防止のために余裕をもってトランプを再生成する
                    if(_cardStackIndex > _cardStack.Count - CARD_NUM)
                    {
                        print("山札が残り1デッキになりました 再度山札を生成します");
                        CreateCards();
                        _cardStackIndex = 0;
                    }

                    return _cardStack[_cardStackIndex];
                }
                else
                {
                    Debug.Log($"デッキが作成されていないまたは空の可能性があります");
                    return default;
                }
            }
        }

        /// <summary>
        /// 山札 
        /// すでに引かれたカードは除外されていない
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
                    Debug.Log($"デッキが作成されていないまたは空の可能性があります");
                    return null;
                }
            }
        }

        /// <summary>山札のIndexの参照</summary>
        public int CardStackIndex => _cardStackIndex;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("トランプのデッキをいくつ用意するか")]
        private int _deckNum = 3;

        #endregion

        #region Member Variables

        /// <summary>
        /// トランプの山札
        /// 山札は可変(シャッフル等)であるためListにした
        /// </summary>
        private List<CardData> _cardStack = null;
        
        /// <summary>山札からトランプを引く際に使用するIndex</summary>
        private int _cardStackIndex = -1;

        private Sprite[] _cardSprites = new Sprite[CARD_NUM];

        #endregion

        #region Constant

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

        #endregion

        #region Events

        /// <summary>山札の生成が終わった後に呼び出される</summary>
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
            // デッキのシャッフル
            _cardStack = _cardStack.OrderBy(_ => Guid.NewGuid()).ToList();

            OnCreateEnd?.Invoke();
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

        #endregion
    }
}