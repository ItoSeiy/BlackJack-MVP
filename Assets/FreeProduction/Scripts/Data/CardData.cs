using UnityEngine;
using UnityEngine.UI;
using System;

namespace BlackJack.Data
{
    [Serializable]
    public class CardData
    {
        public int Num =>
            _rank == RankType.A1 ? 1 :
            _rank == RankType.A11 ? 11 :
            _rank == RankType.J ? 10 :
            _rank == RankType.Q ? 10 :
            _rank == RankType.K ? 10 :
            _num;

        public RankType Rank => _rank;

        public SuitType Suit => _suit;

        public Sprite Sprite => _sprite;

        [SerializeField]
        [Header("計算に使うトランプの数字")]
        private int _num;

        [SerializeField]
        [Header("トランプの分類 絵柄, 数字")]
        private RankType _rank;

        [SerializeField]
        [Header("トランプのスート(マーク)")]
        private SuitType _suit;

        [SerializeField]
        [Header("トランプのイメージ")]
        private Sprite _sprite;

        public CardData(int num, RankType rank, SuitType suit, Sprite sprite)
        {
            _num = num;
            _rank = rank;
            _suit = suit;
            _sprite = sprite;

            // ずれた数字プロパティによってを補正を行う
            _num = Num;
        }

        public CardData Show()
        {
            Debug.Log($"スートは{Suit} 絵柄は{Rank}" +
                  $"\n画像は{_sprite.name} 数字は{Num}");
            return this;
        }

        public CardData ChangeRank(RankType rank)
        {
            _rank = rank;
            Debug.Log("カードのランクが変更された");
            Show();
            return this;
        }

        public enum RankType
        {
            /// <summary>デフォルト 数字のみ</summary>
            None,

            A1,
            A11,
            J,
            Q,
            K
        }

        public enum SuitType
        {
            Club,
            Diamond,
            Heart,
            Spade
        }
    }
}