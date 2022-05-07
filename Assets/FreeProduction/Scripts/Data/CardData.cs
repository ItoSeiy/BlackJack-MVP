using UnityEngine;

namespace BlackJack.Data
{
    [System.Serializable]
    public struct CardData
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

        [SerializeField]
        [Header("計算に使うトランプの数字")]
        private int _num;

        [SerializeField]
        [Header("トランプの分類 絵柄, 数字")]
        private RankType _rank;

        [SerializeField]
        [Header("トランプのスート(マーク)")]
        private SuitType _suit;

        public CardData(int num, RankType rank, SuitType suit)
        {
            _num = num;
            _rank = rank;
            _suit = suit;

            // ずれた数字プロパティによってを補正を行う
            _num = Num;
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