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
        [Header("�v�Z�Ɏg���g�����v�̐���")]
        private int _num;

        [SerializeField]
        [Header("�g�����v�̕��� �G��, ����")]
        private RankType _rank;

        [SerializeField]
        [Header("�g�����v�̃X�[�g(�}�[�N)")]
        private SuitType _suit;

        [SerializeField]
        [Header("�g�����v�̃C���[�W")]
        private Sprite _sprite;

        public CardData(int num, RankType rank, SuitType suit, Sprite sprite)
        {
            _num = num;
            _rank = rank;
            _suit = suit;
            _sprite = sprite;

            // ���ꂽ�����v���p�e�B�ɂ���Ă�␳���s��
            _num = Num;
        }

        public CardData Show()
        {
            Debug.Log($"�X�[�g��{Suit} �G����{Rank}" +
                  $"\n�摜��{_sprite.name} ������{Num}");
            return this;
        }

        public CardData ChangeRank(RankType rank)
        {
            _rank = rank;
            Debug.Log("�J�[�h�̃����N���ύX���ꂽ");
            Show();
            return this;
        }

        public enum RankType
        {
            /// <summary>�f�t�H���g �����̂�</summary>
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