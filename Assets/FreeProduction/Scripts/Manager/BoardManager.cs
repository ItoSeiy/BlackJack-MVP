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
        private int _dealerCardNum;
        private int _playerCardNum;

        [ContextMenu("Player")]
        public void DrawPlayerCard()
        {
            _playerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"�v���C���[���J�[�h��������\n���݂̐�����{_playerCardNum}");

        }

        [ContextMenu("Dealer")]
        public void DrawDealerCard()
        {
            _dealerCardNum += CardManager.Instance.CurrentCard.Num;
            Debug.Log($"�f�B�[���[���J�[�h��������\n���݂̐�����{_dealerCardNum}");
        }

        public enum Person
        {
            Player,
            Dealer
        }
    }
}