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
        public int PlayerCardNum => _playerCardNum;

        public int DealerCardNum => _dealerCardNum;

        /// <summary>�v���C���[�̐���</summary>
        private int _playerCardNum;

        /// <summary>�f�B�[���[�̐���</summary>
        private int _dealerCardNum;

        /// <summary>�f�B�[���[�̕����Ă���J�[�h�̐���</summary>
        private int _dealerHoleCardNum;


        public enum Person { Player, Dealer}

        public enum DealerCardType
        {
            /// <summary>�\�����̃g�����v</summary>
            Up,
            /// <summary>�����Ă���g�����v</summary>
            Hole
        }

        public void StartGame()
        {
            DrawPlayerCard();
            DrawDealerCard(DealerCardType.Up);

            DrawPlayerCard();
            DrawDealerCard(DealerCardType.Hole);
        }

        [ContextMenu("Player")]
        public void DrawPlayerCard()
        {
            _playerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"�v���C���[���J�[�h��������\n���݂̐�����{_playerCardNum}");
        }

        [ContextMenu("Dealer")]
        public void DrawDealerCard(DealerCardType cardType)
        {
            switch (cardType)
            {
                case DealerCardType.Up:



                    break;

                case DealerCardType.Hole:

                    _dealerHoleCardNum = CardManager.Instance.CurrentCard.Num;

                    break;
            }

            _dealerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"�f�B�[���[���J�[�h��������\n���݂̐�����{_dealerCardNum}");
        }
    }
}